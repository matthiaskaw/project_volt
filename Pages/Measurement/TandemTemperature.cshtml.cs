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
        
        
        string[] temperaturevector = SettingsService.
                                        Instance.
                                        MeasurementSetting.
                                        GetSettingByKey(EMeasurementSettings.TandemTemperatureVector).Split(";");

        foreach(string s in temperaturevector){
            CurrentOptions.Add(new SelectListItem(s,s));
        }
        
        string[] smpsdiametervector = SettingsService.
                                            Instance.
                                            MeasurementSetting.
                                            GetSettingByKey(EMeasurementSettings.SMPSDiameterVector).Split(";");

        foreach (string s in smpsdiametervector){
            SMPSDiameterOptions.Add(new SelectListItem(s,s));
        }

        string[] currentStepVector = SettingsService.
                                        Instance.
                                        MeasurementSetting.
                                        GetSettingByKey(EMeasurementSettings.TandemTemperatureStep).Split(";");
        
        foreach(string s in currentStepVector){

            CurrentStepsOption.Add(new SelectListItem(s,s));
        }

        MaxDiameterSMPS = SMPSDiameterVector.Last();
        MaxCurrent = temperaturevector.Last();

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
        
        MeasurementController.Instance.MeasurementType = EMeasurementType.TandemTemperature;

        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.SheathFlow, SheathFlow);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.UpscanTime, UpscanTime);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.DownscanTime, DownscanTime);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.SMPSDMAType, SMPSDMAType);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.TandemTemperatureMinCurrent, MinCurrent);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.TandemTemperatureMaxCurrent, MaxCurrent);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.FirstDMADiameter, FirstDMADiameter);
        SettingsService.Instance.MeasurementSetting.SetSettingByKey(EMeasurementSettings.TandemTemperatureStep, CurrentStep);

        MeasurementController.Instance.CurrentDataset = new Dataset();
        MeasurementController.Instance.CurrentDataset.UUID = Guid.NewGuid();
        MeasurementController.Instance.CurrentDataset.Name = Name;
        MeasurementController.Instance.CurrentDataset.Date = DateTime.Now;
        MeasurementController.Instance.CurrentDataset.Description = Description;
        MeasurementController.Instance.CurrentDataset.Experiment = DataController.Instance.DbContext.Experiments.FirstOrDefault(e => e.UUID.ToString() == SelectedExperimentID);;
        MeasurementController.Instance.CurrentDataset.Sample =  DataController.Instance.DbContext.Samples.FirstOrDefault(s => s.UUID.ToString() == SelectedSampleID);

        
        try{
            DeviceController.Instance.CheckNecessaryDevices();
        }
        catch(Exception ex){
            Logger.WriteToLog($"SMPSMeasurement.OnPost(): Necessary devices not initialized. {ex.Message}");
            return;
        }
       
        Task.Run(async () => {await MeasurementController.Instance.StartMeasurement();});
    }

    public void OnPostCancel(){

        MeasurementController.Instance.StopMeasurement();


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
    public string CurrentStep {get; set;}
    public string FirstDMADiameter {get; set;} = "20";
    public List<SelectListItem> SMPSDiameterOptions { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> CurrentOptions { get; set; } = new List<SelectListItem>();
    public List<SelectListItem> CurrentStepsOption {get; set; } = new List<SelectListItem>();
    //PRIVATE METHODS

    //PRIVATE VARIABLES
}
