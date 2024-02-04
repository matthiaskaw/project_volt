using Measurement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace volt.Pages;

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

    public void OnPostStart(){

        //Console.WriteLine("Starting SMPS Measurement!");
       // DeviceController devicecontroller = DeviceController.Instance;
        //devicecontroller.MeasurementType = EMeasurementType.SMPS;
        //devicecontroller.InitializeDevices();
    }
}
