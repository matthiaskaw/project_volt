


namespace Device{

public class ElectrostaticClassifier : IDevice{

    public ElectrostaticClassifier(){}



    public void Initialize(){}
    public void SendMessage(string message){}
    public string ReceiveMessage(){return "";}
    public void VerifyDevice(string verificationstring){}
    public void Start(){}
    public void Stop(){}
    public void UpdateSettings(){}
    public event EventHandler Initialized;
    public event EventHandler Started;
    public event EventHandler Stopped;
    public event EventHandler<string> AnswerReady;


    public float Diameter {get; set;}
    


}


}