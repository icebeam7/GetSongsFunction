using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace GetSongsFunction
{
    public class Song : TableEntity
    {
        public string SongName { get; set; }
        public string SongAlbum { get; set; }
    }

    public static class GetSongsFunction
    {
        [FunctionName("GetSongsFunction")]
        [StorageAccount("AzureWebJobsStorage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Table("SongsTable", Connection = "AzureWebJobsStorage")] CloudTable table,
            ILogger log)
        {
            log.LogInformation("Getting information from Songs Table!");

            var tableQuery = new TableQuery<Song>();

            var tableContents = await table.ExecuteQuerySegmentedAsync(tableQuery, null);

            return new OkObjectResult(tableContents.Results);
        }
    }
}
