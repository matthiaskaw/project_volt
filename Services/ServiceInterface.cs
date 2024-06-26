namespace Services{

    
    public interface ISettings{

        public Dictionary<Enum,string> LoadSettings();
        public void SaveSettings();
        public void SetSettingByKey(Enum key, string newvalue);
        public string GetSettingByKey(Enum key);
        public string SettingsDirectory {get; set;}
    }
}