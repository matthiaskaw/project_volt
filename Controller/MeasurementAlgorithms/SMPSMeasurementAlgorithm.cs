using Device;
using Measurement;
using Services;
using System.ComponentModel;
using System.IO;

namespace MeasurementAlgorithms{

public class SMPSMeasurementAlgorithm : IMeasurementAlgorithm {


    public SMPSMeasurementAlgorithm(){}

    public async Task<bool> RunMeasurement(){

        Logger.WriteToLog("SMPS Measurement Algorithm: RunMeasurement called!");
        IDevice pc;
        DeviceController.Instance.Devices.TryGetValue(EDeviceTypes.ParticleCounter, out pc);
        ParticleCounter particlecounter = (ParticleCounter)pc;
        
        if(particlecounter == null){

            return false;
        }

        string minVoltage = ParticleCounter.CalculateVoltage("2,4");
        string maxVoltage = ParticleCounter.CalculateVoltage("40,5");

        particlecounter.UpscanTime = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.UpscanTime);
        particlecounter.DownscanTime = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.DownscanTime);
        particlecounter.MinVoltage = minVoltage;
        particlecounter.MaxVoltage = maxVoltage;

        particlecounter.SetScanMode();
        particlecounter.SetScanDirection();
        particlecounter.SetVoltage();
        particlecounter.SetScanTime();
        particlecounter.Start();

        List<string> data = await _collectDataAsync(particlecounter);

        string s = System.Reflection.Assembly.GetEntryAssembly().Location;

        string cwd = System.IO.Path.GetDirectoryName(s);


        File.WriteAllLines(System.IO.Path.Combine(cwd, "testmeasurement.txt"),data);
        

        return true;
    }

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

    public bool IsRunning {get; set;}
    
}
}