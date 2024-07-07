using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DatabaseModel;
using Measurement;
using Microsoft.IdentityModel.Tokens;
namespace volt.Pages.Data{


[BindProperties]
public class DatasetModel : PageModel
{

    public string SelectedExperimentID { get; set; }
    public string SelectedSampleID { get; set;}
    public List<SelectListItem> ExperimentOptions {get; set;}
    public List<SelectListItem> SampleOptions {get; set;}
    public DateTime Date {get; set;}
    public string Name { get; set;}
    public string Description { get; set;}
    public string FilePath { get; set;}

    

    private readonly ILogger<IndexModel> _logger;

    public DatasetModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {


        /*
        ExperimentOptions = DataController.Instance.DbContext.Experiments.Select(exp => new SelectListItem{

            Value = exp.UUID.ToString(),
            Text = exp.Name
        }).ToList();

        SampleOptions = DataController.Instance.DbContext.Samples.Select(sample => new SelectListItem{

            Value= sample.UUID.ToString(),
            Text= sample.Name
        }).ToList();

    */

       
    }


    public void OnPost(IFormFile fileInput){  

        /*Experiment selectedExperiment = DataController.Instance.DbContext.Experiments.FirstOrDefault(e => e.UUID.ToString() == SelectedExperimentID);
        //Sample selectedSample = DataController.Instance.DbContext.Samples.FirstOrDefault(s => s.UUID.ToString() == SelectedSampleID);
        //FilePath = fileInput.FileName;
        //if(selectedExperiment == null){ throw new Exception("Dataset.cshtml.cs: Fail OnPost: selectedExperiment is null!");}
        //if(selectedSample == null){throw new Exception("Dataset.cshtml.cs: Fail OnPost: selectedSample is null!");}
        
        Dataset newDataset = new Dataset();
        newDataset.Experiment = selectedExperiment;
        newDataset.ExperimentID = selectedExperiment.UUID;
        newDataset.Sample = selectedSample;
        newDataset.SampleID = selectedSample.UUID;
        newDataset.Date = Date;
        newDataset.Name = Name;
        newDataset.Description = Description;
        newDataset.Path = FilePath;
    
        DataController.Instance.DbContext.Datasets.Add(newDataset);
        DataController.Instance.DbContext.SaveChanges();
        */
    }


        

}
}
