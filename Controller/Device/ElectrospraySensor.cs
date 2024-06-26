using System.IO.Ports;
using System.Text;
using System.Net.WebSockets;




namespace Device{


    class ElectrospraySensor : IDevice, ISensor{

        public ElectrospraySensor(){

            DeviceID = "ElectrospraySensor";
            DeviceType = EDeviceTypes.ElectrospraySensor;
            _serialport.Encoding = Encoding.UTF8;
        }


        public void Initialize(){
            /*
            string answer="";
            Logger.WriteToLog($"ElectrospraySensor: Initializeing Electrospray Sensor");

            if(SerialPort.GetPortNames().Length == 0){

                Logger.WriteToLog($"ElectrospraySensor: No device connected to COM ports");
                return;
            }


            foreach(string portname in SerialPort.GetPortNames()){

                if(_serialport.IsOpen){

                    Logger.WriteToLog($"ElectrospraySensor.cs: {portname} is already open. Trying next port...");
                    continue;
                }
             _serialport.PortName = portname;
                
                Logger.WriteToLog($"ElectrospraySensor.cs: Trying to open serial port on port {portname}");
                try{
                    _serialport.Open();
                }
                catch(Exception e){

                    Logger.WriteToLog($"ElectrospraySensor.cs: Tried to open port {portname}. Access denied! Trying next port... {e}");
                    
                    continue;

                }
                
                if(!_serialport.IsOpen){
                    
                    Logger.WriteToLog($"ElectrospraySensor.cs: Serialport not open on port {_serialport.PortName}. Trying next port...");
                }
                Logger.WriteToLog($"ElectrospraySensor.cs: Starting verification. Requesting serial number {DeviceID}");
                _serialport.WriteTimeout = 5000;
                try{
                    _serialport.Write("DeviceID");   
                }
                catch(Exception e){

                    Logger.WriteToLog($"ElectrospraySensor.cs: Writing timeout {e}! Trying next port...");
                    _serialport.Close();
                    continue;
                }
                _serialport.ReadTimeout = 5000;
                try{
                    answer = _serialport.ReadTo("\r");
                    Logger.WriteToLog($"ElectrospraySensor.cs: Initialize(): answer = {answer} ");
                }
                catch(TimeoutException){
                        
                    Logger.WriteToLog($"ElectrospraySensor.cs: Initialize() read timed out answer = {answer}");
                    _serialport.Close();
                    continue;
                }

                    
                if(answer.Contains(DeviceID)){
            
                    _serialport.ReadTimeout = -1;
                    Initialized?.Invoke(this, new EventArgs());
                    Logger.WriteToLog($"ElectrospraySensor.cs: Electrospray Sensor verified on port {_serialport.PortName}");
                    break;
                }

                
                if(portname == SerialPort.GetPortNames().Last()){

                    Logger.WriteToLog($"ElectrospraySensor.cs: Reached last element with no successfull verification");
                    throw new InitalizationFailedException("Particle Counter");
                }   

            }*/
            
            if(IsInitialized == true){return;}
            Initialized?.Invoke(this, new EventArgs());

        }

        public bool Start(){return true;}
        public bool End(){return true;}
        
        public string RequestSensorValues(){
            
            return $"{new Random().NextDouble()};{new Random().NextDouble()}";

            /*string requeststringcommand = "ReadSensors";
            _serialport.WriteTimeout = 5000;
            _serialport.Write(requeststringcommand);

            string requestanswer = _serialport.ReadTo("\r");
            Logger.WriteToLog($"ElectrospraySensor.cs: RequestSensorValue(): Request answer is {requestanswer}");
            return requestanswer;
            */
        }

        

        public string DeviceID {get; set;}
        public event EventHandler Initialized;
        public EDeviceTypes DeviceType {get; set;}
        public bool IsInitialized {get;}
        private SerialPort _serialport = new SerialPort(){BaudRate = 115200, DataBits = 8, Parity = Parity.None, StopBits = StopBits.One};

    }

 
}