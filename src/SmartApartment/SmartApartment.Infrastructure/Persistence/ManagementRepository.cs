﻿using Nest;
using SmartApartment.Application.Contracts;
using SmartApartment.Domain;
using SmartApartment.Domain.Models;
using System.Threading.Tasks;

namespace SmartApartment.Infrastructure.Persistence
{
    public class ManagementRepository : BaseRepository<ManagementObject>, IManagementRepository
    {
        public ManagementRepository(IElasticClient client) : base(client)
        {
        }
        public override async Task<bool> CreateIndex(string index)
        {
            var createIndexResponse = await _client.Indices.CreateAsync(Constants.SmartApartment, c => c
                .Settings(s => s
                    .Analysis(Analyzer))
                .Map<Management>(m => m
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
                );

            return createIndexResponse.IsValid;
        }
    }
}