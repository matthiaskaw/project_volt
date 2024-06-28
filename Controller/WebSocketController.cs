using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class WebSocketController{

    
    private static WebSocketController _instance;

    private WebSocketController(){
    }

    public static WebSocketController Instance{

        get{

            if(_instance == null){
                _instance = new WebSocketController();
            }
            return _instance;
        }
    }
    private readonly ConcurrentDictionary<WebSocket, WebSocket> _webSocketConnections = new ConcurrentDictionary<WebSocket, WebSocket>();

    public async Task HandleWebSocketAsync(WebSocket webSocket)
    { 
        
        
        _webSocketConnections.TryAdd(webSocket, webSocket);


        var buffer = new byte[1024 * 4];
        
        WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!result.CloseStatus.HasValue)
        {

            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        _webSocketConnections.TryRemove(webSocket, out _);
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        

    }

    public async Task SendMessageToAllAsync(string message)
    {
        
        var messageBytes = Encoding.UTF8.GetBytes(message);
        int i = 0;
        foreach (var webSocket in _webSocketConnections.Keys)
        {   i++;
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
