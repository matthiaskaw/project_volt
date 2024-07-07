using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace DatabaseModel{

    [Table("Dataset")]
    public class Dataset{
        
        public Guid UUID { get; set;} = Guid.NewGuid();
        public Guid ExperimentID {get; set;}
        public Guid SampleID { get; set;}
        public string Description { get; set;} ="";
        public string Path { get; set;} = "";
        public string Name {get; set;} = "";
        public DateTime Date {get; set;}
        

        //Navigation Properties
        public virtual Experiment Experiment{get; set;}
        public virtual Sample Sample{get; set;}
    }
}