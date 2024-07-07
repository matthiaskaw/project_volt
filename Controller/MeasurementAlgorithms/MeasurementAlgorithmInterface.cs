

namespace MeasurementAlgorithms{

    public interface IMeasurementAlgorithm{

        Task<List<string>> RunMeasurement();

        //public void StoreData(List<string> data);

        public bool IsRunning{get; set;}
    }

}
