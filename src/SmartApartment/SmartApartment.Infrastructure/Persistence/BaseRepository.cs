using Nest;
using SmartApartment.Application.Commons;
using SmartApartment.Application.Contracts;
using SmartApartment.Domain;
using SmartApartment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartment.Infrastructure.Persistence
{
    public class BaseRepository : IBaseRepository
    {
        protected readonly IElasticClient _client;

        protected string Index { get; }
        public BaseRepository(IElasticClient client)
        {
            _client = client;
            Index = Constants.SmartApartment;
        }
        public async Task<bool> Add<T>(string index, T data) where T : class
        {
            if (!await IndexExists(index))
                await CreateIndex(index);

            var response = await _client.IndexAsync(data, x => x.Index(index));
            return response.IsValid;
        }

        public async Task<bool> BulkAddAsync<T>(string index, ICollection<T> data) where T : class
        {
            if (!await IndexExists(index))
                await CreateIndex(index);

            var response = await _client.IndexManyAsync(data, index);
            return response.IsValid;
        }

        public async Task<bool> CreateIndex(string index)
        {
            var createIndexResponse = await _client.Indices.CreateAsync(index, c => c
                .Settings(s => s
                    .Analysis(d => d
                 .Analyzers(an => an
                         .Custom(Constants.AutocompleteAnalyzer, a => a
                                     .Tokenizer("standard")
                                     .Filters("lowercase", "stop", "trim", "autocomplete")
                                 )
                         .Custom(Constants.KeywordAnalyzer, a => a
                                     .Tokenizer("keyword")
                                     .Filters("lowercase", "trim")
                                 )
                         .Custom(Constants.SearchAnalyzer, a => a
                                     .Tokenizer("lowercase")
                                     .Filters("trim")
                                 )
                        )
                 .Tokenizers(t => t
                         .EdgeNGram("autocomplete", g => g
                                     .MinGram(2)
                                     .MaxGram(25)
                                )
                        )
                 .TokenFilters(f => f
                        .EdgeNGram("autocomplete", g => g
                                     .MinGram(2)
                                     .MaxGram(15)
                                )
                )))
                .Aliases(s => s.Alias(Constants.SmartApartment))
                .Map<Management>(m => m
                    .AutoMap()
                    .Properties(p => p
                        .Text(t => t
                            .Name(n => n.Name)
                            .Analyzer(Constants.AutocompleteAnalyzer)
                            .Fields(t => t
                                .Text(t => t
                                    .Name("exact")
                                    .Analyzer(Constants.KeywordAnalyzer)))
                            .SearchAnalyzer(Constants.SearchAnalyzer))
                        .Text(t => t
                            .Name("exact")
                            .Analyzer(Constants.KeywordAnalyzer))
                         .Keyword(k => k
                            .Name(n => n.Market))
                        )
                    )
                .Map<Property>(p => p
                    .AutoMap()
                    .Properties(p => p
                        .Text(t => t
                            .Name(n => n.Name)
                            .Analyzer(Constants.AutocompleteAnalyzer)
                            .Fields(t => t
                                .Text(t => t
                                    .Name("exact")
                                    .Analyzer(Constants.KeywordAnalyzer)))
                            .SearchAnalyzer(Constants.SearchAnalyzer))
                        .Text(t => t
                            .Name(n => n.FormerName)
                            .Analyzer(Constants.AutocompleteAnalyzer)
                            .SearchAnalyzer(Constants.SearchAnalyzer))
                        .Text(t => t
                            .Name("exact")
                            .Analyzer(Constants.KeywordAnalyzer))
                         .Keyword(k => k
                            .Name(n => n.Market))
                        )
                    )
                );

            return createIndexResponse.IsValid;
        }

        public async Task<bool> IndexExists(string index)
        {
            var response = await _client.Indices.ExistsAsync(index);
            return response.Exists;
        }
    }
}
