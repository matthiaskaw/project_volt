using Measurement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
namespace volt.Pages.Measurement;

[BindProperties]
public class SensorMeasurementModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly WebSocketController _websocketcontroller;


    public SensorMeasurementModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
       
        MeasurementTimeVector = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.CurrentReadingTime).Split(";");
        SensorOne = "n.a.";
        SensorTwo = "n.a.";
    }

    public void OnGet()
    {
        
        
        
    }

    public void OnPost(){

        
        
    }

    public void OnPostStart(){

        Logger.WriteToLog("SensorMeasurement.cshtml.cs: OnPost(): SensorMeasurement");
        MeasurementController.Instance.MeasurementType = EMeasurementType.CurrentMeasurement;
        //DeviceController.Instance.InitializeDevices();
        
    }

    public void OnPostAbortClicked(){

        Logger.WriteToLog("Aborting Current measurement");
    }

    public static string[]? MeasurementTimeVector {get;set;}
    public static string? SensorOne {get; set;}
    public static string? SensorTwo {get; set;}

}
