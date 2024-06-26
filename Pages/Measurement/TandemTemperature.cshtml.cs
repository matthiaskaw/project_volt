using Measurement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace volt.Pages.Measurement;

[BindProperties]
public class TandemTemperatureModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public TandemTemperatureModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    
        Name = "";
        MeasurementSeries= "";
        SheathFlow = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SheathFlow);
        SMPSDMAType = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSDMAType);
        UpscanTime = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.UpscanTime);
        DownscanTime = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.DownscanTime);
        
        string smpsdmavector = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSDiameterVector);
        SMPSDiameterVector = smpsdmavector.Split(";");
        
        
        string temperaturevector = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.TandemTemperatureVector);
        TandemTemperatureCurrentVector = temperaturevector.Split(";");
        
        
    }

    public void OnGet()
    {

        //Set Dropdown current to first and last element for min current and max current
        //Implement Dropdown for SMPS diameter min and max
    }

    public void OnPost(){



        Console.WriteLine("OnPost(): TandemTemperature Measurement!");

        
        string TandemTemperatureMinCurrent = Request.Form["MinCurrentDropDown"];
        string TandemTemperatureMaxCurrent = Request.Form["MaxCurrentDropDown"];


        if(string.IsNullOrEmpty(Name)){
            return;
        }
        if(string.IsNullOrEmpty(MeasurementSeries)){
            return;
        }
        if(string.IsNullOrEmpty(SheathFlow)){
            return;
        }
        if(string.IsNullOrEmpty(TandemTemperatureMinCurrent)){
            return;
        }
        if(string.IsNullOrEmpty(TandemTemperatureMaxCurrent)){
            return;
        }

        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.SheathFlow, SheathFlow);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.UpscanTime, UpscanTime);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.DownscanTime, DownscanTime);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.SMPSDMAType, SMPSDMAType);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.TandemTemperatureMinCurrent, TandemTemperatureMinCurrent);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.TandemTemperatureMaxCurrent, TandemTemperatureMaxCurrent);

        DeviceController devicecontroller = DeviceController.Instance;
        MeasurementController.Instance.MeasurementType = EMeasurementType.TandemTemperature;
        devicecontroller.InitializeDevices();
    }

    public void OnPostStart(){

        /*Console.WriteLine("Starting TandemDMA Measurement!");
        DeviceController devicecontroller = DeviceController.Instance;
        var test = Request.Form["Measurement Type"];
        Logger.WriteToLog($"{test}");
        
        devicecontroller.MeasurementType = EMeasurementType.TandemTemperature;
        devicecontroller.InitializeDevices();
        */
    }

    //PROPERTIES
    public string? Name {get; set;}
    public string? MeasurementSeries {get; set;}
    public string? SheathFlow {get; set;}
    public string? UpscanTime {get; set;}
    public string? DownscanTime {get; set;}
    public static string[]? SMPSDiameterVector {get; set;}
    public static string[]? TandemTemperatureCurrentVector {get; set;}
    public string? SMPSDMAType {get; set;}
    
    
    //PRIVATE METHODS

    //PRIVATE VARIABLES
}
