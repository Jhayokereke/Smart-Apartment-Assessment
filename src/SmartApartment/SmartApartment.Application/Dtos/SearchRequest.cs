using MediatR;

namespace SmartApartment.Application.Dtos
{
    public class SearchRequest : IRequest<SearchResponse>
    {
        public string SearchPhrase { get; set; }
        public string[] Markets { get; set; }
        public int Limit { get; set; } = 10;
        public int Pagenumber { get; set; } = 1;
    }
}
