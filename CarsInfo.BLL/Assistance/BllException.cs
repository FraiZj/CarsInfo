using System;

namespace CarsInfo.BLL.Assistance
{
    public class BllException : Exception
    {
        public BllException()
        {
        }

        public BllException(string message)
            : base(message)
        {
        }

        public BllException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
