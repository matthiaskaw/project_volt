using System.Threading.Tasks.Dataflow;
using Device;



namespace MeasurementAlgorithms{
/*
public abstract class AbstractTandemMeasurementAlgorithm : IMeasurementAlgorithm{

    public AbstractTandemMeasurementAlgorithm(IDevice clock, IDevice clockReceiver, List<float> valueList){

        _clock = clock;
        _clockReceiver = clockReceiver;
        _valueList = valueList;
    }




    public void RunMeasurement(){

        foreach(float value in _valueList){
            
            _setValue(value);
            _measure();
        }
    }

    protected abstract void _setValue(float val);
    protected abstract void _measure();
    protected IDevice _clock;
    protected IDevice _clockReceiver;
    protected List<float> _valueList;

}

public class TandemDMAAlgorithm : AbstractTandemMeasurementAlgorithm{

    public TandemDMAAlgorithm(IDevice clock, IDevice clockReceiver, List<float> valueList) : base(clock,clockReceiver, valueList){}
    override protected void _setValue(float val){

        ElectrostaticClassifier clock = _clock as ElectrostaticClassifier;

        if(clock == null){

            Logger.WriteToLog("TandemDMAAlgorithm: Invalid cast to ElectrostaticClassifier!");
            return;
        }

        clock.Diameter = val;

        ParticleCounter clockReceiver = _clockReceiver as ParticleCounter;

        clockReceiver.MaxDiameter = val*1.2f;

    }
    override protected void _measure(){}
}*/
}
