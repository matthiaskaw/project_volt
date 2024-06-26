//wwwroot/js/sensor.js

let socket;

function connectWebSocket() {
    socket = new WebSocket("ws://localhost:5064/ws");

    socket.onopen = function(event) {
        console.log("WebSocket is open now.");
    };

    socket.onmessage = function(event) {
        console.log("Message from server: ", event.data);
    };

    socket.onclose = function(event) {
        console.log("WebSocket is closed now.");
    };

    socket.onerror = function(error) {
        console.log("WebSocket error: ", error);
    };
}

function sendMessage() {
    
    console.log("Hello");
    if (!socket || socket.readyState !== WebSocket.OPEN) {
        connectWebSocket();

    }
    const message = { type: 'message', data: 'Hello, Server!' };
    socket.send(JSON.stringify(message));
}
