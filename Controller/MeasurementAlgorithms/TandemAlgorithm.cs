using System.Globalization;
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
        
        IsRunning = true;
        IDevice ps;
        IDevice pc;

        SMPSMeasurementAlgorithm smpsAlgorithm = new SMPSMeasurementAlgorithm();
        
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

       powerSource.Start();
        

        List<string> masterList = new List<string>();

        foreach(string i in CurrentVector){
            if(!IsRunning){ break;}
            
            double current = 0;
            double.TryParse(i, System.Globalization.CultureInfo.InvariantCulture, out current);
            powerSource.SetCurrent(current);
            
            Thread.Sleep(10000);

            List<string> measurement = await smpsAlgorithm.RunMeasurement();
            masterList.Add(string.Join(",", measurement));
            
        }

        powerSource.End();

        return masterList;
    }

    public List<string> CurrentVector {get; set;}


    //PRIVATE METHODS

    
    void _setCurrentVector(){

        
        Logger.WriteToLog("TandemAlgorithm._setCurrentVector: Try to parse...");

        string minCurrent = SettingsService.
                                Instance.
                                MeasurementSetting.
                                GetSettingByKey(EMeasurementSettings.TandemTemperatureMinCurrent);
        string maxCurrent = SettingsService.
                                Instance.
                                MeasurementSetting.
                                GetSettingByKey(EMeasurementSettings.TandemTemperatureMaxCurrent);

        string currentStep = SettingsService.
                                Instance.
                                MeasurementSetting.
                                GetSettingByKey(EMeasurementSettings.TandemTemperatureStep);


        double currentMin = 0.0;
        if(!double.TryParse(minCurrent,System.Globalization.CultureInfo.InvariantCulture,out currentMin)){
        
            throw new Exception($"TandemAlgorithm._setCurrentVector: Parsing minCurrent ({minCurrent}) failed");
        }

        double currentMax = 0.0;

        if(!double.TryParse(maxCurrent,System.Globalization.CultureInfo.InvariantCulture, out currentMax)){

            throw new Exception($"TandemAlgorithm._setCurrentVector: Parsing maxCurrent ({maxCurrent}) failed");
        }

        double currentStepDouble = 0.0;

        if(!double.TryParse(currentStep,System.Globalization.CultureInfo.InvariantCulture, out currentStepDouble)){

            throw new Exception($"TandemAlgorithm._setCurrentVector: Parsing currentStepDouble ({currentStep}) failed");
        }

        Logger.WriteToLog("TandemAlgorithm._setCurrentVector: Parsed min Current, max Current, and Current step. Calculating current vector");

        List<double> currents = new List<double>();
        double currentCurrent = currentMin;
        while(currentCurrent < currentMax){

            currents.Add(currentCurrent);
            
            currentCurrent = currentCurrent + currentStepDouble;
            
        }
        currents.Add(currentCurrent);
        CurrentVector = new List<string>();
        foreach(double d in currents){
            CurrentVector.Add(d.ToString("F", System.Globalization.CultureInfo.InvariantCulture));
            Logger.WriteToLog($"TandemAlgorithm._setCurrentVector: Current element: {d.ToString("F", System.Globalization.CultureInfo.InvariantCulture)}");
        }


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