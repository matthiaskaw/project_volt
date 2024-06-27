using System;
using System.ComponentModel.DataAnnotations;
using MeasurementAlgorithms;


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

            IMeasurementAlgorithm measurementAlgorithm = null;
            switch(MeasurementType){

                case EMeasurementType.Default:

                    Logger.WriteToLog($"MeasurementController: Start Measurement(): MeasurementType is {MeasurementType.ToString()}");
                    Logger.WriteToLog($"MeasurementController: Start Measurement(): No Measurement Type set");



                    break;

                case EMeasurementType.SMPS:

                    Logger.WriteToLog($"MeasurementController: Start Measurement(): MeasurementType is {MeasurementType.ToString()}");

                    measurementAlgorithm = new SMPSMeasurementAlgorithm();
                    break;

                case EMeasurementType.TandemDMA:
                   
                    Logger.WriteToLog($"MeasurementController: Start Measurement(): MeasurementType is {MeasurementType.ToString()}");
                    //measurementAlgorithm = new SMPSMeasurementAlgorithm();
                    //measurementAlgorithm = new Tandem()
                    break;

                case EMeasurementType.TandemTemperature:
                    Logger.WriteToLog($"MeasurementController: Start Measurement(): MeasurementType is {MeasurementType.ToString()}");
                    measurementAlgorithm = new TandemTemperatureAlgorithm();
                    break;

                case EMeasurementType.CurrentMeasurement:
                    Logger.WriteToLog($"MeasurementController: StartMeasurement(): MeasurementType is {MeasurementType.ToString()}");
                    measurementAlgorithm = new SensorMeasurement();
                    break;
        

            }
            
            if(measurementAlgorithm == null){
                Logger.WriteToLog($"MeasurementController: StartMeasurement(): measurementAlgorithm is null!");
                return false;
            }
            
            return await measurementAlgorithm.RunMeasurement();
            
            }

        public void StartSensorWatching(){
            
            IMeasurementAlgorithm algorithm = new SensorMeasurement();
            algorithm.RunMeasurement();
            


        }


        //PROPERTIES
        public EMeasurementType MeasurementType {get; set;}




    }

    
}