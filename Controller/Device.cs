
namespace Device{

public interface IDevice{


    void Initialize(); //Connect to device and verify by ID attribute (e.g.: serial number)
    void SendMessage(string message);
    string ReceiveMessage();
    void VerifyDevice(string verificationstring);
    void VerifyMessage(string message); //Check received message if complete, for example is a delimiter like /n present


    event EventHandler Initialized;
    event EventHandler Ready;

    event EventHandler<string> AnswerReady;

    
}
}



