using Nest;
using SmartApartment.Application.Contracts;
using SmartApartment.Application.Dtos;
using SearchRequest = SmartApartment.Application.Dtos.SearchRequest;
using System.Threading.Tasks;
using SmartApartment.Domain.Models;
using SmartApartment.Domain;
using System.Linq;

namespace SmartApartment.Infrastructure.Persistence
{
    public class SearchRepository : ISearchRepository
    {
        private readonly IElasticClient _client;

        public SearchRepository(IElasticClient client)
        {
            _client = client;
        }
        public async Task<SearchResponse> Search(SearchRequest request)
        {
            int offset = (request.Pagenumber - 1) * request.Limit;
            var result = await _client.SearchAsync<object>(s => s
            .Index(Constants.SmartApartment)
            .Size(request.Limit)
            .Skip(offset)
            .Query(q => 
                q
                   .Bool(q => q
                       .Must(q => q
                           .MultiMatch(m => m
                               .Fields(f => f
                                   .Field(Infer.Field<PropertyObject>(p => p.Property.Name, 2.0))
                                   .Field(Infer.Field<PropertyObject>(p => p.Property.FormerName, 1.8))
                               )
                               .Fuzziness(Fuzziness.Auto)
                               .Operator(Operator.Or)
                               .Query(request.SearchPhrase)
                           )
                       )
                       .Filter(q => q
                          .Terms(t => t
                               .Field(Infer.Field<PropertyObject>(p => p.Property.Market.Suffix("keyword")))
                               .Terms(request.Markets)
                           )
                       )
                   )
                   ||
                   q
                   .Bool(q => q
                       .Must(q => q
                           .MultiMatch(m => m
                               .Fields(f => f
                                   .Field(Infer.Field<ManagementObject>(m => m.Management.Name, 1.7))
                               )
                               .Fuzziness(Fuzziness.Auto)
                               .Operator(Operator.Or)
                               .Query(request.SearchPhrase)
                           )
                       )
                       .Filter(q => q
                          .Terms(t => t
                               .Field(Infer.Field<ManagementObject>(m => m.Management.Market.Suffix("keyword")))
                               .Terms(request.Markets)
                           )
                       )
                   )

                )
            );

            return new SearchResponse
            {
                Message = "Successful",
                Success = true,
                Data = new SearchData
                {
                    Count = result.Documents.Count,
                    HasMatch = result.Documents.Count > 0,
                    Hits = result.Documents.ToArray()
                }
            };
            
        }
    }
}
