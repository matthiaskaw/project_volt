namespace MeasurementAlgorithms{

    public interface IMeasurementAlgorithm{

        Task<bool> RunMeasurement();
        public bool IsRunning{get; set;}
    }

}
