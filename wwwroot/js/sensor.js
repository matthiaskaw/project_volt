//wwwroot/js/sensor.js

const currentSensorOne = "CurrentSensorOne";
const currentSensorTwo = "CurrentSensorTwo";


document.addEventListener("DOMContentLoaded", function(){
    let currentSensorOneElement = document.getElementById(currentSensorOne);
    let currenSensorTwoElement = document.getElementById(currentSensorTwo);
    var socket = new WebSocket("wss://localhost:5096/ws");
    
    socket.onmessage = function(event) {
        JSON.parse(event.data);
        currentSensorOneElement.innerText = "Sensor Value: " + data.sensor_value;


    };

    socket.onclose = function(event) {
        currentSensorOneElement.innerText = "Connection closed";
    };
});