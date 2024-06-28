using Device;
using Measurement;
using Microsoft.AspNetCore.Razor.TagHelpers;

public class SensorController{

    private static SensorController _instance;
    private SensorController(){

        _sensor = (ISensor)DeviceController.Instance.Devices.GetValueOrDefault(EDeviceTypes.ElectrospraySensor);
        
        if(_sensor == null){ throw new Exception("SensorController.SensorController(): _sensor = null"); }

        _setupEvents();

        


    }

    public static SensorController Instance{

        get{

            if(_instance == null){
                _instance = new SensorController();
            }
            return _instance;
        }
    }

    public void StartSensors(){

        _getSensorData();
    }    
    public event EventHandler<string> DataAvailable;

    private async void _getSensorData(){

        await Task.Run(() => {

            while(true){
                
                _data = _sensor.RequestSensorValues();

                
                _onDataAvailable(_data);
                Thread.Sleep(1000);

            }

        });

    }
    private void _onDataAvailable(string data){

            DataAvailable?.Invoke(this, data);

    }

    private void _setupEvents() {

        DataAvailable += async (sender, e) => { 
            
            await WebSocketController.Instance.SendMessageToAllAsync(e);
            
        };
    }
    
    private ISensor _sensor;
    private string _data;
    public string SensorData{get;}

    

}