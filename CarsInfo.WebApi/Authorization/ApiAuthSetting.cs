namespace CarsInfo.WebApi.Authorization
{
    public class ApiAuthSetting
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public int ExpirationTime { get; set; }
    }
}
