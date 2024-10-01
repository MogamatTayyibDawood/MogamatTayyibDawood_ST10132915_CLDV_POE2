using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace CLDV6212POEFunctionApp.Functions
{
    public static class UploadFile
    {
        [Function("UploadFile")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string shareName = req.Query["shareName"];
            string fileName = req.Query["fileName"];

            // Validate input
            if (string.IsNullOrEmpty(shareName) || string.IsNullOrEmpty(fileName))
            {
                return new BadRequestObjectResult("Share name and file name must be provided.");
            }

            // Initialize File Share Client
            var connectionString = Environment.GetEnvironmentVariable("AzureStorage:ConnectionString");
            var shareServiceClient = new ShareServiceClient(connectionString);
            var shareClient = shareServiceClient.GetShareClient(shareName);

            // Ensure share exists
            await shareClient.CreateIfNotExistsAsync();

            // Upload file to Azure Files
            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);

            using var stream = req.Body;
            await fileClient.CreateAsync(stream.Length);
            await fileClient.UploadAsync(stream);

            return new OkObjectResult("File uploaded to Azure Files");
        }
    }
}
