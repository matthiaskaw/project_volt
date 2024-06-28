
namespace Device{


public enum EDeviceValues{

    SMPSMinDiameter = 0,
    SMPSMaxDiameter = 1
}

public enum EDeviceTypes{

    ParticleCounter = 0,
    Classifier = 1,
    PowerSource = 2,
    ElectrospraySensor = 3
}
public interface IDevice{


    void Initialize(); //Connect to device and verify by ID attribute (e.g.: serial number)
    
    bool End();
  


    event EventHandler Initialized;
    public bool IsInitialized { get; }
    
    public string DeviceID {get; set;}
    public EDeviceTypes DeviceType {get;}
}
}



