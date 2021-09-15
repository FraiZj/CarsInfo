using System.Collections.Generic;
using System.Linq;

namespace CarsInfo.WebApi.ViewModels.Error
{
    public class ErrorResponse
    {
        public ErrorResponse() { }

        public ErrorResponse(string applicationError)
        {
            ApplicationError = applicationError;
        }

        public ErrorResponse(IEnumerable<ErrorModel> errors)
        {
            ValidationErrors = errors.ToList();
        }

        public string ApplicationError { get; set; }

        public IList<ErrorModel> ValidationErrors { get; set; }
    }
}
