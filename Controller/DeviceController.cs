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
    public Dictionary<EDeviceTypes, IDevice> Sensors {get;} = new Dictionary<EDeviceTypes, IDevice>();    

    protected void OnInitialized(){
        Initialized.Invoke(this, new EventArgs());
    }
    private event EventHandler StartDevices;
    private event EventHandler StopDevices;

    public event EventHandler Initialized;

    //PRIVATE VARIABLES
    private void SetupEvents(){
  
    }
    
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


        //Devices.Add(EDeviceTypes.ParticleCounter, new ParticleCounter());
        //Devices.Add(EDeviceTypes.Classifier, new ElectrostaticClassifier());
        //Devices.Add(EDeviceTypes.PowerSource, new PowerSource());
        Devices.Add(EDeviceTypes.ElectrospraySensor, new ElectrospraySensor());
         
        
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

        
        switch(MeasurementController.Instance.MeasurementType){
            
            case EMeasurementType.Default:
                throw new Exception();
                Logger.WriteToLog("Device Controller: No Measurement Type set!");
                break;

            case EMeasurementType.SMPS:
                if(!Devices.ContainsKey(EDeviceTypes.ParticleCounter)){throw new Exception();}
            break;

            case EMeasurementType.TandemDMA:
                if(!Devices.ContainsKey(EDeviceTypes.ParticleCounter)){throw new Exception();}
                if(!Devices.ContainsKey(EDeviceTypes.Classifier)){throw new Exception();}                
                break;
                

            case EMeasurementType.TandemTemperature:
                if(!Devices.ContainsKey(EDeviceTypes.ParticleCounter)){throw new Exception();}
                break;

            case EMeasurementType.CurrentMeasurement:
                if(!Devices.ContainsKey(EDeviceTypes.ElectrospraySensor)){throw new Exception();}
                break;
        }       
    }

    public void InitializeSensors(){
        
        Sensors.Clear();
        ElectrospraySensor electrospraySensor = new ElectrospraySensor();
        Sensors.Add(electrospraySensor.DeviceType, electrospraySensor);

        
        foreach(KeyValuePair<EDeviceTypes, IDevice> entry in Sensors){
            entry.Value.Initialize();
        }
        
    }


}