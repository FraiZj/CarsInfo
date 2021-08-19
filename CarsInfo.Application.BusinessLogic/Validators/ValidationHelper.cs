using CarsInfo.Application.BusinessLogic.Exceptions;

namespace CarsInfo.Application.BusinessLogic.Validators
{
    public static class ValidationHelper
    {
        public static void ThrowIfNull(object obj)
        {
            if (obj is null)
            {
                throw new BllException("Argument can't be null");
            }
        }
        
        public static void ThrowIfStringNullOrWhiteSpace(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new BllException("Argument can't be null or whitespace");
            }
        }
    }
}
