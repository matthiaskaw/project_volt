using System.Collections.Generic;
using System.IO.Ports;
using Device;
using MeasurementAlgorithms;
using Measurement;

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
    private IMeasurementAlgorithm _measurementAlgorithm;
    private Dictionary<EDeviceTypes, IDevice> _devices = new Dictionary<EDeviceTypes, IDevice>();
    private event EventHandler StartDevices;
    private event EventHandler StopDevices;

    private void SetupEvents(){

        
        StopDevices += (sender, args) => {

            foreach(var dev in _devices){

                dev.Value.Stop();
            }
        };
    }
   

    public void InitializeDevices(){

        _devices.Clear();

        switch(MeasurementType){

            case EMeasurementType.Default:
                //Throw exception "No Measurement Type"
                Logger.WriteToLog("Device Controller: No Measurement Type set!");
                break;

            case EMeasurementType.SMPS:

                Logger.WriteToLog("DeviceController: Initialize SMPS");
                var particlecounter = new ParticleCounter(100,100,10.5f,1000);
                _devices.Add(EDeviceTypes.ParticleCounter, particlecounter);
                break;

            case EMeasurementType.TandemPyrolysis:
            
                Logger.WriteToLog("DeviceController: Initialize TandemPyrolysis");
                break;
                

            case EMeasurementType.Temperature:
            
                Logger.WriteToLog("DeviceController: Initialize Temperature");
                break;

        }
        
        bool allinitialized = false;
        foreach(KeyValuePair<EDeviceTypes,IDevice> dev in _devices){

            dev.Value.Initialize();
            dev.Value.Initialized += (sender,args) => {
           
                foreach(KeyValuePair<EDeviceTypes,IDevice> dev in _devices){
                
                    if(!dev.Value.IsInitialized){

                        break;
                    }
                    else{
                        
                        break;
                    }
                }

        };
    }
}
}