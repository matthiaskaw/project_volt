
namespace Device{


public enum EDeviceValues{

    SMPSMinDiameter = 0,
    SMPSMaxDiameter = 1
}

public enum EDeviceTypes{

    ParticleCounter = 0,
    Classifier = 1,
    PowerSource = 2,
}
public interface IDevice{


    void Initialize(); //Connect to device and verify by ID attribute (e.g.: serial number)
    void SendMessage(string message);
    string ReceiveMessage();
    void VerifyDevice(string verificationstring);
    void UpdateSettings();
    void SetValues(Dictionary<EDeviceValues, object> values);


    void Start();
    void Stop();


    event EventHandler Initialized;
    event EventHandler Started;
    event EventHandler Stopped;
    event EventHandler<string> AnswerReady;

    
    public string DeviceID {get; set;}
    public bool IsInitialized{get;}
}
}



