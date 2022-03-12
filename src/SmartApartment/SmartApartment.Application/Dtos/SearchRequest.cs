using System.ComponentModel.DataAnnotations;

namespace SmartApartment.Application.Dtos
{
    public class SearchRequest
    {
        [Required]
        public string SearchPhrase { get; set; }
        public string[] Markets { get; set; }
        public int Limit { get; set; }
    }
}
