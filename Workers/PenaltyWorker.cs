using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Cronos;
using FourtitudeIntegrated.Controllers;
using FourtitudeIntegrated.DbContexts;
using FourtitudeIntegrated.Enum;
using FourtitudeIntegrated.Models;
using Microsoft.EntityFrameworkCore;

namespace FourtitudeIntegrated.Workers
{
    public class PenaltyWorker : BackgroundService
    {
        private readonly ILogger _logger;
        private Timer? _timer;
        //private readonly FourtitudeIntegratedContext _context;

        private CancellationTokenSource _cancellationTokenSource;
        private readonly string _cronExpression = "0 0 * * *"; // Example: Run at 12AM every day
        //private readonly string _cronExpression = "*/1 * * * *";

        public PenaltyWorker(ILogger<PenaltyWorker> logger) //, FourtitudeIntegratedContext context)
        {
            _logger = logger;
            //_context = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Penalty Worker started at: {time}", DateTimeOffset.Now);
            _cancellationTokenSource = new CancellationTokenSource();

            var cronExpression = CronExpression.Parse(_cronExpression);
            var nextOccurrence = cronExpression.GetNextOccurrence(DateTimeOffset.UtcNow, TimeZoneInfo.Local);

            while (!stoppingToken.IsCancellationRequested)
            {
                var delay = nextOccurrence - DateTimeOffset.UtcNow;

                if (delay > TimeSpan.Zero)
                {
                    await Task.Delay((TimeSpan)delay, stoppingToken);
                }

                if (!stoppingToken.IsCancellationRequested)
                {
                    await RunJob();
                    nextOccurrence = cronExpression.GetNextOccurrence(DateTimeOffset.UtcNow, TimeZoneInfo.Local);
                }
            }
            _logger.LogInformation("Penalty Worker stopped at: {time}", DateTimeOffset.Now);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();

            return base.StopAsync(cancellationToken);
        }


        private async Task RunJob()
        {
            Console.WriteLine("Updating Penalties contributions.");

            string url = null;

            using (var client = new HttpClient())
            {
                try
                {
                    //Update contribution penalties
                    url = "https://localhost:7159/api/Contributions/"; 
                    
                    var res = await client.GetAsync(url);
                    var content = await res.Content.ReadAsStringAsync();

                    List<ContributionsDTODetails> contributions = JsonSerializer.Deserialize<List<ContributionsDTODetails>>(content).Where(contribution => !contribution.Status.Equals(Enum.ContributionStatus.Cleared.ToString()) && contribution.DateDue < DateTime.Now).ToList();

                    //var contributions = await _context.Contributions.Where(contribution => contribution.Status != Enum.ContributionStatus.Cleared && contribution.DateDue < DateTime.Now).ToListAsync();

                    foreach (ContributionsDTODetails contribution in contributions)
                    {
                        contribution.PenaltyDue = contribution.PenaltyDue + contribution.Balance * 0.01M;

                        var updated = new ContributionsDTO()
                        {
                            AccountId = contribution.AccId,
                            AmountDue= contribution.AmountDue,
                            AmountPaid= contribution.AmountPaid,
                            ContributionId= contribution.ContributionId,
                            DateDue= contribution.DateDue,
                            DateOfLastPayment= contribution.DateOfLastPayment,
                            PenaltyDue= contribution.PenaltyDue,
                            Status = contribution.Status.Equals(ContributionStatus.Partial.ToString()) ? ContributionStatus.Partial : ContributionStatus.Unpaid
                        };

                        try
                        {
                            // Convert the request body to JSON
                            var jsonRequestBody = new StringContent(
                                System.Text.Json.JsonSerializer.Serialize(updated),
                                System.Text.Encoding.UTF8,
                                "application/json"
                            );

                            url = $"https://localhost:7159/api/Contributions/{contribution.ContributionId}";
                            HttpResponseMessage response = await client.PutAsync(url, jsonRequestBody);

                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                Console.WriteLine(responseBody);
                            }
                            else
                            {
                                Console.WriteLine($"HTTP Error: {response.StatusCode}");
                            }
                        }
                        catch(Exception ex)
                        {
                            _logger.LogError(ex.Message);
                        }
                    }

                    //Update loan penalties
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            }
            Console.WriteLine("Updating Penalties contributions.");
        }

    }
}