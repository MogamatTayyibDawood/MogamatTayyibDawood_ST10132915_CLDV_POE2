# CLDV6212POEFunctionApp

Overview

This Azure Functions application is part of the CLDV6212 Portfolio of Evidence (PoE) project. It integrates several Azure services, including Azure Queue Storage, Azure Table Storage, Azure Blob Storage, and Azure File Storage. The application provides HTTP-triggered functions that enable users to store data, upload files, and manage message queues, facilitating scalable cloud-based operations.
Prerequisites

Before running this application, ensure you have the following:

    .NET 6 or later: The application is built using .NET and requires the SDK installed on your machine.
    Azure Subscription: You will need an Azure account to create and manage Azure resources.
    Azure Storage Account: Create a storage account to use Azure Storage services like Blobs, Tables, and Queues.

Setup Instructions

    Clone the Repository: Begin by cloning the repository containing the application code:

    bash

git clone <repository-url>
cd CLDV6212POEFunctionApp

Configure Azure Storage Connection String:

    Set the connection string for your Azure Storage account in your environment variables. This connection string allows the application to interact with the Azure Storage services.
    The key for this environment variable should be AzureStorage:ConnectionString.

Install Required NuGet Packages: Ensure that you have the necessary NuGet packages installed to access Azure services:

    Microsoft.Azure.Functions.Extensions
    Microsoft.Extensions.Hosting
    Azure.Storage.Queues
    Azure.Data.Tables
    Azure.Storage.Blobs
    Azure.Storage.Files.Shares

You can install these packages using the .NET CLI:

bash

dotnet add package Microsoft.Azure.Functions.Extensions
dotnet add package Microsoft.Azure.WebJobs.Extensions.Storage.Blobs
dotnet add package Microsoft.Azure.WebJobs.Extensions.Storage.Queues
dotnet add package Azure.Storage.Queues
dotnet add package Azure.Data.Tables
dotnet add package Azure.Storage.Blobs
dotnet add package Azure.Storage.Files.Shares

Build the Application: Compile the application to ensure all dependencies are resolved:

bash

dotnet build

Run the Application: To run the function app locally, execute the following command:

bash

    func start

Functions Overview
1. ProcessQueueMessage

This function allows users to add messages to an Azure Storage Queue. It validates input parameters to ensure that both the queue name and message content are provided. Upon successful execution, it sends the message to the specified queue.

2. StoreTableInfo

This function is responsible for storing data in an Azure Table. It requires the specification of the table name, partition key, row key, and the actual data to be stored. The function checks for valid input and creates the table if it does not already exist. After storing the data, it confirms successful operation.

3. UploadBlob

This function enables users to upload files to Azure Blob Storage. Users must provide the container name and the desired blob name for the file. The function checks for the presence of a file in the request, uploads it to the specified container, and handles any errors related to missing input.

4. UploadFile

This function uploads files to Azure File Storage. It requires the user to specify the name of the file share and the file name. The function creates the share if it does not exist and uploads the specified file. It also includes error handling for invalid input parameters.
Error Handling

Each function includes error handling to provide meaningful feedback when input validation fails. Common error responses include missing required parameters, allowing users to correct their requests effectively.

Additional Information

    Logging: The application uses logging capabilities to capture relevant information during function execution, which is helpful for debugging and monitoring purposes.
    Scalability: Leveraging Azure Functions enables automatic scaling based on demand, allowing the application to handle varying loads efficiently.

License

This project is licensed under the MIT License. For more details, please refer to the LICENSE file.
