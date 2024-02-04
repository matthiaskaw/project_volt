using System.Collections.Generic;
using System.IO.Ports;
using Device;
using MeasurementAlgorithms;
using Measurement;
using Microsoft.AspNetCore.Mvc;


public class DeviceController{

    private static DeviceController _instance;

    private DeviceController(){
        MeasurementType = EMeasurementType.Default;
    }

    public static DeviceController Instance{

        get{

            if(_instance == null){
                _instance = new DeviceController();
            }
            return _instance;
        }
    }

    public EMeasurementType MeasurementType { get; set;} 
    public Dictionary<EDeviceTypes, IDevice> Devices {get;} = new Dictionary<EDeviceTypes, IDevice>();

    private IMeasurementAlgorithm _measurementAlgorithm;
    

    private event EventHandler StartDevices;
    private event EventHandler StopDevices;


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
        switch(MeasurementType){
            
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

        }

        foreach(KeyValuePair<EDeviceTypes, IDevice> entry in Devices){

            //entry.Value.Initialize();
            


        }

        MeasurementController.Instance.StartMeasurement(MeasurementType);
    }
}