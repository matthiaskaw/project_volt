using System.IO;
using Microsoft.EntityFrameworkCore;
using DatabaseModel;
using Services;


namespace DatabaseModel{
public enum EDatabaseEntity{

    Experiment = 0,
    Sample = 1,
    Dataset = 2

}


public class DataController{

    private static DataController _instance;
    public SimpleDbContext DbContext {get;}
    private DataController(){

        
        
        DbContext = new SimpleDbContext(SettingsService.Instance.DatabaseDirectory);

    }

    public static DataController Instance {
        get{

            if(_instance == null){
                _instance = new DataController();
            }
            return _instance;
        }
    }

    private string _storageDirectory{get; set;}
    
    public string SaveDataset(List<string> dataset, string name, string measurementType){ 
        
        string fileextension = ".txt";
        string directory = Path.Combine(SettingsService.Instance.MeasurementDirectory, DateTime.Now.ToString("yyyy-MM-dd"));
        Directory.CreateDirectory(directory);

        string filename = Path.Combine(
            directory,
            measurementType+"_"+
            name+"_"+
            DateTime.Now.ToString("yyyyMMdd_HHmmss")+
            fileextension
            );

        File.WriteAllLines(filename, dataset);
        
        return filename;
    
    }

    public void SaveDatasetMetaData(List<string> content, string name, string measurementType) {

        string metadatafileextension = ".med";
        
        string directory = Path.Combine(SettingsService.Instance.MeasurementDirectory, DateTime.Now.ToString("yyyy-MM-dd"));
        Directory.CreateDirectory(directory);

        string filename = Path.Combine(
            directory,
            measurementType+"_"+
            name+"_"+
            DateTime.Now.ToString("yyyyMMdd_HHmmss")+
            metadatafileextension
            );


       

        File.WriteAllLines(filename, content);




    }

    public Dataset CurrentDataSet {get; set;}

    





    
}
}