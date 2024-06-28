using System.Net.WebSockets;
using Device;


namespace MeasurementAlgorithms{

    public class SensorMeasurementAlgorithm : IMeasurementAlgorithm{

        
      
        public SensorMeasurementAlgorithm(){
            IsRunning = false;

        }
        public async Task<bool> RunMeasurement(){

            IsRunning = true;
            
            await Task.Run(async () => {
                while(IsRunning){
                    

                    string data = SensorController.Instance.SensorData;
            
                }

            
            
            }
            );
            
            return true;

        
        }

        
        public bool IsRunning {get; set;}
    }
    
    
    }



        



