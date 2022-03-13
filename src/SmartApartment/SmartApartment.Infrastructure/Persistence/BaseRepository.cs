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
        private readonly IElasticClient _client;

        protected string Index { get; }
        public BaseRepository(IElasticClient client)
        {
            _client = client;
            Index = Constants.SmartApartment;
        }
        public async Task<bool> Add(string index, T data)
        {
            if (!await IndexExists(index))
                throw new ApiException("Index does not exist");

            var response = await _client.IndexAsync(data, x => x.Index(index));
            return response.IsValid;
        }

        public async Task<bool> BulkAddAsync(string index, ICollection<T> data)
        {
            if (!await IndexExists(index))
                throw new ApiException("Index does not exist");

            var response = await _client.IndexManyAsync(data, index);
            return response.IsValid;
        }

        public abstract Task<bool> CreateIndex(string index);

        public async Task<bool> IndexExists(string index)
        {
            var response = await _client.Indices.ExistsAsync(index);
            return response.Exists;
        }
    }
}
