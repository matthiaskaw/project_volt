using Measurement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using DatabaseModel;

namespace volt.Pages.Measurement;
//asp-items="@(new SelectList(Model.SMPSDiameterVector))"
[BindProperties]
public class TandemTemperatureModel : PageModel
{


    private readonly ILogger<IndexModel> _logger;

    public TandemTemperatureModel(ILogger<IndexModel> logger)
    {
        _logger = logger;

        Name = "";
        SheathFlow = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SheathFlow);
        SMPSDMAType = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSDMAType);
        UpscanTime = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.UpscanTime);
        DownscanTime = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.DownscanTime);
        
        string smpsdmavector = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.SMPSDiameterVector);
        SMPSDiameterVector = smpsdmavector.Split(";");
        
        
        string temperaturevector = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.TandemTemperatureVector);
        TandemTemperatureCurrentVector = temperaturevector.Split(";");

        CurrentOptions = new SelectList(TandemTemperatureCurrentVector);

        SMPSDiameterOptions = new SelectList(SMPSDiameterVector);
        
        MaxDiameterSMPS = SMPSDiameterVector.Last();
        MaxCurrent = TandemTemperatureCurrentVector.Last();

    
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
        Console.WriteLine("OnGet(): TandemTemperature");

        
        //Set Dropdown current to first and last element for min current and max current
        //Implement Dropdown for SMPS diameter min and max
    }

    public void OnPost(){



        Console.WriteLine("OnPost(): TandemTemperature Measurement!");

        

        if(string.IsNullOrEmpty(Name)){
            return;
        }
        if(string.IsNullOrEmpty(SheathFlow)){
            return;
        }
        if(string.IsNullOrEmpty(MinCurrent)){
            return;
        }
        if(string.IsNullOrEmpty(MaxCurrent)){
            return;
        }

        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.SheathFlow, SheathFlow);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.UpscanTime, UpscanTime);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.DownscanTime, DownscanTime);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.SMPSDMAType, SMPSDMAType);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.TandemTemperatureMinCurrent, MinCurrent);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.TandemTemperatureMaxCurrent, MaxCurrent);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.FirstDMADiameter, FirstDMADiameter);

        MeasurementController.Instance.CurrentDataset = new Dataset();
        MeasurementController.Instance.CurrentDataset.UUID = Guid.NewGuid();
        MeasurementController.Instance.CurrentDataset.Name = Name;
        MeasurementController.Instance.CurrentDataset.Date = DateTime.Now;
        MeasurementController.Instance.CurrentDataset.Description = Description;
        MeasurementController.Instance.CurrentDataset.Experiment = DataController.Instance.DbContext.Experiments.FirstOrDefault(e => e.UUID.ToString() == SelectedExperimentID);;
        MeasurementController.Instance.CurrentDataset.Sample =  DataController.Instance.DbContext.Samples.FirstOrDefault(s => s.UUID.ToString() == SelectedSampleID);

        

        MeasurementController.Instance.MeasurementType = EMeasurementType.TandemTemperature;
        Task.Run(async () => {await MeasurementController.Instance.StartMeasurement();});
    }



    //PROPERTIES
    public string? Name {get; set;}
    public string? Description {get; set;}
    public string SelectedExperimentID { get; set; }
    public string SelectedSampleID { get; set;}
    public DateTime Date { get; set; } = DateTime.Now;
    public List<SelectListItem> ExperimentOptions {get; set;}
    public List<SelectListItem> SampleOptions {get; set;}
    public string? SheathFlow {get; set;}
    public string? UpscanTime {get; set;}
    public string? DownscanTime {get; set;}
    public static string[]? SMPSDiameterVector {get; set;}
    public static string[]? TandemTemperatureCurrentVector {get; set;}
    public string? SMPSDMAType {get; set;}
    
    public string MinDiameterSMPS {get; set;}
    public string MaxDiameterSMPS {get; set;}
    public string MinCurrent {get; set;}
    public string MaxCurrent{get; set;}
    public string FirstDMADiameter {get; set;} = "20";
    public SelectList SMPSDiameterOptions { get; set; }
    public SelectList CurrentOptions { get; set; }
    //PRIVATE METHODS

    //PRIVATE VARIABLES
}
