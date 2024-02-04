using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using Measurement;
namespace volt.Pages.Measurement{



public class IndexModel : PageModel
{

    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    

     // Your property to hold the selected value
    [BindProperty]
    public EMeasurementType SelectedValue { get; set; }

    
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
        var selectedValue = Request.Form["SelectedValue"];

    // Convert enum values to SelectListItem objects
        var items = Enum.GetValues(typeof(EMeasurementType))
                    .Cast<EMeasurementType>()
                    .Select(e => new SelectListItem { Value = e.ToString(), Text = e.ToString() })
                    .ToList();

    // Find the SelectListItem for the selected value
    var selectedItem = items.FirstOrDefault(item => item.Value == selectedValue);

    if (selectedItem == null)
    {
        // Log or handle the case where the selected value is not found in the list
        return Page();
    }



        if(selectedValue.ToString() == EMeasurementType.SMPS.ToString()){

            Console.WriteLine($"Index: OnPost: SMPS Measurement ({EMeasurementType.SMPS.ToString()})");
            return RedirectToPage("./SMPSMeasurement");
    
        }

        if(selectedValue.ToString() == EMeasurementType.TandemTemperature.ToString()){
           
            Console.WriteLine($"Index: OnPost: TandemTemperature Measurement ({EMeasurementType.TandemTemperature.ToString()})");
            return RedirectToPage("./TandemTemperature");
        }

        if(selectedValue.ToString() == EMeasurementType.TandemDMA.ToString()){
            Console.WriteLine($"Index: OnPost: TandemDMA Measurement ({EMeasurementType.TandemDMA.ToString()})");
            return RedirectToPage("./TandemDMA");
        }
        // Redirect or return a different view as needed
        return RedirectToPage("Error");
    }
    }


}
