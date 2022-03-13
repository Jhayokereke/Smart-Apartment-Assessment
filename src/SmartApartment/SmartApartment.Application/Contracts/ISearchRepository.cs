using SmartApartment.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartment.Application.Contracts
{
    public interface ISearchRepository
    {
        Task<SearchResponse> Search(SearchRequest request);
    }
}
