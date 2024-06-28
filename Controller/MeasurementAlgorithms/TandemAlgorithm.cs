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
    public async Task<bool> RunMeasurement(){
            
        IDevice ps;
        IDevice pc;

        if(!DeviceController.Instance.Devices.TryGetValue(EDeviceTypes.PowerSource, out ps)){

            Logger.WriteToLog($"TandemTemperatureAlgorithm: RunMeasurement: Powersource not found in Devices");
            return false;
        };

        if(!DeviceController.Instance.Devices.TryGetValue(EDeviceTypes.ParticleCounter, out pc)){

            Logger.WriteToLog($"TandemTemperatureAlgorithm: RunMeasurement: Power Source not fount in Devices");
            return false;
        }

        PowerSource powerSource = (PowerSource)ps;
        ParticleCounter particleCounter = (ParticleCounter)pc;

        if(powerSource == null){

            Logger.WriteToLog($"TandemTemperatureAlgorithm: RunMeasurement: powerSource is null. Could not cast IDevice to PowerSource");
            return false;
        }

        if(particleCounter == null){

            Logger.WriteToLog($"TandemTemperatureAlgorithm: RunMeasurement: particleCounter is null. Could not cast IDevice to ParticleCounter");

        }

        
        particleCounter.UpscanTime = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.UpscanTime);
        particleCounter.DownscanTime = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.DownscanTime);
        particleCounter.MinVoltage = ParticleCounter.CalculateVoltage(
            SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSMinDiameter));
        particleCounter.MaxVoltage = ParticleCounter.CalculateVoltage(
            SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSMaxDiameter));;

        particleCounter.SetScanMode();
        particleCounter.SetScanDirection();
        particleCounter.SetVoltage();
        particleCounter.SetScanTime();
 

    
    

        for(int i = 0; i < CurrentVector.Length; i++){
            
            double current = 0;

            double.TryParse(CurrentVector[i], out current);

            powerSource.SetCurrent(current);
            particleCounter.Start();

            await _collectDataAsync(particleCounter);
        }

        return true;
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