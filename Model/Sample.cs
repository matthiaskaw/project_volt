namespace Model.DatabaseModel{


    public class Sample{

        public Sample(){}
        public Sample(string name, string labID, string description, DateTime date){

            Name = name;
            LabID = labID;
            Description = description;
            Date = date;
        }
        public Guid UUID { get; set; } = new Guid();
        public string Name { get; set; } = "";
        public DateTime Date { get; set; }
        public string LabID {get; set;} = "";
        public string Description { get; set; } = "";
    }


}