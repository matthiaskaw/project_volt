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
    public ParticleCounter(string upscanTime, string downscanTime, string minVoltage, string maxVoltage){

        
        UpscanTime = upscanTime;
        DownscanTime = downscanTime;
        MinVoltage = minVoltage;
        MaxVoltage = maxVoltage; 
        UpscanDirection = true;
        
        DeviceType = EDeviceTypes.ParticleCounter;

        //_serialport.DataReceived += HandleReceivedData;
    }

    public ParticleCounter(){

        DeviceType = EDeviceTypes.ParticleCounter;
        
        DeviceID = "70701275";
    }


  

    public void Initialize(){
        
            Logger.WriteToLog("Particle Counter: Initializing");
       
            Logger.WriteToLog($"Particle Counter: Trying to verify Particle Counter on ports: {SerialPort.GetPortNames().ToString()}.");

            if(SerialPort.GetPortNames().Length == 0){

                Logger.WriteToLog("PArticle Counter: No serial port available. Please check connection!");
            }
     
            foreach(string portname in SerialPort.GetPortNames()){
            
                

                Logger.WriteToLog($"Particle Counter: Verifying on Port {portname}");
                _serialport.PortName = portname;
                
                Logger.WriteToLog($"Particle Counter: Trying to open serial port on port {portname}");
                try{
                    _serialport.Open();
                }
                catch(Exception e){

                    Logger.WriteToLog($"Particle Counter: Tried to open port {portname}. Access denied! Trying next port");
                    continue;

                }
                
                if(!_serialport.IsOpen){
                    
                    Logger.WriteToLog($"Particle Counter: Serial port is not open {_serialport.PortName}. Trying next port...");
                    continue;
                }

                Logger.WriteToLog($"Particle Counter: Starting verification. Requesting serial number {DeviceID}");
                _serialport.Write("RSN\r");   
                _serialport.ReadTimeout = 5000;
                
                try{
                    _answer = _serialport.ReadTo("\r");
                }
                catch(TimeoutException){
                        
                    Logger.WriteToLog($"Particle Counter: Initialize() read timed out answer = {_answer}");
                    _serialport.Close();
                    continue;
                }

                    
                if(_answer.Contains(DeviceID)){
            
                    _serialport.ReadTimeout = -1;
                    Initialized?.Invoke(this, new EventArgs());
                    Logger.WriteToLog($"Particle Counter: Particle Counter verified on port {_serialport.PortName}");
                    break;
                }

                
                if(portname == SerialPort.GetPortNames().Last()){

                    Logger.WriteToLog($"Particle Counter: Reached last element with no successfull verification");
                    throw new InitalizationFailedException("Particle Counter");
                }   
        }
        
    }

    public event EventHandler Initialized;
  
    //PRIVATE METHODS
   

    public bool SetScanMode(){

        Logger.WriteToLog($"Particle Counter: SetScanMode...");

        _serialport.Write("SCM,2\r");
        System.Threading.Thread.Sleep(500);
        _answer = _serialport.ReadTo("\r");

        if(_answer == _positiveAnswer){

            Logger.WriteToLog($"Particle Counter: SetScanMode(): {_answer}: ScanMode set to SMPS!");        
            return true;
        }
        else{

            Logger.WriteToLog($"Particle Counter: SetScanMode(): {_answer}: Scan mode not set to SMPS");
            return false;
        }

    }
    public bool SetScanTime(){
        
        Logger.WriteToLog($"Particle Counter: SetScanTime(): Setting Scan Time, Upscantime = {UpscanTime}, DownscanTime = {DownscanTime} ");

        _serialport.Write($"ZT0,{UpscanTime},{DownscanTime}\r");
        

         _answer = _serialport.ReadTo("\r");

        if(_answer == _positiveAnswer){

            Logger.WriteToLog($"Particle Counter: SetScanTime(): {_answer}: Scantime set! ");        
            return true;
        }
        else{

            Logger.WriteToLog($"Particle Counter: SetScanTime(): {_answer}: Scantime not set! ");        
            return false;
        }
    }
     public bool SetVoltage(){

        Logger.WriteToLog($"Particle Counter: SetVoltage(): Setting Voltage, MinVoltage = {MinVoltage}, DownscanTime = {MaxVoltage} ");
        _serialport.Write($"ZV{MinVoltage},{MaxVoltage}\r");
        
        _answer = _serialport.ReadTo("\r");
        
        if(_answer == _positiveAnswer){

            Logger.WriteToLog($"Particle Counter: SetVoltage(): {_answer}: Voltage set! ");        
            return true;
        }
        else{
            Logger.WriteToLog($"Particle Counter: SetVoltage(): {_answer}: Voltage not set! ");
            return false;
        }
    }
    public bool SetScanDirection(){
        

        Logger.WriteToLog($"Particle Counter: SetScanDirection(): Setting scan direction");
        _serialport.Write($"ZU\r");

        _answer = _serialport.ReadTo("\r");

        if(_answer == _positiveAnswer){
            Logger.WriteToLog($"Particle Counter: SetScanDirection(): {_answer}: Direction set! ");
            return true;
        }
        else{
            Logger.WriteToLog($"Particle Counter: SetVoltage(): {_answer}: Direction set! ");
            return false;
        }       
    }

    public bool Start(){

        _serialport.Write("ZB\r");

        System.Threading.Thread.Sleep(500);

        _answer = _serialport.ReadTo("\r");
        
        if(_answer == _positiveAnswer){

            Logger.WriteToLog($"Particle Counter: Start: {_answer}: Starting SMPS");
            _isMeasuring = true;
            return true;
            
        }
        else{

            Logger.WriteToLog($"Particle Counter: Start: {_answer}: Cannot start SMPS");
            return false;
        }



    }

    public bool End(){

        Logger.WriteToLog($"Particle Counter: End(): Ending Measurement");

        _serialport.Write("ZE\r");
        System.Threading.Thread.Sleep(500);
        _answer = _serialport.ReadTo("\r");

        if(_answer == _positiveAnswer){

            Logger.WriteToLog($"Particle Counter: End(): Measurement ended!");
            return true;
        }
        else{

            Logger.WriteToLog($"Particle Counter: End(): Measurement not ended succesfully!");
            return false;
        }

    }

    public async Task<List<string>> CollectDataAsync(){

        List<string> data = new List<string>();

        while(_isMeasuring){

            string line = _serialport.ReadTo("\r");
            Logger.WriteToLog($"Particle Counter: CollectData: line = {line}");
            data.Add(line);
            if(line.Contains("-1")){

                _isMeasuring  = false;

            }
        }
        End();

        return data;
        
    }


    public static string CalculateVoltage(string diameter){

        double sheathflow = 15.0/60000;
        double doublediameter = float.Parse(diameter);
        doublediameter = doublediameter*1e-9;
        double cunningham = Aerosol.CunninghamCorrection(doublediameter);


        Logger.WriteToLog($"_calculateVoltage: Aerosol.AirViscosity() = {Aerosol.Airviscosity()}");
        double voltage = doublediameter*3*Aerosol.Airviscosity()*sheathflow*Math.Log(1.905/0.937)/(2*1.6e-19*cunningham*0.04987);

        Logger.WriteToLog($"SMPSMeasurementAlgorithm: _calculateVoltage: voltage = {voltage}");

        int Ivoltage = (int)Math.Round((float)voltage);

        Logger.WriteToLog($"SMPSMeasurementAlgorithm: _calculateVoltage: voltage as integer is = {Ivoltage}");

        if(Ivoltage < 10){

            Ivoltage = 10;
        }
        if(Ivoltage > 10000){

            Ivoltage = 10000;
        }

        return Ivoltage.ToString();
    }

    
    //properties
   
    public string UpscanTime{get; set;}
    public string DownscanTime{get; set;}
    public string MinVoltage{get; set;}
    public string MaxVoltage{get; set;}
    public bool UpscanDirection{get; set;}
    public string SerialNumber {get; set;}
    public string DeviceID{get; set;}
    public EDeviceTypes DeviceType {get;}
    public bool IsInitialized {get;}



    private SerialPort _serialport = new SerialPort(){BaudRate = 115200, DataBits = 8, Parity = Parity.None, StopBits = StopBits.One};
    private string _positiveAnswer = "OK";
    private string _negativeAnswer = "ERROR";
    private string _answer = "70701275"; //serial number
    private bool _isMeasuring = false;

}

}