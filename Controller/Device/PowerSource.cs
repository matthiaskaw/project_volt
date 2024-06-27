using System.IO.Ports;
using FluentModbus;
using Test;
using System.Text;



namespace Device{


    public class PowerSource : IDevice{

        public PowerSource(){

            DeviceID = "1610630001";
            DeviceType = EDeviceTypes.PowerSource;
            
        }

        public void Initialize(){


            Logger.WriteToLog("Power Source: Trying to initialize Power Source");
            Logger.WriteToLog($"Power Source: Possible SerialPorts {SerialPort.GetPortNames().ToString()}");
            
            foreach(string portname in SerialPort.GetPortNames()){
                if(_serialPort.IsOpen){

                    Logger.WriteToLog($"PowerSource: Serialport {portname} already open! Trying next");
                    continue;
                }
                _serialPort.PortName = portname;
                Logger.WriteToLog($"Power Source: Trying to open to Power Source on {portname}");           
                try{
                    _serialPort.Open();
                }
                catch(Exception e){

                    Logger.WriteToLog($"Power Source: Cannot open port {portname}.Exception: {e}");
                }

                if(!_serialPort.IsOpen){

                    Logger.WriteToLog($"Power Source: Initialize(): _serialPort is not open! (Portname: {_serialPort.PortName})");
                    continue;
                }

                if(!_enableDeviceToRemoteControl()){

                    Logger.WriteToLog($"Power Source: Initialize(): Was not able to enable Remote Control! (Portname: {_serialPort.PortName}))");
                    continue;
                }

                Logger.WriteToLog($"Power Source: Initialize(): Trying to verify decive on port {_serialPort.PortName}");

                Logger.WriteToLog($"Power Source: Initialize(): Calling _getDeviceID");
                
                string receivedDeviceID = _getDeviceID();
                
                if(!receivedDeviceID.Contains(DeviceID)){

                    Logger.WriteToLog($"Power Source: Initialize(): Verification failed! Received DeviceID is {receivedDeviceID}. Saved Device ID is {DeviceID}");
                    _serialPort.Close();
                    continue;
                }



                Logger.WriteToLog($"Power Source: Initialize(): Verfication successfull! Device verified on port {_serialPort.PortName} with ID {DeviceID}");
                _switchDeviceOn();
                Initialized?.Invoke(this, new EventArgs());
                return;

            }

            Logger.WriteToLog($"Power Source: Initialize(): No device found!");
            //throw exception;

        }
 

        public bool End(){
            
            if(!_switchDeviceOff()){

                Logger.WriteToLog($"Power Source: End(): Cannot switch off device!");
                return false;
            }

            if(!_disableDeviceToRemoteControl()){

                Logger.WriteToLog($"Power Source: End(): Cannot switch off disable remote control");
                return false;
            }

            return true;


        }

        public bool SetCurrent(double current){
            
            UInt16 currentPercentage = (UInt16)Math.Round(52428*current/60);
            Logger.WriteToLog($"Power Source: SetCurrent: Passed current = {current}; currentPercentage = {currentPercentage}");

            byte[] telegram = new byte[8];

            telegram[0] = 0x00;
            telegram[1] = 0x06;
            telegram[2] = 0x01;
            telegram[3] = 0xF5;
            
            UInt16 data_lowerbyte = (UInt16)(currentPercentage & 0xff);
            UInt16 data_upperbyte = (UInt16)(currentPercentage >> 8);

            telegram[4] = (byte)data_lowerbyte;
            telegram[5] = (byte)data_upperbyte;

            UInt16 crc = _calculateCRC(new byte[6]{telegram[0],
                            telegram[1],
                            telegram[2],
                            telegram[3],
                            telegram[4],
                            telegram[5]});

            UInt16 crc_lowerbyte = (UInt16)(crc & 0xff);
            UInt16 crc_upperbyte = (UInt16)(crc >> 8);

            telegram[6] = (byte)crc_lowerbyte;
            telegram[7] = (byte)crc_upperbyte;


            _serialPort.Write(telegram, 0,8);

            byte [] buffer=new byte[8];

            int bufferSize = _serialPort.Read(buffer, 0,8);


            if(bufferSize < 8 ){

                Logger.WriteToLog($"PowerSource: Set Current: buffer has invalid size (buffer = {BitConverter.ToString(buffer)}, {bufferSize})");
                return false;
            }

            
            UInt16 bufferdata_lowerbyte = (UInt16)(buffer[4] & 0xff);
            UInt16 bufferdata_upperbyte = (UInt16)(buffer[5] >> 8); 
            
            UInt16 datapercentage = (UInt16)(bufferdata_lowerbyte+bufferdata_upperbyte);
            
            if(current != datapercentage){

                Logger.WriteToLog($"Power Source: SetCurrent: Trying to set current to {current} failed. Received data is {datapercentage}");
                Logger.WriteToLog($"Power Source: SetCurrent: received answer is {BitConverter.ToString(buffer)}");
                return false;
            }

            
            double setcurrent = (double)60*(double)datapercentage/(double)52428;

            Logger.WriteToLog($"Power Source: SetCurrent: Current set to {setcurrent}");
            return true;

            
       }



        public event EventHandler Initialized;

        public bool IsInitialized {get;}

        //PRIVATE METHODS

        bool _enableDeviceToRemoteControl(){

            byte[] telegram = new byte[8]; 
            telegram[0] = 0x00;
            telegram[1] = 0x05;
            telegram[2] = 0x01;
            telegram[3] = 0x92;
            telegram[4] = 0xFF;
            telegram[5] = 0x00;

            ushort CRC = _calculateCRC(new byte [6]{0x00,0x05,0x01,0x92,0xff,0x00});

            ushort CRC_lowerbyte = (ushort)(CRC & 0xff);
            ushort CRC_upperbyte = (ushort)(CRC >> 8);

            telegram[6] = (byte)CRC_lowerbyte;
            telegram[7] = (byte)CRC_upperbyte;
            
            Logger.WriteToLog($"Power Source: _enableDeviceToRemoteContro: Written telegram {BitConverter.ToString(telegram)}!");
            Logger.WriteToLog($"Power Source: _enableDeviceToRemoteContro: Checking if _serialPort is open!");
            if(!_serialPort.IsOpen){
                Logger.WriteToLog($"Power Source: _enableDeviceToRemoteControl: _serialPort is not open! (Port: {_serialPort.PortName})"); 
                return false;
            }
            Logger.WriteToLog($"Power Source: _enableDeviceToRemoteControl: _serialPort open! (Port: {_serialPort.PortName}) Trying to enable remote control!"); 

            _serialPort.Write(telegram, 0, telegram.Length);

            byte[] buffer = new byte[8];
            int bufferSize = _serialPort.Read(buffer, 0,8);

            
            Logger.WriteToLog($"Power Source: Reading Buffer...");
            int i = 0;
            Logger.WriteToLog($"Power Source: buffer = {BitConverter.ToString(buffer)}...");
            

            Logger.WriteToLog($"Power Source: _enableDeviceToRemoteContro: Checking if buffer size is valid...");
            if(bufferSize < 8){

                Logger.WriteToLog($"Power Source: _enableRemoteControl: Length of received answer is to small! (Buffer size = {bufferSize})");
                return false;
            }
            Logger.WriteToLog($"Power Source: _enableDeviceToRemoteContro: Buffer size ({bufferSize}) is valid!");

            Logger.WriteToLog($"Power Source: _enableDeviceToRemoteContro: Checking if received answer is positive (buffer[4] = {buffer[4]}, buffer[5] = {buffer[5]}) ...");

            if(!(buffer[4] == 0xff) && !(buffer[5] == 0x00)){

                Logger.WriteToLog($"Power Source: _enableRemoteControl: Remote Control not enabled! (buffer[4] = {buffer[4]}, buffer[5] = {buffer[5]})");
                
            }
            Logger.WriteToLog($"Power Source: _enableDeviceToRemoteContro: Received answer is positive! Remote Control is enabled;!");

            return true;
        }

        bool _disableDeviceToRemoteControl(){
            
            byte[] telegram = new byte[8]; 
            telegram[0] = 0x00;
            telegram[1] = 0x05;
            telegram[2] = 0x01;
            telegram[3] = 0x92;
            telegram[4] = 0x00;
            telegram[5] = 0x00;

            ushort CRC = _calculateCRC(new byte [6]{0x00,0x05,0x01,0x92,0x00,0x00});

            ushort CRC_lowerbyte = (ushort)(CRC & 0xff);
            ushort CRC_upperbyte = (ushort)(CRC >> 8);

            telegram[6] = (byte)CRC_lowerbyte;
            telegram[7] = (byte)CRC_upperbyte;
            
            Logger.WriteToLog($"Power Source: _enableDeviceToRemoteContro: Written telegram {BitConverter.ToString(telegram)}!");
            Logger.WriteToLog($"Power Source: _enableDeviceToRemoteContro: Checking if _serialPort is open!");
            if(!_serialPort.IsOpen){
                Logger.WriteToLog($"Power Source: _enableDeviceToRemoteControl: _serialPort is not open! (Port: {_serialPort.PortName})"); 
                return false;
            }
            Logger.WriteToLog($"Power Source: _enableDeviceToRemoteControl: _serialPort open! (Port: {_serialPort.PortName}) Trying to enable remote control!"); 

            _serialPort.Write(telegram, 0, telegram.Length);

            byte[] buffer = new byte[8];
            int bufferSize = _serialPort.Read(buffer, 0,8);

            
            Logger.WriteToLog($"Power Source: Reading Buffer...");
            int i = 0;
            Logger.WriteToLog($"Power Source: buffer = {BitConverter.ToString(buffer)}...");
            

            Logger.WriteToLog($"Power Source: _enableDeviceToRemoteContro: Checking if buffer size is valid...");
            if(bufferSize < 8){

                Logger.WriteToLog($"Power Source: _enableRemoteControl: Length of received answer is to small! (Buffer size = {bufferSize})");
                return false;
            }
            Logger.WriteToLog($"Power Source: _enableDeviceToRemoteContro: Buffer size ({bufferSize}) is valid!");

            Logger.WriteToLog($"Power Source: _enableDeviceToRemoteContro: Checking if received answer is positive (buffer[4] = {buffer[4]}, buffer[5] = {buffer[5]}) ...");

            if(!(buffer[4] == 0x00) && !(buffer[5] == 0x00)){

                Logger.WriteToLog($"Power Source: _enableRemoteControl: Remote Control not enabled! (buffer[4] = {buffer[4]}, buffer[5] = {buffer[5]})");
                
            }
            Logger.WriteToLog($"Power Source: _enableDeviceToRemoteContro: Received answer is positive! Remote Control is enabled;!");

            return true;
        }
        bool _switchDeviceOff(){

            byte[] telegram = new byte[8];

            telegram[0] = 0x00;
            telegram[1] = 0x05;
            telegram[2] = 0x01;
            telegram[3] = 0x95;
            telegram[4] = 0x00;
            telegram[5] = 0x00;
            
            ushort CRC = _calculateCRC(new byte [6]{0x00,0x05,0x01,0x92,0x00,0x00});
            ushort CRC_lowerbyte = (ushort)(CRC & 0xff);
            ushort CRC_upperbyte = (ushort)(CRC >> 8);

            telegram[6] = (byte)CRC_lowerbyte;
            telegram[7] = (byte)CRC_upperbyte;

            _serialPort.Write(telegram, 0,8);

             byte[] buffer = new byte[8];
            int bufferSize = _serialPort.Read(buffer, 0,8);

            
            Logger.WriteToLog($"Power Source: switchDeviceOn: Reading Buffer...");
            Logger.WriteToLog($"Power Source:switchDeviceOn: buffer = {BitConverter.ToString(buffer)}...");
            

            Logger.WriteToLog($"Power Source: switchDeviceOn: Checking if buffer size is valid...");
            if(bufferSize < 8){

                Logger.WriteToLog($"Power Source: switchDeviceOn: Length of received answer is to small! (Buffer size = {bufferSize})");
                return false;
            }
            Logger.WriteToLog($"Power Source: switchDeviceOn: Buffer size ({bufferSize}) is valid!");

            Logger.WriteToLog($"Power Source: switchDeviceOn: Checking if received answer is positive (buffer[4] = {buffer[4]}, buffer[5] = {buffer[5]}) ...");

            if(!(buffer[4] == 0x00) && !(buffer[5] == 0x00)){

                Logger.WriteToLog($"Power Source: switchDeviceOn: Remote Control not enabled! (buffer[4] = {buffer[4]}, buffer[5] = {buffer[5]})");
                
            }
            Logger.WriteToLog($"Power Source: switchDeviceOn: Received answer is positive! Remote Control is enabled;!");

            return true;
        }

        bool _switchDeviceOn(){
            
            byte[] telegram = new byte[8];

            telegram[0] = 0x00;
            telegram[1] = 0x05;
            telegram[2] = 0x01;
            telegram[3] = 0x95;
            telegram[4] = 0xFF;
            telegram[5] = 0x00;
            
            ushort CRC = _calculateCRC(new byte [6]{0x00,0x05,0x01,0x92,0xff,0x00});
            ushort CRC_lowerbyte = (ushort)(CRC & 0xff);
            ushort CRC_upperbyte = (ushort)(CRC >> 8);

            telegram[6] = (byte)CRC_lowerbyte;
            telegram[7] = (byte)CRC_upperbyte;

            _serialPort.Write(telegram, 0,8);

             byte[] buffer = new byte[8];
            int bufferSize = _serialPort.Read(buffer, 0,8);

            
            Logger.WriteToLog($"Power Source: switchDeviceOn: Reading Buffer...");
            int i = 0;
            Logger.WriteToLog($"Power Source:switchDeviceOn: buffer = {BitConverter.ToString(buffer)}...");
            

            Logger.WriteToLog($"Power Source: switchDeviceOn: Checking if buffer size is valid...");
            if(bufferSize < 8){

                Logger.WriteToLog($"Power Source: switchDeviceOn: Length of received answer is to small! (Buffer size = {bufferSize})");
                return false;
            }
            Logger.WriteToLog($"Power Source: switchDeviceOn: Buffer size ({bufferSize}) is valid!");

            Logger.WriteToLog($"Power Source: switchDeviceOn: Checking if received answer is positive (buffer[4] = {buffer[4]}, buffer[5] = {buffer[5]}) ...");

            if(!(buffer[4] == 0xff) && !(buffer[5] == 0x00)){

                Logger.WriteToLog($"Power Source: switchDeviceOn: Remote Control not enabled! (buffer[4] = {buffer[4]}, buffer[5] = {buffer[5]})");
                
            }
            Logger.WriteToLog($"Power Source: switchDeviceOn: Received answer is positive! Remote Control is enabled;!");

            return true;
            
        }

        string _getDeviceID(){

            byte[] telegram = new byte[8];
            telegram[0] = 0x00;
            telegram[1] = 0x03;
            telegram[2] = 0x00;
            telegram[3] = 0x97;
            telegram[4] = 0x00;
            telegram[5] = 0x14;

            Logger.WriteToLog($"Power Source: _getDeviceID: Calculation checksum of telegram...");

            ushort CRC = _calculateCRC(new byte[6]{0x00, 0x03, 0x00, 0x97, 0x00, 0x14});

            Logger.WriteToLog($"Power Source: _getDeviceID: Checksum is {CRC}");

            ushort CRC_lowerbyte = (ushort)(CRC & 0xff);
            ushort CRC_upperbyte = (ushort)(CRC >> 8);

            telegram[6] = (byte)CRC_lowerbyte;
            telegram[7] = (byte)CRC_upperbyte;
            Logger.WriteToLog($"Power Source: _getDeviceID: Writing telegram ({BitConverter.ToString(telegram)}) to port ({_serialPort.PortName})");
            
            
            _serialPort.Write(telegram, 0, telegram.Length);
            
            byte[] buffer = new byte[50];
            System.Threading.Thread.Sleep(500);
            int bufferSize = _serialPort.Read(buffer, 0,50);
            Logger.WriteToLog($"Power Source: _getDeviceID: Buffer is {BitConverter.ToString(buffer)}; Size ({buffer.Length})");
            Logger.WriteToLog($"Power Source: _getDeviceID: Checking buffer size...");
            if(bufferSize < 8){

                Logger.WriteToLog($"Power Source: _enableRemoteControl: Length of received answer is to small! (Buffer size = {bufferSize})");
                
            }
            Logger.WriteToLog($"Power Source: _getDeviceID: Buffer size is greater than 8!");
            Logger.WriteToLog($"Power Source: _getDeviceID: Getting DeviceID from buffer...");

            uint datalength = buffer[2];
            Logger.WriteToLog($"Power Source: _getDeviceID: Data length is {datalength}!");
            byte[] data = new byte[buffer.Length];
            
            for(uint i = 0; i <= datalength; i++){

                data[i] = buffer[i+3];    
            }
            Logger.WriteToLog($"Power Source: _getDeviceID: answer is {Encoding.ASCII.GetString(data)}");


        
        
            return Encoding.ASCII.GetString(data);
        }

        void _setPowerToMax(){

        }

        void _setVoltageToMax(){

        }

        

        UInt16 _calculateCRC(byte[] telegram){
            
            UInt16 CRC = 0xFFFF;
            for(int pos = 0; pos < telegram.Length; pos++){
            
                CRC ^= (UInt16)telegram[pos];
                for(int i = 8; i != 0; i--){

                    if((CRC & 0x0001) != 0){
                        CRC >>= 1;
                        CRC ^=0xA001;
                    }
                    else{
                        CRC >>= 1;
                    }
                }
            
            }
            return CRC;
        }

        

        private string ConvertModbusTelegramToString(Span<byte> telegram){
            byte[] answer = telegram.ToArray();
            string ans = Encoding.ASCII.GetString(answer);
            Logger.WriteToLog($"Power Source: ConvertModbusTelegramToString() {ans}");

            return ans;
        }
        //PROPERTIES
        public string DeviceID {get; set;}
        public EDeviceTypes DeviceType {get;}


        //PRIVATE MEMBER
        private ModbusRtuClient _modbusClient = new ModbusRtuClient(){

            BaudRate = 115200,
            StopBits = StopBits.One,
            Parity = Parity.None
            
        };

        private SerialPort _serialPort = new SerialPort(){

            BaudRate = 115200,
            StopBits = StopBits.One,
            Parity = Parity.None
        }; 
        
        
       }


       public class ModbusClient{


            enum EFunctionCodes {

                ReadCoils = 0x01,
                ReadHoldingRegister = 0x03,
                WriteSingleCoil = 0x05,
                WriteSingleRegister = 0x06,
                WriteMultipleRegisters = 0x10
            } 


            public ModbusClient(){}
            public byte[] WriteSingleCoil(uint address, bool data){
                return new byte[2];
                

                
            } 
            //Mobus-Telegram
            //Header Byte   Function-Code Byte  Register_first_byte register_second_byte    dataword_first_byte     dataword_second_byte CRC
       }

    
}