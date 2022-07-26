using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace StorageApp
{
    class Program
    {

        static async Task Main(string[] args)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient("removing url");

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("myfirstcontainer");

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobServiceClient.Uri);
            BlobClient blobClient = containerClient.GetBlobClient("sample_XLSX");

            await blobClient.UploadAsync(BinaryData.FromString("hello world"), overwrite: true);
            await ListBlobsFlatListing(containerClient, 100);
        }


        private static async Task ListBlobsFlatListing(BlobContainerClient blobContainerClient,
            int? segmentSize)
        {
            try
            {
                // Call the listing operation and return pages of the specified size.
                var resultSegment = blobContainerClient.GetBlobsAsync()
                    .AsPages(default, segmentSize);

                // Enumerate the blobs returned for each page.
                await foreach (Azure.Page<BlobItem> blobPage in resultSegment)
                {
                    foreach (BlobItem blobItem in blobPage.Values)
                    {
                        Console.WriteLine("Blob name: {0}", blobItem.Name);
                    }

                    Console.WriteLine();
                }
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }
    }
}