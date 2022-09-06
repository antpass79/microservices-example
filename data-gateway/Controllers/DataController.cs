using Microsoft.AspNetCore.Mvc;
using Dapr.Client;

namespace data_gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;

        public DataController(ILogger<DataController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Task<Guid> Get()
        {
            var guid = Guid.NewGuid();

            Console.WriteLine(guid);
            return Task.FromResult(guid);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] int data)
        {
            Console.WriteLine($"data-gateway - data received = {data}");

            try
            {
                var daprClient = new DaprClientBuilder().Build();
                await daprClient.PublishEventAsync("data-pubsub", "counters", data);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.InnerException?.Message);

                return await Task.FromResult(BadRequest());
            }

            return await Task.FromResult(Ok());
        }
    }
}