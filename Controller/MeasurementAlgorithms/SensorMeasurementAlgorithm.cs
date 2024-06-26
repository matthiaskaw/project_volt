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

            await Task.Run(() => {
                while(IsRunning){

                
                    string sensorvalues;
                    sensorvalues = $"{new Random().NextDouble()*5.0};{new Random().NextDouble()}";
                    //Logger.WriteToLog($"SensorMeasurementAlgorithm.cs: Values received:\r{sensorvalues}");
                    Thread.Sleep(500);
            
                }
            });
            


            return true;

        
        }

        public bool IsRunning {get; set;}
    }



        



}