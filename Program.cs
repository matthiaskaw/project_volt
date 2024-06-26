using Measurement;
using Services;
using System.Net.WebSockets;
using System.Text;

SettingsService settings = SettingsService.Instance;
MeasurementController measurementController = MeasurementController.Instance;
DeviceController deviceController = DeviceController.Instance;


deviceController.Initialized += async (sender, data) => {

    
    await measurementController.StartMeasurement();
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseWebSockets();
app.MapRazorPages();

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
        await HandleWebSocketAsync(webSocket);
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.Run();

static async Task HandleWebSocketAsync(WebSocket webSocket)
{
    var buffer = new byte[1024 * 4];
    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

    while (!result.CloseStatus.HasValue)
    {
        var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
        Console.WriteLine("Received: " + receivedMessage);

        var responseMessage = Encoding.UTF8.GetBytes("Message received");
        await webSocket.SendAsync(new ArraySegment<byte>(responseMessage), result.MessageType, result.EndOfMessage, CancellationToken.None);

        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    }

    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
}