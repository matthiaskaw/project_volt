using System;
using System.ComponentModel.DataAnnotations;

namespace Services{
    public enum EMeasurementSettings
    {

        SheathFlow = 0,
        ScanTimeConstant = 1,
        DownscanTime = 2,
        UpscanTime = 3,
        SMPSMinDiameter = 4,
        SMPSMaxDiameter = 5,
        TandemTemperatureMinCurrent = 6,
        TandemTemperatureMaxCurrent = 7,
        SMPSDMAType = 8,
        TandemTemperatureVector = 9,
        TandemDMAVector = 10,
        TandemDMADMAType = 11,
        TandemDMAMinDiameter = 12,
        TandemDMAMaxDiameter = 13,
        SMPSDiameterVector = 14,
        CurrentReadingTime = 15

    }

    public class MeasurementSettings : ISettings{

        public MeasurementSettings(string settingsdirectory){

            SettingsDirectory = settingsdirectory;

            _measurementFilename = System.IO.Path.Combine(SettingsDirectory, this.GetType().ToString()+".set");

            if(!System.IO.File.Exists(_measurementFilename)){

                _setupSettings();
            }

            _settings = LoadSettings();

        }

        public Dictionary<Enum, string> LoadSettings(){
            
            Dictionary<Enum, string> tempsettings = new Dictionary<Enum, string>();

            try{

                string[] filecontent = File.ReadAllLines(_measurementFilename);
                //Create dictionary for settings with blank values
                string[] enumNames =  Enum.GetNames<EMeasurementSettings>();
            
                foreach(string line in filecontent){

                    string[] keyvalue = line.Split('\t');
                    string key = keyvalue[0];
                    string value = keyvalue[1];
                    Logger.WriteToLog($"MeasurementSettingsService: key = {key}; value = {value}");
                    tempsettings.Add((EMeasurementSettings)Enum.Parse(typeof(EMeasurementSettings), key), value);

                }


                //Iterate through each line; get the Settings name and value and store them in the Dictionary; name\tvalue;

            
            }
            catch (Exception e){

                Logger.WriteToLog($"MeasurementSettingsServic: LoadSetting: Unable to Read Settings. {e}");

            }            

            return tempsettings;

        }
        public void SaveSettings(){
            

            List<string> lines = new List<string>();
            //make string array for each keyvalue pair of _settings;
            foreach(KeyValuePair<Enum, string> kvpair in _settings){

                string line = kvpair.Key.ToString()+"\t"+kvpair.Value;
                Logger.WriteToLog($"MeasurementSettingsService: SaveSettings: line to save: {line}");
                lines.Add(line);
            }
            System.IO.File.WriteAllLines(_measurementFilename, lines);
        }

        public void SetSettingByKey(Enum key, string newvalue){

            if(!_validatesettingvalue(key, newvalue)){

                Logger.WriteToLog($"MeasurementSettingsService.cs: SetSettingByKey(): newvalue ({newvalue}) is invalid!");
                return;
            }

            Logger.WriteToLog($"MeasurementSettingsService.cs: SetSettingByKey(): {key.ToString()} is valid and set to {newvalue}!");

            _settings[key] = newvalue;
            Logger.WriteToLog($"MeasurementSettingsService.cs: SetSettingByKey(): {key.ToString()} is valid and set to {newvalue}!");
        }


        public string GetSettingByKey(Enum key){
            
            Logger.WriteToLog($"MeasurementSettingsService.cs: GetSettingByKey(): key ({key.ToString()}).");
            return _settings[key];
            
        }
       
        public string SettingsDirectory{get; set;}
        private void _setupSettings(){

            Dictionary<Enum, string> tempsettings = new Dictionary<Enum, string>();
            tempsettings.Add(EMeasurementSettings.SheathFlow, "15");
            tempsettings.Add(EMeasurementSettings.ScanTimeConstant, "17");
            tempsettings.Add(EMeasurementSettings.UpscanTime, "120");
            tempsettings.Add(EMeasurementSettings.DownscanTime, "15");
            tempsettings.Add(EMeasurementSettings.SMPSMinDiameter, "2.4");
            tempsettings.Add(EMeasurementSettings.SMPSMaxDiameter, "68.6");
            tempsettings.Add(EMeasurementSettings.TandemTemperatureMinCurrent, "0");
            tempsettings.Add(EMeasurementSettings.TandemTemperatureMaxCurrent, "30");
            tempsettings.Add(EMeasurementSettings.SMPSDMAType, "3085");
            tempsettings.Add(EMeasurementSettings.TandemTemperatureVector, "0;1;2;3;4;5;6;7;8;9;10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;25;26;27;28;29;30");
            tempsettings.Add(EMeasurementSettings.TandemDMAVector, "2.4;2.5;2.6;2.7;2.8;2.9;3.0;3.1");
            tempsettings.Add(EMeasurementSettings.TandemDMADMAType, "3085");
            tempsettings.Add(EMeasurementSettings.SMPSDiameterVector, "2.4;68.6");
            tempsettings.Add(EMeasurementSettings.CurrentReadingTime, "1;2;4;8;10;20;40;80;100;200;400;800;1000");    
            _settings = tempsettings;
            SaveSettings();
        }

        private bool _validatesettingvalue(Enum key, string? newvalue){

            float i;
            EMeasurementSettings currentKey = (EMeasurementSettings)key;
            switch(currentKey){

                case EMeasurementSettings.SheathFlow:
                    return float.TryParse(newvalue, out i);

                case EMeasurementSettings.ScanTimeConstant:
                    return float.TryParse(newvalue, out i);
                    
                case EMeasurementSettings.DownscanTime:
                    return float.TryParse(newvalue, out i);
                    
                case EMeasurementSettings.UpscanTime:
                    return float.TryParse(newvalue, out i);

                case EMeasurementSettings.SMPSMinDiameter:
                    return float.TryParse(newvalue, out i);
                
                case EMeasurementSettings.SMPSMaxDiameter:
                    return float.TryParse(newvalue, out i);
                
                case EMeasurementSettings.TandemTemperatureMinCurrent:
                    return float.TryParse(newvalue, out i);

                case EMeasurementSettings.TandemTemperatureMaxCurrent:
                    return float.TryParse(newvalue, out i);

                case EMeasurementSettings.SMPSDMAType:
                    //Check if DMA Type is 3085 or 3081
                    //Maybe restrict option by using a dropbox in UI
                    break;
                
                case EMeasurementSettings.TandemDMAMinDiameter:
                    return float.TryParse(newvalue, out i);

                case EMeasurementSettings.TandemDMAMaxDiameter:
                    return float.TryParse(newvalue, out i);
                
                default:

                    Logger.WriteToLog($"MeasurementSettingsService.cs: _validatesettingvalue(): {key} cannot be validated!");
                    break;
            }
            return true;
       
        }
        private string _measurementFilename;
        private Dictionary<Enum, string> _settings;
        




    }

    
}