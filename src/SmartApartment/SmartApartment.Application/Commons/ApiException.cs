using System;

namespace SmartApartment.Application.Commons
{
    public class ApiException : Exception
    {
        public ApiException(string message) : base(message)
        {
        }
    }
}
