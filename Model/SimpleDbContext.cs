
using System.Data;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace Model.DatabaseModel{


    public class SimpleDbContext : DbContext {

        public string DataDirectory {get; set;} = "";
        public IList<Sample> Samples{ get; set; }
        public IList<Experiment> Experiments{ get; set;}
        public IList<Dataset> Datasets {get; set;}

        private string _samplesJSONPath;
        private string _datasetsJSONPath;
        private string _experimentsJSONPath;

        public SimpleDbContext(string directory){

            DataDirectory = directory;
            _samplesJSONPath = Path.Combine(DataDirectory, "samples.json");
            _datasetsJSONPath = Path.Combine(DataDirectory, "datasets.json");
            _experimentsJSONPath = Path.Combine(DataDirectory, "experiments.json");

    
            if(!System.IO.File.Exists(_samplesJSONPath)){ System.IO.File.Create(_samplesJSONPath); }
            if(!System.IO.File.Exists(_datasetsJSONPath)){ System.IO.File.Create(_datasetsJSONPath); }
            if(!System.IO.File.Exists(_experimentsJSONPath)){ System.IO.File.Create(_experimentsJSONPath); }
            
        }
     
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("InMemoryDB");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){

            modelBuilder.Entity<Sample>().HasKey(c => c.UUID);
            modelBuilder.Entity<Experiment>().HasKey(c => c.UUID);
            modelBuilder.Entity<Dataset>().HasKey(c => c.UUID);
        }


        public override int SaveChanges()
        {

            var samplesJSON = JsonConvert.SerializeObject(Samples);
            var datasetsJSON = JsonConvert.SerializeObject(Datasets);
            var ExperimentsJSON = JsonConvert.SerializeObject(Experiments);

            File.WriteAllText(_samplesJSONPath, samplesJSON);
            File.WriteAllText(_datasetsJSONPath, datasetsJSON);
            File.WriteAllText(_experimentsJSONPath, ExperimentsJSON);

            return base.SaveChanges();
        
        }

        public void LoadData(){
            
            if(!File.Exists(_samplesJSONPath)){ return; }
            if(!File.Exists(_datasetsJSONPath)){ return;}
            if(!File.Exists(_experimentsJSONPath)){ return; }

            var samplesJSON = File.ReadAllText(_samplesJSONPath);
            var samples = JsonConvert.DeserializeObject<List<Sample>>(samplesJSON);
            Samples = samples ?? new List<Sample>();

            var datasetsJSON = File.ReadAllText(_datasetsJSONPath);
            var datasets = JsonConvert.DeserializeObject<List<Dataset>>(datasetsJSON);
            Datasets = datasets ?? new List<Dataset>();

            var experimentsJSON = File.ReadAllText(_experimentsJSONPath);
            var experiments = JsonConvert.DeserializeObject<List<Experiment>>(experimentsJSON);
            Experiments = experiments ?? new List<Experiment>();

        }
    }
}