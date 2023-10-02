
namespace Measurement{
    public interface IMeasurement{

        void StartMeasurement();
        void EndMeasurement();
        void CancelMeasurement();

        event EventHandler StartedMeasurement;
        event EventHandler EndedMeasurement;
        event EventHandler CanceledMeasurement;

    }

    public enum EMeasurementType{
        SMPS = 0,
        TandemPyrolysis = 1,

        Temperature = 2

    }


}

