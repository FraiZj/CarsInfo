namespace CarsInfo.WebApi.ViewModels.Auth
{
    public class AuthViewModel
    {
        public AuthViewModel(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
