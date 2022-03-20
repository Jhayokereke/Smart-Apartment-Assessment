using Nest;
using SmartApartment.Application.Commons;
using SmartApartment.Application.Contracts;
using SmartApartment.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApartment.Infrastructure.Persistence
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly IElasticClient _client;

        protected string Index { get; }
        public BaseRepository(IElasticClient client)
        {
            _client = client;
            Index = Constants.SmartApartment;
        }
        public async Task<bool> Add(string index, T data)
        {
            if (!await IndexExists(index))
                await CreateIndex(index);

            var response = await _client.IndexAsync(data, x => x.Index(index));
            return response.IsValid;
        }

        public async Task<bool> BulkAddAsync(string index, ICollection<T> data)
        {
            if (!await IndexExists(index))
                await CreateIndex(index);

            var response = await _client.IndexManyAsync(data, index);
            return response.IsValid;
        }

        public abstract Task<bool> CreateIndex(string index);

        public async Task<bool> IndexExists(string index)
        {
            var response = await _client.Indices.ExistsAsync(index);
            return response.Exists;
        }

        protected IAnalysis Analyzer(AnalysisDescriptor descriptor)
        {
            return descriptor
                 .Analyzers(an => an
                         .Custom(Constants.AutocompleteAnalyzer, a => a
                                     .Tokenizer("autocomplete")
                                     .Filters("lowercase", "stop", "eng_stopwords", "trim")
                                 )
                         .Custom(Constants.KeywordAnalyzer, a => a
                                     .Tokenizer("keyword")
                                     .Filters("lowercase", "eng_stopwords", "trim")
                                 )
                         .Custom(Constants.SearchAnalyzer, a => a
                                     .Tokenizer("lowercase")
                                     .Filters("eng_stopwords", "trim")
                                 )
                        )
                 .Tokenizers(t => t
                         .EdgeNGram("autocomplete", g => g
                                     .MinGram(2)
                                     .MaxGram(15)
                                     .TokenChars(TokenChar.Letter)
                                )
                        )
                 .TokenFilters(f => f
                         .Stop("eng_stopwords", s => s
                                    .StopWords("_english_")
                                )
                        );
        }
    }
}
