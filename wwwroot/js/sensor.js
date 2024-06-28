//wwwroot/js/sensor.js

let socket;

function connectWebSocket() {
    socket = new WebSocket("ws://localhost:5064/ws");

    socket.onopen = function(event) {
        console.log("WebSocket is open now.");
        updateUI(event.data);
    };

    socket.onmessage = function(event) {
        console.log("Message from server: ", event.data);
        updateUI(event.data);
    };

    socket.onclose = function(event) {
        console.log("WebSocket is closed now.");
    };

    socket.onerror = function(error) {
        console.log("WebSocket error: ", error);
    };
}

function updateUI(newData) {
    const sensor1value = document.getElementById("sensor1value");
    const sensor2value = document.getElementById("sensor2value");

    const stringArray = newData.split(";");
    console.log(stringArray);
    sensor1value.value=stringArray[0];
    sensor2value.value=stringArray[1];

}




window.onload = function() {
    console.log("WebSocket load...")
    connectWebSocket();
    
};



