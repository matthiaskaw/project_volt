using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using Measurement;
using DatabaseModel;
using Microsoft.IdentityModel.Tokens;
namespace volt.Pages.Data{


[BindProperties]
public class ExperimentModel : PageModel
{
    
    public string Name { get; set; } ="";
    public string Description { get; set; } = "";

    private readonly ILogger<IndexModel> _logger;

    public ExperimentModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
       
    }

    public void OnGet()
    {
        Logger.WriteToLog("OnGet Experiment");
    }

    public void OnPost(){  

        Experiment experiment = new Experiment();
        experiment.Name = Name;
        experiment.Description = Description;
        DataController.Instance.DbContext.Experiments.Add(experiment);
        DataController.Instance.DbContext.SaveChanges();

    }
        

}
}
