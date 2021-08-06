namespace CarsInfo.DAL.Assistance
{
    public class JoinModel
    {
        public JoinModel(string foreignTable, string foreignField, string primaryField = "Id")
        {
            ForeignTable = foreignTable;
            ForeignField = foreignField;
            PrimaryField = primaryField;
        }

        public string ForeignTable { get; set; }
        
        public string ForeignField { get; set; }

        public string PrimaryField { get; set; }
    }
}
