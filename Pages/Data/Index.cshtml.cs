using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using Measurement;
namespace volt.Pages.Data{


[BindProperties]
public class IndexModel : PageModel
{

    private readonly ILogger<IndexModel> _logger;
    public string TargetPage {get; set;}
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
       
    }

    public IActionResult OnPost(){
        
        if(!string.IsNullOrEmpty(TargetPage)){
            return Redirect(TargetPage);
        }

        return Page();
       

       

    }
    
        

}
}

