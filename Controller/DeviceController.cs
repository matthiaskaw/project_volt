using System.Collections.Generic;
using System.IO.Ports;
using Device;
using MeasurementAlgorithms;
using Measurement;
using Microsoft.AspNetCore.Mvc;


public class DeviceController{

    private static DeviceController _instance;

    private DeviceController(){
    }

    public static DeviceController Instance{

        get{

            if(_instance == null){
                _instance = new DeviceController();
            }
            return _instance;
        }
    }

    public Dictionary<EDeviceTypes, IDevice> Devices {get;} = new Dictionary<EDeviceTypes, IDevice>(); 
    public void Cancel(){

        Logger.WriteToLog("DeviceController.Cancel(): Trying to End all devices...");
        foreach(IDevice device in Devices.Values){
            device.End();
        }
    }
    public void InitializeDevices(){

        
        try{
            Devices.Clear();
        }
        catch(Exception e){

            Logger.WriteToLog($"DeviceController.InitializeDevices(): Excepton thrown after Device.Clear() {e} ");
        }

        Devices.Add(EDeviceTypes.ParticleCounter, new ParticleCounter());
        //Devices.Add(EDeviceTypes.Classifier, new ElectrostaticClassifier());
        Devices.Add(EDeviceTypes.PowerSource, new PowerSource());
        //Devices.Add(EDeviceTypes.ElectrospraySensor, new ElectrospraySensor());
        
        foreach(KeyValuePair<EDeviceTypes, IDevice> entry in Devices){

            try{
                entry.Value.Initialize();
            }
            catch(Exception e){

                Logger.WriteToLog($"DeviceController.InitialzeDevices: Exception thrown. Removing {entry.Key} from list.");
                Devices.Remove(entry.Key);
            }
        }
    }

    public void CheckNecessaryDevices(){
        
        //Check if all devices that are necessary for a measurement are ready?
        Logger.WriteToLog($"DeviceController.CheckNecessaryDevices: Checking devices...");
        switch(MeasurementController.Instance.MeasurementType){

            case EMeasurementType.Default:
                Logger.WriteToLog("DeviceController.CheckNecessaryDevices: No Measurement Type set!");
                throw new Exception();
                break;

            case EMeasurementType.SMPS:

                Logger.WriteToLog("DeviceController.CheckNecessaryDevices: SMPS Measurement Type set!");
                if(!Devices.ContainsKey(EDeviceTypes.ParticleCounter)){
                    Logger.WriteToLog("DeviceController.CheckNecessaryDevices: Particle Counter not initialized!");
                    throw new Exception();}
                break;

            case EMeasurementType.TandemDMA:
                if(!Devices.ContainsKey(EDeviceTypes.ParticleCounter)){
                    Logger.WriteToLog("DeviceController.CheckNecessaryDevices: Particle Counter not initialized!");
                    throw new Exception();}
                if(!Devices.ContainsKey(EDeviceTypes.Classifier)){
                    Logger.WriteToLog("DeviceController.CheckNecessaryDevices: Classifier not initialized!");
                    throw new Exception();}                
                break;

            case EMeasurementType.TandemTemperature:
                if(!Devices.ContainsKey(EDeviceTypes.ParticleCounter)){
                    Logger.WriteToLog("DeviceController.CheckNecessaryDevices: Particle Counter not initialized!");
                    throw new Exception();}
                if(!Devices.ContainsKey(EDeviceTypes.PowerSource)){
                    Logger.WriteToLog("DeviceController.CheckNecessaryDevices: Power Source not initialized!");
                    throw new Exception();}
                break;

            case EMeasurementType.CurrentMeasurement:
                if(!Devices.ContainsKey(EDeviceTypes.ElectrospraySensor)){
                    Logger.WriteToLog("DeviceController.CheckNecessaryDevices: ElectrospraySensor not initialized!");
                    throw new Exception();}
                break;
        }       
    }

}