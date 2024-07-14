using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DatabaseModel;
using Measurement;
namespace volt.Pages.Data{


[BindProperties]
public class SampleModel : PageModel
{

    public string Name { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public string LabID { get; set; }
    public string Description { get; set; } 
    private readonly ILogger<IndexModel> _logger;

    public SampleModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {




       
    }

    public void OnPost(){  

        Sample sample = new Sample(Name, LabID, Description, Date);
        DataController.Instance.DbContext.Samples.Add(sample);
        DataController.Instance.DbContext.SaveChanges();
        Logger.WriteToLog($"Onpost sample");
    }
        

}
}
