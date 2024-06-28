using Measurement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace volt.Pages.Measurement;

[BindProperties]
public class SMPSMeasurementModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public SMPSMeasurementModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        
        Name="";
        MeasurementSeries ="";
        SheathFlow = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SheathFlow);
        DownscanTime = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.DownscanTime);
        UpscanTime = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.UpscanTime);
        string temp = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSDiameterVector);
        SMPSDiameterVector = temp.Split(";");
        SMPSMinDiameter = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSMinDiameter);
        SMPSMaxDiameter = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSMaxDiameter);
        SMPSDMAType = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSDMAType);

        
    
    }

    public void OnGet()
    {
        Console.WriteLine("OnGet(): SMPSMeasurement");
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

        Console.WriteLine($"SMPSMeasurement.cshtml.cs: OnPost(): Starting SMPS Measurement...");
        

        MeasurementController.Instance.MeasurementType = EMeasurementType.SMPS;
        Task.Run(async () => {await MeasurementController.Instance.StartMeasurement();});
            


    }

    public void OnPostStart(){


       
    
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
    


}
