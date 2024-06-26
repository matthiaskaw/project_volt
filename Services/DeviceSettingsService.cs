

namespace Services{

    public class DeviceSettingsService{

        private static DeviceSettingsService _instance;

        private DeviceSettingsService(){
            
            
        
        }

        public static DeviceSettingsService Instance{

            get{

                if(_instance == null){
                    _instance = new DeviceSettingsService();
                }
                return _instance;
            }
        }

        public void SaveSettings(){

            
        }

        private void _loadSettings(){


        }
    }
}