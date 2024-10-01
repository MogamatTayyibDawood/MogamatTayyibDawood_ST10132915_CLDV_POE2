using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace CLDV6212POEFunctionApp.Functions
{
    public static class UploadBlob
    {
        [Function("UploadBlob")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string containerName = req.Query["containerName"];
            string blobName = req.Query["blobName"];
            var file = req.Form.Files.GetFile("file");

            // Validate input
            if (string.IsNullOrEmpty(containerName) || string.IsNullOrEmpty(blobName) || file == null)
            {
                return new BadRequestObjectResult("Container name, blob name, and file must be provided.");
            }

            // Create Blob Service Client and Blob Container Client
            var connectionString = Environment.GetEnvironmentVariable("AzureStorage:ConnectionString");
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Ensure the container exists
            await containerClient.CreateIfNotExistsAsync();

            // Upload file as a blob
            var blobClient = containerClient.GetBlobClient(blobName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return new OkObjectResult("File uploaded to Blob Storage");
        }
    }
}
