using System.Collections.Generic;

namespace SmartApartment.Application.Commons
{
    public class ApiResponse
    {
        public ApiResponse(string message = "", bool success = false)
        {
            Message = message;
            Success = success;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> ValidationErrors { get; set; }
    }
}
