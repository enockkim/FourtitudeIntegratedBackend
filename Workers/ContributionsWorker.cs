using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Cronos;
using FourtitudeIntegrated.Controllers;
using FourtitudeIntegrated.Models;

namespace FourtitudeIntegrated.Workers
{
    public class ContributionsWorker : BackgroundService
    {
        private readonly ILogger _logger;
        private Timer? _timer;

        private CancellationTokenSource _cancellationTokenSource;
        private readonly string _cronExpression = "0 0 1 * *"; // Example: Run at 1 AM every day
        //private readonly string _cronExpression = "*/2 * * * *";


        public ContributionsWorker(ILogger<ContributionsWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Contributions Worker started at: {time}", DateTimeOffset.Now);
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
            _logger.LogInformation("Contributions Worker stopped at: {time}", DateTimeOffset.Now);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();

            return base.StopAsync(cancellationToken);
        }


        private async Task RunJob()
        {
            // Your logic here
            Console.WriteLine("Creating contributions.");
            using (var client = new HttpClient())
            {

                try
                {
                    string url = "https://localhost:7159/api/Accounts";

                    var res = await client.GetAsync(url);
                    var content = await res.Content.ReadAsStringAsync();

                    List<GetAccountsDto> accounts = JsonSerializer.Deserialize<List<GetAccountsDto>>(content).Where(acc => acc.AccCategory != "Default" && acc.AccCategory != "Director").ToList();

                    foreach (var account in accounts)
                    {

                        url = "https://localhost:7159/api/Contributions";

                        int currentMonth = Convert.ToInt16(DateTime.Now.ToString("MM"));
                        int currentYear = Convert.ToInt16(DateTime.Now.ToString("yy"));
                        int contributionId = Convert.ToInt32($"{account.AccId}{currentMonth}{currentYear}");

                        // Create the request body as needed
                        var requestBody = new ContributionsDTO
                        {
                            ContributionId = contributionId,
                            AccountId = account.AccId,
                            AmountDue = 770,
                            PenaltyDue = 0,
                            AmountPaid = 0,
                            Status = 0,
                            DateDue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 6, 0, 0, 0)
                        };

                        // Convert the request body to JSON
                        var jsonRequestBody = new StringContent(
                            System.Text.Json.JsonSerializer.Serialize(requestBody),
                            System.Text.Encoding.UTF8,
                            "application/json"
                        );

                        HttpResponseMessage response = await client.PostAsync(url, jsonRequestBody);

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
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            }
            Console.WriteLine("Contributions created.");
        }

    }
}