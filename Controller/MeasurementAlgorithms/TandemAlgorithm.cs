using Device;


namespace TandemAlgorithms{

public abstract class AbstractMeasurementAlgorithm{

    public AbstractMeasurementAlgorithm(IDevice clock, IDevice clockReceiver, List<float> valueList){


        _clock = clock;
        _clockReceiver = clock;
        _valueList = valueList;

    }

    public AbstractMeasurementAlgorithm(IDevice measurementDevice, List<float> valueList){

        _clock = null;


    }

    public void Run(){


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

public class TandemDMAAlgorithm : AbstractMeasurementAlgorithm{


    public TandemDMAAlgorithm(IDevice clock, IDevice clockReceiver, List<float> valueList) : base(clock,clockReceiver, valueList){}
    override protected void _setValue(float val){}
    override protected void _measure(){}
}


}