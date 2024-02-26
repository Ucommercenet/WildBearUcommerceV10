using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WildBearAdventures.API.ApiControllers.Miscellaneous
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscoverPipelinesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DiscoverPipelinesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        
        [HttpPost("DiscoverPipelines")]
        public string DiscoverPipelines()
        {
            var host = HttpContext.Request.Host.Host;
            var port = HttpContext.Request.Host.Port;

            using var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(
                new HttpMethod(method: "POST"), requestUri: $"https://{host}:{port}/ucommerce/api/v1/search");

            var response = client.Send(request);

            return response.IsSuccessStatusCode ? "ScratchIndexRequest complete" : "ScratchIndexRequest did NOT complete";
        }



    }
}
