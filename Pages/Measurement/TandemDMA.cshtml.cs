using Measurement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace volt.Pages.Measurement;

[BindProperties]
public class TandemDMAModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public TandemDMAModel(ILogger<IndexModel> logger)
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
        
        
        string tandemdiameterVector = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.TandemDMAVector);
        TandemDiameterVector = tandemdiameterVector.Split(";");
    }

    public void OnGet()
    {
        Console.WriteLine("OnGet(): TandemDMA Measurement!");
        
    }

    public void OnPost(){


        Logger.WriteToLog("SMPSMeasurement.cshtml.cs: OnPost(): SMPSMeasurement");
        Logger.WriteToLog("SMPSMeasurement.cshtml.cs: OnPost(): Checking for inputs...");

        if(string.IsNullOrEmpty(Name)){

            Logger.WriteToLog($"SMPSMeasurement.cshtml.cs: OnPost(): Property 'Name' is empty!");
            return;
        }

        if(string.IsNullOrEmpty(MeasurementSeries)){

            Logger.WriteToLog($"SMPSMeasurement.cshtml.cs: OnPost(): Property 'MeasurementSeries' is empty!");
            return;
        }

        if(string.IsNullOrEmpty(SheathFlow)){

            Logger.WriteToLog($"SMPSMeasurement.cshtml.cs: OnPost(): Property 'Sheathflow' is empty!");
        }

        if(string.IsNullOrEmpty(UpscanTime)){

            Logger.WriteToLog($"SMPSMeasurement.cshtml.cs: OnPost(): Property 'UpscanTime' is empty!");
            return;
        }

        if(string.IsNullOrEmpty(DownscanTime)){

            Logger.WriteToLog($"SMPSMeasurement.cshtml.cs: OnPost(): Property 'DownscanTime' is empty!");
            return;
        }

        if(string.IsNullOrEmpty(SMPSMinDiameter)){

            Logger.WriteToLog($"SMPSMeasurement.cshtml.cs: OnPost(): Property 'SMPSMinDiameter' is empty!");
            return;
        }

        if(string.IsNullOrEmpty(SMPSMaxDiameter)){

            Logger.WriteToLog($"SMPSMeasurement.cshtml.cs: OnPost(): Property 'SMPSMaxDiameter' is empty!");
            return;
        }

        if(string.IsNullOrEmpty(SMPSDMAType)){

            Logger.WriteToLog($"SMPSMeasurement.cshtml.cs: OnPost(): Property 'SMPSDMAType' is empty!");
            return;

        }

        
        
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.SheathFlow, SheathFlow);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.UpscanTime, UpscanTime);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.DownscanTime, DownscanTime);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.SMPSMinDiameter, SMPSMinDiameter);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.SMPSMaxDiameter, SMPSMaxDiameter);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.SMPSDMAType, SMPSDMAType);
//        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.TandemDMAMinDiameter, )

        MeasurementController.Instance.MeasurementType = EMeasurementType.TandemDMA;
        Task.Run(async () => {await MeasurementController.Instance.StartMeasurement();});

        Console.WriteLine("OnPost(): TandemDMA Measurementlol!");
    }

    public void OnPostStart(){

        Console.WriteLine("Starting TandemDMA Measurement!");
        DeviceController devicecontroller = DeviceController.Instance;
        var test = Request.Form["Measurement Type"];
        Logger.WriteToLog($"{test}");
        
        MeasurementController.Instance.MeasurementType = EMeasurementType.TandemDMA;
        devicecontroller.InitializeDevices();
    }

    public string? Name {get; set;}
    public string? MeasurementSeries {get; set;}
    public string? SheathFlow{get; set;}
    public string? DownscanTime {get; set;}
    public string? UpscanTime{get; set;}
    public string? SMPSMinDiameter {get; set;}
    public string? SMPSMaxDiameter {get;set;}
    public string? SMPSDMAType {get; set;}
    public static string[]? SMPSDiameterVector {get; set;}
    public static string []? TandemDiameterVector {get; set;}

}
