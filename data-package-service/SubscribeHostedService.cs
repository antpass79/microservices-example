using Dapr;
using data_package_service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

public class SubscribeHostedService : ControllerBase
{
    private readonly IHubContext<DataHub> _hubContext;

    public SubscribeHostedService(IHubContext<DataHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [Topic("data-pubsub", "counters")]
    [HttpPost]
    [Route("data-pubsub")]
    public async Task<DataPackage> ProcessData([FromBody] int data)
    {
        var dataPackage = new DataPackage
        {
            Data = data,
            PackedAt = DateTime.Now
        };

        Console.WriteLine($"data-package-service - data subscribed = {dataPackage.Data} at {dataPackage.PackedAt}");

        await _hubContext.Clients.All.SendAsync("DataReceived", dataPackage);

        return await Task.FromResult(dataPackage);
    }
}