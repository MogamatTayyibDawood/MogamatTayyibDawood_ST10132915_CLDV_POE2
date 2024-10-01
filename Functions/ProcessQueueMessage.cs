
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CLDV6212POEFunctionApp.Functions
{
    public static class ProcessQueueMessage
    {
        [Function("ProcessQueueMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string queueName = req.Query["queueName"];
            string message = req.Query["message"];

            // Validate input
            if (string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(message))
            {
                return new BadRequestObjectResult("Queue name and message must be provided.");
            }

            // Initialise Queue Client
            var connectionString = Environment.GetEnvironmentVariable("AzureStorage:ConnectionString");
            var queueServiceClient = new QueueServiceClient(connectionString);
            var queueClient = queueServiceClient.GetQueueClient(queueName);

            // Ensure queue exists
            await queueClient.CreateIfNotExistsAsync();

            // Add message to queue
            await queueClient.SendMessageAsync(message);

            return new OkObjectResult("Message added to queue");
        }
    }
}//xhanges