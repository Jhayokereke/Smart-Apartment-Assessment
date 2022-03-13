using SmartApartment.Application.Commons;

namespace SmartApartment.Application.Dtos
{
    public class SearchResponse : BaseResponse<SearchData>
    {
        public SearchResponse(string message = "", bool success = false) : base(message, success)
        {
        }
    }

    public class SearchData
    {
        public bool HasMatch { get; set; }
        public long Count { get; set; }
        public object[] Hits { get; set; }
    }
}
