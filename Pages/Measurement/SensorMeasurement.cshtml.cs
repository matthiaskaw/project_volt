using Measurement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using DatabaseModel;
using Services;

namespace volt.Pages.Measurement;

[BindProperties]
public class SensorMeasurementModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly WebSocketController _websocketcontroller;

    public string Name { get; set; } ="";
    public DateTime Date {get; set;} = DateTime.Now;
    public string Description { get; set;} = "";
    public string SelectedSampleID {get; set;}
    public string SelectedExperimentID {get; set;} = "";
    public List<SelectListItem> SampleOptions {get; set;} = DataController.Instance.DbContext.Samples.Select(sample => new SelectListItem{

            Value= sample.UUID.ToString(),
            Text= sample.Name
        }).ToList();
    public List<SelectListItem> Experiments {get; set;}
    public static string[]? MeasurementTimeVector {get;set;}
    public static string? SensorOne {get; set;}
    public static string? SensorTwo {get; set;}


    public SensorMeasurementModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
       

      
        
        MeasurementTimeVector = SettingsService.Instance.MeasurementSetting.GetSettingByKey(EMeasurementSettings.CurrentReadingTime).Split(";");
        SensorOne = "n.a.";
        SensorTwo = "n.a.";

        SampleOptions = DataController.Instance.DbContext.Samples.Select(sample => new SelectListItem{

            Value= sample.UUID.ToString(),
            Text= sample.Name
        }).ToList();
         
    }

    public void OnGet()
    {
           

        
        
    }


    public void OnPostStart(){


        MeasurementController.Instance.MeasurementType = EMeasurementType.CurrentMeasurement;

        MeasurementController.Instance.CurrentDataset = new Dataset();
        MeasurementController.Instance.CurrentDataset.UUID = Guid.NewGuid();
        MeasurementController.Instance.CurrentDataset.Name = Name;
        MeasurementController.Instance.CurrentDataset.Date = DateTime.Now;
        MeasurementController.Instance.CurrentDataset.Description = Description;
        MeasurementController.Instance.CurrentDataset.Experiment = DataController.Instance.DbContext.Experiments.FirstOrDefault(
            e => e.Name == DataController.Instance.DbContext.ExperimentNames.GetValueOrDefault(EMeasurementType.SMPS));
        MeasurementController.Instance.CurrentDataset.ExperimentID =  MeasurementController.Instance.CurrentDataset.Experiment.UUID;
        MeasurementController.Instance.CurrentDataset.Sample =  DataController.Instance.DbContext.Samples.FirstOrDefault(s => s.UUID.ToString() == SelectedSampleID);
        MeasurementController.Instance.CurrentDataset.SampleID = MeasurementController.Instance.CurrentDataset.Sample.UUID;
        
        Task.Run(async () => {await MeasurementController.Instance.StartMeasurement();});
        
    }

    public void OnPostStop(){

        MeasurementController.Instance.StopMeasurement();
        
    }

    public void OnPostCancel(){

        MeasurementController.Instance.Cancel();
    }
}
