using Measurement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using DatabaseModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace volt.Pages.Measurement;

[BindProperties]
public class SMPSMeasurementModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public SMPSMeasurementModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        
        Name="";
        SheathFlow = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SheathFlow);
        DownscanTime = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.DownscanTime);
        UpscanTime = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.UpscanTime);
        string[] SMPSDiameterVector = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSDiameterVector).Split(";");
        SMPSMinDiameter = SMPSDiameterVector.First();
        SMPSMaxDiameter = SMPSDiameterVector.Last();
        
        
        SMPSDMAType = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSDMAType);

        

       
    
    }

    public void OnGet()
    {

         ExperimentOptions = DataController.Instance.DbContext.Experiments.Select(exp => new SelectListItem{

            Value = exp.UUID.ToString(),
            Text = exp.Name
        }).ToList();

        SampleOptions = DataController.Instance.DbContext.Samples.Select(sample => new SelectListItem{

            Value= sample.UUID.ToString(),
            Text= sample.Name
        }).ToList();
        Console.WriteLine("OnGet(): SMPSMeasurement");
    }

    public void OnPost(){

        Logger.WriteToLog("SMPSMeasurement.cshtml.cs: OnPost(): SMPSMeasurement");
        Logger.WriteToLog("SMPSMeasurement.cshtml.cs: OnPost(): Checking for inputs...");

        if(string.IsNullOrEmpty(Name)){

            Logger.WriteToLog($"SMPSMeasurement.cshtml.cs: OnPost(): Property 'Name' is empty!");
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
        MeasurementController.Instance.CurrentDataset = new Dataset();
        MeasurementController.Instance.CurrentDataset.UUID = Guid.NewGuid();
        MeasurementController.Instance.CurrentDataset.Name = Name;
        MeasurementController.Instance.CurrentDataset.Date = DateTime.Now;
        MeasurementController.Instance.CurrentDataset.Description = Description;
        MeasurementController.Instance.CurrentDataset.Experiment = DataController.Instance.DbContext.Experiments.FirstOrDefault(e => e.UUID.ToString() == SelectedExperimentID);;
        MeasurementController.Instance.CurrentDataset.ExperimentID =  MeasurementController.Instance.CurrentDataset.Experiment.UUID;
        MeasurementController.Instance.CurrentDataset.Sample =  DataController.Instance.DbContext.Samples.FirstOrDefault(s => s.UUID.ToString() == SelectedSampleID);
        MeasurementController.Instance.CurrentDataset.SampleID = MeasurementController.Instance.CurrentDataset.Sample.UUID;
        //DeviceController.Instance.CheckNecessaryDevices();
        Task.Run(async () => {await MeasurementController.Instance.StartMeasurement();});
            

    }


    public string? Name {get; set;}
    public string? Description {get; set;}
    public string SelectedExperimentID { get; set; }
    public string SelectedSampleID { get; set;}
    public DateTime Date { get; set; }
    public List<SelectListItem> ExperimentOptions {get; set;}
    public List<SelectListItem> SampleOptions {get; set;}
    public string? SheathFlow{get; set;}
    public string? DownscanTime {get; set;}
    public string? UpscanTime{get; set;}
    public string? SMPSMinDiameter {get; set;}
    public string? SMPSMaxDiameter {get;set;}
    public string? SMPSDMAType {get; set;}
    public SelectList SMPSDiameterOptions {get; set;} = new SelectList(SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSDiameterVector).Split(";")); 
    


}
