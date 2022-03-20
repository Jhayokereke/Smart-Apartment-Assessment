using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartment.Application.Contracts
{
    public interface IBaseRepository
    {
        Task<bool> BulkAddAsync<T>(string index, ICollection<T> data) where T : class;
        Task<bool> Add<T>(string index, T data) where T : class;
        Task<bool> IndexExists(string index);
        Task<bool> CreateIndex(string index);
    }
}
