using System.Collections.Generic;

namespace CarsInfo.WebApi.ViewModels.Users
{
    public class UserReadViewModel
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        
        public bool EmailVerified { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}