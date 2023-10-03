using System.IO.Ports;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Threading;
using System.Reflection.Metadata.Ecma335;


using Measurement;
using Test;
namespace Device{
public class ParticleCounter : IDevice
{
    public ParticleCounter(int upscanTime, int downscanTime, float minDiameter, float maxDiameter){


        UpscanTime = upscanTime;
        DownscanTime = downscanTime;
        MinVoltage = calculateVoltage(minDiameter);
        MaxVoltage = calculateVoltage(maxDiameter);
        UpscanDirection = true;

        _serialport.DataReceived += HandleReceivedData;
    }
    public void Initialize(){
        

        Logger.WriteToLog("Particle Counter: Initializing");
       
        Logger.WriteToLog("Particle Counter: Trying to verify Particle Counter...");
        
        foreach(string portname in SerialPort.GetPortNames()){
            
            
                Logger.WriteToLog($"Particle Counter: Verifying on Port {portname}");
                _serialport.PortName = portname;
                _serialport.Open(true);

                if(!_serialport.IsOpen){
                    Logger.WriteToLog($"Particle Counter: Could not open device on port {portname}. Trying next port");
                    continue;
                }
                else{

                    Logger.WriteToLog($"Particle Counter: Starting verification. Requesting serial number {_verificationstring}");
                    VerifyDevice(_verificationstring);
                }

            
        }
   
        
    }

   
    public int UpscanTime{get; set;}
    public int DownscanTime{get; set;}
    public int MinVoltage{get; set;}
    public int MaxVoltage{get; set;}
    public bool UpscanDirection{get; set;}


    

    public void SendMessage(string message){}
    public string ReceiveMessage(){return "";}
    public void VerifyMessage(string message){}
    public void VerifyDevice(string verificationstring){
            
        _serialport.Write("RSN\n");   
        
      
        _answer = _serialport.ReadLine();
        

        if(_answer == _verificationstring){
            
            Initialized?.Invoke(this, new EventArgs());
            
        }
        else{
            Logger.WriteToLog($"Particle Counter: Answer {_answer} =/= {_verificationstring}. Trying next port...");

        }


    }

    
    public event EventHandler Initialized;

    public event EventHandler Ready;
    public event EventHandler<string> AnswerReady;
    public event EventHandler StartedMeasurement;
    public event EventHandler EndedMeasurement;
    public event EventHandler CanceledMeasurement;

    string SerialNumber {get; set;}
    private SerialPortTest _serialport = new SerialPortTest();

    public void HandleReceivedData(object sender, EventArgs e){

    }
    private bool setScanTime(){

        _serialport.Write($"ZT0,{UpscanTime},{DownscanTime}");
        _answer = _serialport.ReadLine();

        if(_answer == _positiveAnswer){

            return true;
        }
        else{
            return false;
        }
    }
    private bool setVoltage(){

        _serialport.Write($"ZV{MinVoltage},{MaxVoltage}");
        _answer = _serialport.ReadLine();
        

        if(_answer == _positiveAnswer){

            return true;
        }
        else{

            return false;
        }
    }
    private bool setScanDirection(){
        
        _serialport.Write($"ZU\n");

        _answer = _serialport.ReadLine();

        if(_answer == _positiveAnswer){

            return true;
        }
        else{
            return false;
        }       
    } 

    private int calculateVoltage(float diameter){

        return 0;
    }
    private  string _answer;
    private string _answerbuffer;

    private string _positiveAnswer = "OK\n";
    private string _negativeAnswer = "ERROR\n";
    private string _verificationstring = "123456789"; //serial number

}

}