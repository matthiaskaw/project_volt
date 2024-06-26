using System.IO.Ports;
using System;
using System.Collections.Generic;
namespace Test{


    public class SerialPortTest{

        public SerialPortTest(){

            PortName = "COM1";
            DataBits = 8;
            StopBits = 1;
            BaudRate = 11500;
            _output = "";

        }

        public string PortName {get; set;}
        public int DataBits {get; set;}

        public int StopBits {get; set;}

        public System.IO.Ports.Parity Parity {get; set;}
        public int BaudRate { get; set; }

        public event EventHandler DataReceived;

        public void Write (string text){

            if(text == "RSN\n"){

                sendanswer("123456789");

            }
            else if(text == "ZV0,10000\n"){
                sendanswer("OK\n");
            }
            else if(text == "SCM,2\n"){

                sendanswer("OK\n");
            }
            else if(text == "ZU\n"){

                sendanswer("OK\n");
            }
            else if(text == "ZT0,1200,150\n"){

                sendanswer("OK\n");
            }

            
        } 

        public string ReadLine(){
            return _output;
            

        }
 
        private void sendanswer(string ans){

            _output = ans;
            DataReceived?.Invoke(this, new EventArgs());
        }

        private void emulateMeasurement(){

            int time = 0;
            Random random = new Random();
            while(time < 10){

                sendanswer($"{random.Next()}\n");
                Thread.Sleep(100);
            }
        }

        public bool Open(bool isOpen){
            IsOpen = isOpen;

            return isOpen;
        }

        public bool IsOpen {get; set;}
        private string _output;
      

        static public string[] GetPortNamesTest(){
            string[] s = new string[3];

            s[0] = "COM1";
            s[1] = "COM2";
            s[2] = "COM3"; 
            return s;

        }

        

    }

}

public class ModbusRTUTest{

    public ModbusRTUTest(){ }
        

    public string PortName {get; set;}
    public bool IsConnected{get; set;}


    public Span<byte> ReadHoldingRegisters(int unitIdentifier, int startingAddress, uint lengths){
        byte[] arr = new byte[]{0x31, 0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39};
        return new Span<byte>(arr);

    }

    public void WriteSingleCoil(int unitIdentifier, int registerAddress, bool value){

        
    }

    public void Connect(string portname){


    }

    

  
}