namespace DatabaseModel{


    public class Experiment{


        public Guid UUID {get; set;} = Guid.NewGuid();
        public string Name {get; set;} = "";
        public string Description {get; set;} = "";
        
    }
}