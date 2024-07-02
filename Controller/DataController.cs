using System.IO;
using Model.DatabaseModel;

public class DataController{

    private static DataController _instance;

    private DataController(){


    }

    public static DataController Instance {
        get{

            if(_instance == null){
                _instance = new DataController();
            }
            return _instance;
        }
    }

    public string StorageDirectory{get; set;}
    private ICRUD _database;   



    
}