using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartment.Application.Contracts
{
    public interface IBaseRepository<T> where T : class
    {
        Task<bool> BulkAddAsync(string index, ICollection<T> data);
        Task<bool> Add(string index, T data);
        Task<bool> IndexExists(string index);
        Task<bool> CreateIndex(string index);
    }
}
