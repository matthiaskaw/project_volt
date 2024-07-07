using System.Threading.Tasks.Dataflow;
using Device;
using MeasurementAlgorithms;
using Services;



namespace MeasurementAlgorithms{



/*public class TandemDMAAlgorithm : IMeasurementAlgorithm{


    
}*/

public class TandemTemperatureAlgorithm : IMeasurementAlgorithm {

    public TandemTemperatureAlgorithm(){

        _setCurrentVector();

    }
    public async Task<List<string>> RunMeasurement(){
            
        //IDevice ps;
        //IDevice pc;

        SMPSMeasurementAlgorithm smpsAlgorithm = new SMPSMeasurementAlgorithm();
        /*
        if(!DeviceController.Instance.Devices.TryGetValue(EDeviceTypes.PowerSource, out ps)){

            Logger.WriteToLog($"TandemTemperatureAlgorithm: RunMeasurement: Powersource not found in Devices");
            return new List<string>();
        };

        if(!DeviceController.Instance.Devices.TryGetValue(EDeviceTypes.ParticleCounter, out pc)){

            Logger.WriteToLog($"TandemTemperatureAlgorithm: RunMeasurement: Power Source not fount in Devices");
            return new List<string>();
        }

        PowerSource powerSource = (PowerSource)ps;
     

        if(powerSource == null){

            Logger.WriteToLog($"TandemTemperatureAlgorithm: RunMeasurement: powerSource is null. Could not cast IDevice to PowerSource");
            return new List<string>();
        }

       
        */
    

        List<string> masterList = new List<string>();

        for(int i = 0; i < CurrentVector.Length; i++){
            
            double current = 0;

            double.TryParse(CurrentVector[i], out current);

            List<string> measurement = await smpsAlgorithm.RunMeasurement();
            masterList.Add(string.Join(",", measurement));

        }

        return masterList;
    }

    public string[] CurrentVector {get; set;}


    //PRIVATE METHODS

    public async Task<List<string>> _collectDataAsync(ParticleCounter counter){

        List<string> data = new List<string>();

        while(IsRunning){

            string line = counter.Read();
            Logger.WriteToLog($"Particle Counter: CollectData: line = {line}");
            data.Add(line);
            if(line.Contains("-1")){

                IsRunning  = false;

            }
        }
        counter.End();

        return data;
        
    }

    void _setCurrentVector(){

        string temp = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.TandemTemperatureVector);
        string[] fullCurrentVector = temp.Split(";");
        long positionMinCurrent = Array.IndexOf(fullCurrentVector,
                    SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.TandemTemperatureMinCurrent));
        long positionMaxCurrent = Array.IndexOf(fullCurrentVector,
                    SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.TandemTemperatureMaxCurrent));

        long trimmedLength = positionMaxCurrent - positionMinCurrent + 1;
        
        CurrentVector = new string[trimmedLength];
        Array.Copy(fullCurrentVector, positionMinCurrent, CurrentVector, 0, trimmedLength);

    }
    static async Task DummyAsync()
    {
        Console.WriteLine("Dummy function start");

        // Simulate some asynchronous work
        await Task.Delay(2000); // This represents an asynchronous operation that takes 2 seconds

        Console.WriteLine("Dummy function end");
    }

    public bool IsRunning { get; set; }
}

    
}