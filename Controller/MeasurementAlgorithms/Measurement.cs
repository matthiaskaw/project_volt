using System;


namespace Measurement{
    public enum EMeasurementType{
        
        Default = 0,
        SMPS = 1,
        TandemDMA = 2,
        TandemTemperature = 3

    }


    public static class Aerosol{
        public static double CunninghamCorrection(double diameter){
            
            double freemeanpath = 66e-9; 

            return 1+(freemeanpath/diameter)*(2.514/0.8*Math.Exp(-0.55*diameter/freemeanpath));
        }

        
        public static double Airviscosity(){return 1.72e-5;}
    }

}

