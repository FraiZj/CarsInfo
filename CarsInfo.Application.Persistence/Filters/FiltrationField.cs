namespace CarsInfo.Application.Persistence.Filters
{
    public class FiltrationField
    {
        public FiltrationField(
            string field,
            int value,
            string @operator = "=",
            string separator = "")
        {
            Field = field;
            Value = value;
            Operator = @operator;
            Separator = separator;
        }

        public FiltrationField(
            string field,
            object value,
            string @operator = "=",
            string separator = "")
        {
            Field = field;
            Value = value;
            Operator = @operator;
            Separator = separator;
        }

        public string Field { get; set; }

        public object Value { get; set; }

        public string Operator { get; set; }

        public string Separator { get; set; }
    }
}