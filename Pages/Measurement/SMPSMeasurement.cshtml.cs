using Measurement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace volt.Pages.Measurement;

public class SMPSMeasurement : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public SMPSMeasurement(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        Console.WriteLine("HELLO WORLD !!!!!!!!!!!!!!!!!!!!!!!!!!!!!****************!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }

    public void OnPost(){

        Console.WriteLine("HELLO WORLD !!!!!!!!!!!!!!!!!!!!!!!!!!!!!****************!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }

    public void OnPostStart(){

        Console.WriteLine("Starting SMPS Measurement!");
        DeviceController devicecontroller = DeviceController.Instance;
        
        devicecontroller.MeasurementType = EMeasurementType.SMPS;
        devicecontroller.InitializeDevices();
    }

}
