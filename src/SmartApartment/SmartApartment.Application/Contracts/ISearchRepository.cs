using SmartApartment.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartment.Application.Contracts
{
    public interface ISearchRepository<T> where T : class
    {
        Task<bool> BulkAddAsync(string index, IEnumerable<T> data);
        Task<SearchResult> Search(SearchRequest request);
    }
}
