using System.Net.WebSockets;
using Device;
using DatabaseModel;


namespace MeasurementAlgorithms{

    public class SensorMeasurementAlgorithm : IMeasurementAlgorithm{

        
      
        public SensorMeasurementAlgorithm(){
            IsRunning = false;

        }
        public async Task<List<string>> RunMeasurement(){

            IsRunning = true;
            List<string> data = new List<string>();

            await Task.Run(async () => {
                
                while(IsRunning){
                    
                    data.Add(SensorController.Instance.SensorData);
                    Thread.Sleep(500);

                }
            
            });
            Logger.WriteToLog($"SensorMeasurementAlgorithm.RunMeasurement: data = {data.ToString()}"); 
            return data;

        }
        public bool IsRunning {get; set;}
    }
    
    
}



        



