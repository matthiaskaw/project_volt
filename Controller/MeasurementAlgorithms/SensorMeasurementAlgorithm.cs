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
                    
                    data.Append(SensorController.Instance.SensorData);
                
                }
            
            });

            return data;

        }
        public bool IsRunning {get; set;}
    }
    
    
}



        



