using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CLDV6212POEFunctionApp.Functions
{
    //Add function to store information in Azure tables
    public static class StoreTableInfo
    {
        [Function("StoreTableInfo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // Extract parameters from the request
            string tableName = req.Query["tableName"];
            string partitionKey = req.Query["partitionKey"];
            string rowKey = req.Query["rowKey"];
            string data = req.Query["data"];

            // Validate inputs
            //Add error handling for StoreTableInfo function
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(partitionKey) || string.IsNullOrEmpty(rowKey) || string.IsNullOrEmpty(data))
            {
                return new BadRequestObjectResult("Table name, partition key, row key, and data must be provided.");
            }

            // Initialise Table Service Client and Table Client
            var connectionString = Environment.GetEnvironmentVariable("AzureStorage:ConnectionString");
            var serviceClient = new TableServiceClient(connectionString);
            var tableClient = serviceClient.GetTableClient(tableName);
           
            // Create table if not exists
            await tableClient.CreateIfNotExistsAsync();

            // Create a new table entity with the provided data
            var entity = new TableEntity(partitionKey, rowKey) { ["Data"] = data };

            // Add entity to table
            await tableClient.AddEntityAsync(entity);

            return new OkObjectResult("Data added to table");
        }
    }
}
