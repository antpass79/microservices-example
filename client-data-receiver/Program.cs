using client_data_receiver;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5081/DataHub")
    .WithAutomaticReconnect()
    .Build();

connection.On<DataPackage>("DataReceived", dataPackage =>
{
    Console.WriteLine($"client-data-receiver - data received = {dataPackage.Data} at {dataPackage.PackedAt}");
});

await connection.StartAsync();

var host = CreateHostBuilder(args).Build();
await host.RunAsync();

static IHostBuilder CreateHostBuilder(string[] args) =>
        Host
            .CreateDefaultBuilder(args);