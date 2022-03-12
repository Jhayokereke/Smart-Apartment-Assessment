namespace SmartApartment.Application.Dtos
{
    public class SearchResult
    {
        public bool HasMatch { get; set; }
        public long Count { get; set; }
        public object[] Hits { get; set; }
    }
}
