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

    

     // Your property to hold the selected value
    
    public EMeasurementType SelectedValue { get; set; }

    public string TargetPage {get; set;}
    // Your list of SelectListItem objects
    public List<SelectListItem> Items { get; set; }
    
    public void OnGet()
    {


        Items = Enum.GetValues(typeof(EMeasurementType))
                    .Cast<EMeasurementType>()
                    .Select(e => new SelectListItem { Value = e.ToString(), Text = e.ToString() })
                    .ToList();
        // Initialize your list of items
        

        // You can set the selected value if needed
        SelectedValue = EMeasurementType.TandemTemperature;
    }

    public IActionResult OnPost()
    {  
       if(!string.IsNullOrEmpty(TargetPage)){
            return Redirect(TargetPage);
        }

        return Page();
    }
    }


}
