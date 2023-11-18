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
    private List<IDevice> _toBeIntitializedDevices = new List<IDevice>();
    private List<IDevice> _initializedDevices = new List<IDevice>();
    

    private event EventHandler StartDevices;
    private event EventHandler StopDevices;
private void SetupEvents(){

        
        StopDevices += (sender, args) => {

            foreach(IDevice dev in _initializedDevices){

                dev.Stop();
            }
        };
    }
   

    public void InitializeDevices(){

        _initializedDevices.Clear();
        _toBeIntitializedDevices.Clear();

        switch(MeasurementType){

            case EMeasurementType.Default:
                //Throw exception "No Measurement Type"
                Logger.WriteToLog("Device Controller: No Measurement Type set!");
                break;

            case EMeasurementType.SMPS:

                Logger.WriteToLog("DeviceController: Initialize SMPS");
                var particlecounter = new ParticleCounter(100,100,10.5f,1000);
                _toBeIntitializedDevices.Add(particlecounter);
                break;

            case EMeasurementType.TandemPyrolysis:
            
                Logger.WriteToLog("DeviceController: Initialize TandemPyrolysis");
                break;
                

            case EMeasurementType.Temperature:
            
                Logger.WriteToLog("DeviceController: Initialize Temperature");
                break;

        }

        foreach(IDevice dev in _toBeIntitializedDevices){

            dev.Initialize();
            dev.Initialized += (sender,args) => {

                _initializedDevices.Add((IDevice)sender);

                if(_toBeIntitializedDevices.Count == _initializedDevices.Count){
                    
                    
                }
            };
        }
    }
}