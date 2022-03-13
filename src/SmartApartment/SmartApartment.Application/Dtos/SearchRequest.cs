using MediatR;

namespace SmartApartment.Application.Dtos
{
    public class SearchRequest : IRequest<SearchResponse>
    {
        public string SearchPhrase { get; set; }
        public string[] Markets { get; set; }
        public int Limit { get; set; }
        public int Pagenumber { get; set; }
    }
}
