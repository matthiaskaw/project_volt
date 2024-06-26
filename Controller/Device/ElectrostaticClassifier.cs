using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Threading;
using System.Reflection.Metadata.Ecma335;


using Measurement;
using Test;
using System.Text;

namespace Device{


    public class ElectrostaticClassifier : IDevice{

        public ElectrostaticClassifier(){
            DeviceType = EDeviceTypes.Classifier;
        }


        public void Initialize(){

            byte[] ipaddress = new byte[4]{172,16,1,100};
            int port = 3602;
            Logger.WriteToLog($"ElectrostaticClassifier: Initilizing Device with IP {ipaddress[0]}.{ipaddress[1]}.{ipaddress[2]}.{ipaddress[3]} and port {port}");

            try{
                System.Net.IPAddress ipADDRESS = new System.Net.IPAddress(ipaddress);
                Logger.WriteToLog($"Electrostatic classifier: ipaddress =     {ipADDRESS.ToString()}");
                _tcpclient.Connect(ipADDRESS, port);

                if(DeviceID == GetDeviceID()){

                    Initialized?.Invoke(this, new EventArgs());
                    Logger.WriteToLog($"ElectrostaticClassifier: Device verified! {DeviceID}");

                }
                else{

                    Logger.WriteToLog($"Electrostatic Classifier: Device ({ipaddress}, {port}) not verified! Check connection!");
                }
            }
            catch (ArgumentNullException e)
            {
                Logger.WriteToLog("ArgumentNullException: {0}" + e);
            }
            catch (SocketException e)
            {
                Logger.WriteToLog("S.ocketException: {0}" + e);
            }
}

        
        

        public event EventHandler Initialized;
        
        public string DeviceID {get; set;}
        public EDeviceTypes DeviceType {get;}

        private TcpClient _tcpclient = new TcpClient();

        public string GetDeviceID(){

            using NetworkStream stream = _tcpclient.GetStream();
            byte[] sendBuffer = Encoding.UTF8.GetBytes("RDSN");
            
            stream.Write(sendBuffer);

            byte[] receiveBuffer = new byte[1024];
            int bytesReceived = stream.Read(receiveBuffer);
            string data = Encoding.UTF8.GetString(receiveBuffer.AsSpan(0, bytesReceived));

            Logger.WriteToLog($"ElectrostaticClassifier: GetDeviceID() received Data = {data}");

            return data;
        }

        public bool Start(){
            return true;
        }

        public bool End(){
            return false;
        }
    }
}
 