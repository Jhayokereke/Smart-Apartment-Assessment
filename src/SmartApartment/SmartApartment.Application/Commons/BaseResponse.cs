using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartment.Application.Commons
{
    public abstract class BaseResponse<T> : ApiResponse where T : class
    {
        public BaseResponse(string message = "", bool success = false) : base(message, success)
        {
        }
        public T Data { get; set; }
    }
}
