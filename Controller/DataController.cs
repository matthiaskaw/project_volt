using System.IO;
using Microsoft.EntityFrameworkCore;
using Model.DatabaseModel;


public enum EDatabaseEntity{

    Experiment = 0,
    Sample = 1,
    Dataset = 2

}


public class DataController{

    private static DataController _instance;
    private SimpleDbContext _dbContext;
    private DataController(){

        
        string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
        Logger.WriteToLog($"DataController: applicationPath = {applicationPath}");
        _storageDirectory = System.IO.Path.Combine(applicationPath, "data");
        Logger.WriteToLog($"DataController: data path = {_storageDirectory}");
        System.IO.Directory.CreateDirectory(_storageDirectory);

        _dbContext = new SimpleDbContext(_storageDirectory);
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
     

    public void TakeDataBaseEntity(Sample sample ){

        
        _dbContext.Samples.Add(sample);
        _dbContext.SaveChanges();

    } 

    public void TakeDataBaseEntity(Experiment experiment){

        _dbContext.Experiments.Add(experiment);
        _dbContext.SaveChanges();
        
    }

    public void TakeDataBaseEntity(Dataset dataset){


        _dbContext.Datasets.Add(dataset);
        _dbContext.SaveChanges();

    }





    
}