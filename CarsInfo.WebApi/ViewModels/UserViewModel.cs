using System.Collections.Generic;
using CarsInfo.Application.BusinessLogic.Dtos;

namespace CarsInfo.WebApi.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
