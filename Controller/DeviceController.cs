using System.Collections.Generic;
using System.IO.Ports;
using Device;
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

    private List<IDevice> _devices = new List<IDevice>();

    public void InitializeDevices(){



        switch(MeasurementType){

            case EMeasurementType.Default:
                //Throw exception "No Measurement Type"
                Logger.WriteToLog("Device Controller: No Measurement Type set!");
                break;

            case EMeasurementType.SMPS:
                Logger.WriteToLog("DeviceController: Initialize SMPS");
                
                var particlecounter = new ParticleCounter(100,100,10.5f,1000);
                
                _devices.Add(particlecounter);
                
                break;

            case EMeasurementType.TandemPyrolysis:
                Logger.WriteToLog("DeviceController: Initialize TandemPyrolysis");
                break;

            case EMeasurementType.Temperature:
                Logger.WriteToLog("DeviceController: Initialize Temperature");
                break;

        }

        foreach(IDevice dev in _devices){

            dev.Initialize();
        }
    }
   
}