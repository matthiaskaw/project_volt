using Device;
using Measurement;
using Services;
using System.ComponentModel;
using System.IO;

namespace MeasurementAlgorithms{

public class SMPSMeasurementAlgorithm : IMeasurementAlgorithm {


    public SMPSMeasurementAlgorithm(){}

    public async Task<List<string>> RunMeasurement(){
        IsRunning = true;
        Logger.WriteToLog("SMPS Measurement Algorithm: RunMeasurement called!");
        IDevice pc;
        DeviceController.Instance.Devices.TryGetValue(EDeviceTypes.ParticleCounter, out pc);
        ParticleCounter particlecounter = (ParticleCounter)pc;
        
        if(particlecounter == null){

            throw new Exception("SMPSMeasurementAlgorithm().RunMeasurement: Particle Counter is null");
        }
        string minVoltage = ParticleCounter.CalculateVoltage(
            SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSMinDiameter));
        string maxVoltage = ParticleCounter.CalculateVoltage(
            SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSMaxDiameter));

        particlecounter.UpscanTime = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.UpscanTime)+"0";
        particlecounter.DownscanTime = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.DownscanTime)+"0";
        particlecounter.MinVoltage = minVoltage;
        particlecounter.MaxVoltage = maxVoltage;

        particlecounter.SetScanMode();
        particlecounter.SetScanDirection();
        particlecounter.SetVoltage();
        particlecounter.SetScanTime();
        particlecounter.Start();

        List<string> data = await _collectDataAsync(particlecounter);
    

        return data;
    }

    private async Task<List<string>> _collectDataAsync(ParticleCounter counter){

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

    private async Task<List<string>> _collectDataAsyncDummy(){

        List<string> data = new List<string>();
        int count = 0;
        while(IsRunning){
            
            var  line = $"{ new Random().Next(0, 100)}";
            Logger.WriteToLog($"Particle Counter: CollectData: line = {line}");
            data.Add(line);


            if(count >= 10){

                IsRunning  = false;

            }
            count++;;
        }
        

        return data;
        
    }

    public bool IsRunning {get; set;}
    
}
}