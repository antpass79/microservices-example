using Dapr.Client;

string storeName = Environment.GetEnvironmentVariable("store_name")!;
string storeKey = Environment.GetEnvironmentVariable("store_key")!;

CancellationTokenSource source = new CancellationTokenSource();
CancellationToken cancellationToken = source.Token;

var daprClient = new DaprClientBuilder().Build();
var data = await daprClient.GetStateAsync<int>(storeName, storeKey);

while (true)
{
    Console.WriteLine($"data-client-sender - data sent = {data}");

    var request = daprClient.CreateInvokeMethodRequest(HttpMethod.Post, "data-gateway", "data", data);
    try
    {
        await daprClient.InvokeMethodAsync(request, cancellationToken);
    }
    catch (Exception exception)
    {
        Console.WriteLine(exception.Message);
    }

    await daprClient.SaveStateAsync(storeName, storeKey, data);
    await Task.Delay(1000);

    data++;
}