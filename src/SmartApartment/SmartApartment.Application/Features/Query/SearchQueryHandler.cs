using MediatR;
using SmartApartment.Application.Commons;
using SmartApartment.Application.Contracts;
using SmartApartment.Application.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace SmartApartment.Application.Features.Query
{
    public class SearchQueryHandler : IRequestHandler<SearchRequest, SearchResponse>
    {
        private readonly ISearchRepository _searchRepo;
        private readonly SearchRequestValidator _validator;

        public SearchQueryHandler(ISearchRepository searchRepository)
        {
            _searchRepo = searchRepository;
            _validator = new SearchRequestValidator();
        }
        public async Task<SearchResponse> Handle(SearchRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult);

            return await _searchRepo.Search(request);
        }
    }
}
