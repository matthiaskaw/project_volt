
namespace Device{

public interface IDevice{


    void Initialize(); //Connect to device and verify by ID attribute (e.g.: serial number)
    void SendMessage(string message);
    string ReceiveMessage();
    void VerifyDevice(string verificationstring);
    void UpdateSettings();
    void Start();
    void Stop();


    event EventHandler Initialized;
    event EventHandler Started;
    event EventHandler Stopped;
    event EventHandler<string> AnswerReady;

    
}
}



