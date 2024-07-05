using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using Measurement;
namespace volt.Pages.Measurement{


[BindProperties]
public class IndexModel : PageModel
{

    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {




       
    }

    public void OnPost(){  

    }
        

}
}
