using System.Net.WebSockets;
using Device;


namespace MeasurementAlgorithms{

    public class SensorMeasurement : IMeasurementAlgorithm{

        
      
        public SensorMeasurement(){
            IsRunning = false;

        }
        public async Task<bool> RunMeasurement(){

            IsRunning = true;
            IDevice sens;
            DeviceController.Instance.Devices.TryGetValue(EDeviceTypes.ElectrospraySensor, out sens);
            ElectrospraySensor sensor = (ElectrospraySensor)sens;
            


            if(sensor == null){

                Logger.WriteToLog($"SensorMeasurementAlgorithm.cs: sensor object is null. Cannot continue");
            }

            await Task.Run(async () => {
                while(IsRunning){
                    string sensorvalues;

                
                    
                    sensorvalues = $"{new Random().NextDouble()*5.0};{new Random().NextDouble()}";
                    Thread.Sleep(500);        
                    await WebSocketController.Instance.SendMessageToAllAsync(sensorvalues);     

            
                }

            
            
            }
            );
            
            return true;

        
        }

        public bool IsRunning {get; set;}
    }
    
    
    }



        



