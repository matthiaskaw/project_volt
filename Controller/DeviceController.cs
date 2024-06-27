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
   
    public void InitializeDevices(){

        
        try{
            Devices.Clear();
        }
        catch(Exception e){

            Logger.WriteToLog($"Particle Counter: Excepton thrown after Device.Clear() {e} ");
        }
        
        ParticleCounter particleCounter;
        PowerSource powerSource;
        ElectrostaticClassifier electrostaticClassifier;
        ElectrospraySensor electrospraySensor;
        
        switch(MeasurementController.Instance.MeasurementType){
            
            case EMeasurementType.Default:
                //Throw exception "No Measurement Type"
                Logger.WriteToLog("Device Controller: No Measurement Type set!");
                break;

            case EMeasurementType.SMPS:

                Logger.WriteToLog("DeviceController: Initialize SMPS");
                particleCounter = new ParticleCounter();
                Devices.Add(particleCounter.DeviceType, particleCounter);
                break;

            case EMeasurementType.TandemDMA:
                particleCounter = new ParticleCounter();
                electrostaticClassifier = new ElectrostaticClassifier();

                Logger.WriteToLog("DeviceController: Initialize TandemPyrolysis");
                Devices.Add(particleCounter.DeviceType, particleCounter);
                Devices.Add(electrostaticClassifier.DeviceType, electrostaticClassifier);
                
                break;
                

            case EMeasurementType.TandemTemperature:
                
                Logger.WriteToLog("DeviceController: Initialize Temperature");
                particleCounter = new ParticleCounter();
                powerSource = new PowerSource();
                Devices.Add(particleCounter.DeviceType, particleCounter);
                Devices.Add(powerSource.DeviceType, powerSource);

                break;

            case EMeasurementType.CurrentMeasurement:
                Logger.WriteToLog("DeviceController.cs: Initialize current sensors");
                electrospraySensor = new ElectrospraySensor();
                Devices.Add(electrospraySensor.DeviceType, electrospraySensor);
                break;



        }


        foreach(KeyValuePair<EDeviceTypes, IDevice> entry in Devices){
        
            entry.Value.Initialize();
            
        }

    
    }

    public void AreDevicesReady(EMeasurementType measurementType){


        
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