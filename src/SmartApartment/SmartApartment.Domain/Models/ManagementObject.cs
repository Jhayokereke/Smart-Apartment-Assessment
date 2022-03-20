using System.Text.Json.Serialization;

namespace SmartApartment.Domain.Models
{
    public class ManagementObject
    {
        [JsonPropertyName("mgmt")]
        public Management Management { get; set; }
    }

    public class Management
    {
        [JsonPropertyName("mgmtID")]
        public int ManagementID { get; set; }
        public string Name { get; set; }
        public string Market { get; set; }
        public string State { get; set; }
    }

}
