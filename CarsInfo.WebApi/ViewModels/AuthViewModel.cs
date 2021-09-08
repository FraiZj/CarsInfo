using System.ComponentModel.DataAnnotations;

namespace CarsInfo.WebApi.ViewModels
{
    public class AuthViewModel
    {
        public AuthViewModel(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
        
        [Required]
        public string AccessToken { get; set; }
        
        [Required]
        public string RefreshToken { get; set; }
    }
}
