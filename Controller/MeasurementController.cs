using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using DatabaseModel;
using MeasurementAlgorithms;
using Device;
using Services;

namespace Measurement{


    public class MeasurementController{

        private static MeasurementController _instance;

        private MeasurementController(){

            MeasurementType = EMeasurementType.Default;
        
        }

        public static MeasurementController Instance{

            get{

                if(_instance == null){
                    _instance = new MeasurementController();
                }
                return _instance;
            }    
        }
        

        //PUBLIC METHODS
        public async Task<bool> StartMeasurement(){

            List<string> metadata = new List<string>();
            switch(MeasurementType){

                case EMeasurementType.Default:

                    Logger.WriteToLog($"MeasurementController: Start Measurement(): MeasurementType is {MeasurementType.ToString()}");
                    Logger.WriteToLog($"MeasurementController: Start Measurement(): No Measurement Type set");
                    break;

                case EMeasurementType.SMPS:

                    _setSMPSMetaData(ref metadata);
                    
                    Logger.WriteToLog($"MeasurementController: Start Measurement(): MeasurementType is {MeasurementType.ToString()}");
                    _measurementAlgorithm = new SMPSMeasurementAlgorithm();
                    break;

                case EMeasurementType.TandemDMA:
                    
                    _setSMPSMetaData(ref metadata);

                    metadata.Add(EMeasurementSettings.TandemDMAMinDiameter.ToString() + ": " + SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.TandemDMAMinDiameter));
                    metadata.Add(EMeasurementSettings.TandemDMAMaxDiameter.ToString() + ": " + SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.TandemDMAMinDiameter));
                    metadata.Add(EMeasurementSettings.TandemDMADMAType.ToString() + ": " + SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.TandemDMADMAType));
                    metadata.Add(EMeasurementSettings.FurnaceCurrent.ToString() + ": " + SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.FurnaceCurrent));

                    Logger.WriteToLog($"MeasurementController: Start Measurement(): MeasurementType is {MeasurementType.ToString()}");
                    
                    break;

                case EMeasurementType.TandemTemperature:

                 
                    _setSMPSMetaData(ref metadata);
                    
                    //PowerSource Metadata:
                    metadata.Add(EMeasurementSettings.TandemTemperatureMinCurrent.ToString() + ": " + SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.TandemTemperatureMinCurrent));
                    metadata.Add(EMeasurementSettings.TandemTemperatureMaxCurrent.ToString() + ": " + SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.TandemTemperatureMaxCurrent));
                    metadata.Add(EMeasurementSettings.TandemDMADMAType.ToString() + ": " + SettingsService.Instance);
                    metadata.Add(EMeasurementSettings.FirstDMADiameter.ToString() + ": " + SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.FirstDMADiameter));


                    Logger.WriteToLog($"MeasurementController: Start Measurement(): MeasurementType is {MeasurementType.ToString()}");
                    _measurementAlgorithm = new TandemTemperatureAlgorithm();
                    break;

                case EMeasurementType.CurrentMeasurement:
                    Logger.WriteToLog($"MeasurementController: StartMeasurement(): MeasurementType is {MeasurementType.ToString()}");
                    _measurementAlgorithm = new SensorMeasurementAlgorithm();
                    break;
            }
            
            if(_measurementAlgorithm == null){
                Logger.WriteToLog($"MeasurementController: StartMeasurement(): measurementAlgorithm is null!");
                return false;
            }
            
            List<string> data = await _measurementAlgorithm.RunMeasurement();

            CurrentDataset.Path = DataController.Instance.SaveDataset(data, CurrentDataset.Name, MeasurementType.ToString());
            DataController.Instance.SaveDatasetMetaData(metadata, CurrentDataset.Name, MeasurementType.ToString());
           
            DataController.Instance.DbContext.Datasets.Add(CurrentDataset);
            DataController.Instance.DbContext.SaveChanges();

            return true;
        }

        public void Cancel(){

            _measurementAlgorithm.IsRunning = false;
            _onCanceled();

        }

        //PROPERTIES
        public event EventHandler Canceled;        
        public EMeasurementType MeasurementType {get; set;}
        public Dataset CurrentDataset {get;set;} = null;

        private void _onCanceled(){

            Canceled?.Invoke(this, new EventArgs());
        }

        private void _setSMPSMetaData(ref List<string> metadata){
            
            metadata.Add(EMeasurementSettings.SMPSMinDiameter.ToString() + ": " + SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSMinDiameter));
            metadata.Add(EMeasurementSettings.SMPSMinDiameter.ToString() + ": " + SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSMaxDiameter));
            metadata.Add("Minimum Voltage" + ": " +  ParticleCounter.CalculateVoltage(SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSMinDiameter)));
            metadata.Add(EMeasurementSettings.SMPSMaxDiameter.ToString() + ": " + SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSMaxDiameter));
            metadata.Add("Maximum Voltage" + ": " +  ParticleCounter.CalculateVoltage(SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSMaxDiameter)));
            metadata.Add(EMeasurementSettings.UpscanTime.ToString() + ": " + SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.UpscanTime));
            metadata.Add(EMeasurementSettings.DownscanTime.ToString() + ": " + SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.DownscanTime));
            metadata.Add(EMeasurementSettings.SheathFlow.ToString() + ": " + SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SheathFlow));
            metadata.Add(EMeasurementSettings.SMPSDMAType.ToString() + ": " + SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSDMAType));

        }


        private IMeasurementAlgorithm _measurementAlgorithm;


    }

    
}