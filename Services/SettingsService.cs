namespace Services{


    public class SettingsService{

        public static SettingsService Instance {
            
            get{
            if(_instance == null){
                _instance = new SettingsService();
            }
            return _instance;
        }
        }
        private SettingsService(){

            Logger.WriteToLog($"SettingsService: Setting up measurement service...");

            string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            Logger.WriteToLog($"SettingsService: applicationPath = {applicationPath}");
            _settingsDirectoryPath = System.IO.Path.Combine(applicationPath, "settings");
            Logger.WriteToLog($"SettingsService: Settings path = {_settingsDirectoryPath}");

            if(!System.IO.Directory.Exists(_settingsDirectoryPath)){
                
                Logger.WriteToLog($"SettingsService: Creating directory {_settingsDirectoryPath}");
                System.IO.Directory.CreateDirectory(_settingsDirectoryPath);
            }

            _measurementSetting = new MeasurementSettings(_settingsDirectoryPath);

        }

        public void Save(){

            _measurementSetting.SaveSettings();
        }

        public void Load(){}

        public ISettings MeasurementSetting {get{return _measurementSetting;}}


        private static SettingsService _instance;
        private ISettings _measurementSetting; 
        private string _settingsDirectoryPath;
    }
}