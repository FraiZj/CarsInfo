namespace CarsInfo.Application.BusinessLogic.Options
{
    public class ApiAuthOptions
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public int ExpirationTime { get; set; }
    }
}
