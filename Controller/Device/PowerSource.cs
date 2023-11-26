using Test;


namespace Device{


    public class PowerSource : IDevice{

        public PowerSource(){}

        public void Initialize(){

            throw new InitalizationFailedException("PowerSource");

        }
    

        public void SendMessage(string message){}
        public string ReceiveMessage(){return "";}


        public void Start(){}
        public void Stop(){}
        public void UpdateSettings(){}
        public void SetValues(Dictionary<EDeviceValues, object> values){}
        public event EventHandler Initialized;
        public event EventHandler Started;
        public event EventHandler Stopped;
        public event EventHandler Ready;
        public event EventHandler<string> AnswerReady;
        public event EventHandler StartedMeasurement;
        public event EventHandler EndedMeasurement;
        public event EventHandler CanceledMeasurement;


        //PRIVATE METHODS
   
  
   
    


        //properties
   
        public int UpscanTime{get; set;}
        public int DownscanTime{get; set;}
        public float MinDiameter{get; set;}
        public float MaxDiameter{get; set;}
        public bool UpscanDirection{get; set;}
        string SerialNumber {get; set;}
        public string DeviceID {get; set;}
        public bool IsInitialized {get{return _isInitialized;}}



        private SerialPortTest _serialport = new SerialPortTest();
        private  string _answer;
        private string _answerbuffer;
        private string _positiveAnswer = "OK\n";
        private string _negativeAnswer = "ERROR\n";
        private string _verificationstring = "123456789"; //serial number
        private bool _isInitialized = false;

       }
}