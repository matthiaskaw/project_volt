using System.IO.Ports;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Threading;
using System.Reflection.Metadata.Ecma335;


using Device;
using Measurement;

public class ParticleCounter : IDevice, IMeasurement
{
    public ParticleCounter(){

        _serialport.DataReceived += HandleReceivedData;
    }
    public void Initialize(){
        
        _serialport.ReadTimeout = 1000;
       

        foreach(string portname in SerialPort.GetPortNames()){

            _serialport.PortName = portname;
            _serialport.Open();

            if(!_serialport.IsOpen){

                continue;
            }
            else{

                //if open, send request for serial number
                //until answer is received and ready to read 
                
                VerifyDevice(_verificationstring);
                
                
            }

            
        }
   
        
    }

   

    

    public void SendMessage(string message){}
    public string ReceiveMessage(){return "";}
    public void VerifyMessage(string message){}
    public void VerifyDevice(string verificationstring){


        Timer waittimer = new Timer((object? state) => {
            
            _answer = _serialport.ReadLine();
            if(_answer == verificationstring){

                Initialized?.Invoke(this, new EventArgs());
                Logger.WriteToLog($"VerifyDevice on ParticleCounter: Serialnumber {_verificationstring} ");
            }
            else{
                
            }
            }, null, 0, 1000);

    }
    public void StartMeasurement(){}
    public void EndMeasurement(){}
    public void CancelMeasurement(){}

    
    public event EventHandler Initialized;
    public event EventHandler Canceled;
    public event EventHandler Ready;
    public event EventHandler<string> AnswerReady;
    public event EventHandler StartedMeasurement;
    public event EventHandler EndedMeasurement;
    public event EventHandler CanceledMeasurement;

    string SerialNumber {get; set;}
    private SerialPort _serialport = new SerialPort();

    public void HandleReceivedData(object sender, EventArgs e){
    }

    private  string _answer;
    private string _answerbuffer;
    private string _verificationstring = "123456789"; //serial number

}