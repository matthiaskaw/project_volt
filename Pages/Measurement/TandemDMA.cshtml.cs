using Measurement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace volt.Pages.Measurement;

public class TandemDMA : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public TandemDMA(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        Console.WriteLine("OnGet(): TandemDMA Measurement!");
        DeviceController devicecontroller = DeviceController.Instance;
        devicecontroller.InitializeDevices();
    }

    public void OnPost(){

        Console.WriteLine("OnPost(): TandemDMA Measurementlol!");
    }

    public void OnPostStart(){

        Console.WriteLine("Starting TandemDMA Measurement!");
        DeviceController devicecontroller = DeviceController.Instance;
        var test = Request.Form["Measurement Type"];
        Logger.WriteToLog($"{test}");
        
        MeasurementController.Instance.MeasurementType = EMeasurementType.TandemDMA;
        devicecontroller.InitializeDevices();
    }

}
