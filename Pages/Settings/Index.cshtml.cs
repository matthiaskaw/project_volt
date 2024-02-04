using Measurement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace volt.Pages;

    
public class SettingsModell : PageModel
{
    public enum EBaudRate {
        Baudrate115200 = 115200,
        Baudrate9600 = 9600
    }
    enum ESettingsType {

        Default = 0,
        General = 1,
        Device = 2,

    }


    [BindProperty]
    public List<SelectListItem> BaudrateItems{get; set;}


    [BindProperty]
    public EBaudRate SelectedBaudrate {get;set;}

    private readonly ILogger<IndexModel> _logger;

    /*public SettingsModell(ILogger<IndexModel> logger)
    {
        _logger = logger;

    }*/


public string DeviceName {get; set;}
public string DeviceType {get; set;}
    public SettingsModell(){
        
DeviceName="Classifier";
DeviceType="3082";
    }
  

    public void OnGet()
    {
        
        DeviceName = "Classifier";
        DeviceType = "3082";
        
        SelectedBaudrate = EBaudRate.Baudrate115200;
        // Convert enum values to SelectListItem objects
        BaudrateItems = Enum.GetValues(typeof(EBaudRate))
                    .Cast<EBaudRate>()
                    .Select(e => new SelectListItem { Value = e.ToString(), Text = e.ToString() })
                    .ToList();


        if(BaudrateItems == null){

            Logger.WriteToLog("Index Settings: Baudrate is null");
        }

        
    

    }

    public void OnPost(){


    }

    public void OnPostStart(){

        Logger.WriteToLog($"Settings/Index: Current setting type = {_settingstype}");
        
    }

    public IActionResult OnPostDeviceSettingsClicked(){
        
        _settingstype = ESettingsType.Device;
        return Partial("_Device");
        
    }
    public IActionResult OnPostGeneralSettingsClicked(){

        _settingstype = ESettingsType.General;
        return Partial("Index");
    }
    private ESettingsType _settingstype = ESettingsType.Default; 
}
