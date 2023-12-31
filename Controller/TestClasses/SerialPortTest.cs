using System.IO.Ports;
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
      



    }
}
