using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HeadlessProjectv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlphaWorkaroundController : ControllerBase
    {
        [SwaggerOperation(Summary = "The button to index data to the search engine is missing.")]
        [HttpPost("PostScratchIndexRequest")]
        public string PostScratchIndexRequest()
        {
            var host = HttpContext.Request.Host.Host;
            var port = HttpContext.Request.Host.Port;

            var client = new HttpClient();

            //TODO: call the transaction API
            using var request = new HttpRequestMessage(
                new HttpMethod(method: "POST"), requestUri: $"https://{host}:{port}/ucommerce/api/v1/search");

            var response = client.Send(request);

            return response.IsSuccessStatusCode ? "ScratchIndexRequest complete" : "ScratchIndexRequest did NOT complete";
        }
    }
}