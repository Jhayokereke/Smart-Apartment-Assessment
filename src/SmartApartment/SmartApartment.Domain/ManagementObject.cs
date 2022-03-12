using Newtonsoft.Json;

namespace SmartApartment.Domain
{
    public class ManagementObject
    {
        [JsonProperty("mgmt")]
        public Management Management { get; set; }
    }

    public class Management
    {
        [JsonProperty("mgmtID")]
        public int ManagementID { get; set; }
        public string Name { get; set; }
        public string Market { get; set; }
        public string State { get; set; }
    }

}
