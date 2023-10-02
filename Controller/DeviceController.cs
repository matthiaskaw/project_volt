using System.Collections.Generic;
using System.IO.Ports;
using Device;
using Measurement;

public class DeviceController{

    private static DeviceController _instance;

    private DeviceController(){}

    public static DeviceController Instance{

        get{

            if(_instance == null){
                _instance = new DeviceController();
                Logger.WriteToLog("DeviceController instantiated!");
            }
            Logger.WriteToLog("DeviceController returned");
            return _instance;
        }
    }

    public EMeasurementType MeasurementType {get; set;}

    private List<IDevice> _devices;

    void InitializeDevices(EMeasurementType type){


        switch(type){

            case EMeasurementType.SMPS:
                Logger.WriteToLog("DeviceController: Initialize SMPS");
                _devices[0] = new ParticleCounter();
                
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