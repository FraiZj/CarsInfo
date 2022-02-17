using System.Collections.Generic;

namespace CarsInfo.Application.BusinessLogic.Dtos
{
    public class UserEditorDto
    {
        public ICollection<string> Roles { get; set; }
    }
}