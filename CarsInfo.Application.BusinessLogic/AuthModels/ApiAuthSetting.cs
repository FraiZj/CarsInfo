namespace CarsInfo.Application.BusinessLogic.AuthModels
{
    public class ApiAuthSetting
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public int ExpirationTime { get; set; }
    }
}
