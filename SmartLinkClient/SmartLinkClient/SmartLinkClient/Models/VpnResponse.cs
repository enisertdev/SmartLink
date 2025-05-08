using System.Text.Json.Serialization;

namespace SmartLinkClient.Models
{
    public class VpnResponse
    {
        [JsonPropertyName("security")]
       public SecurityInfo Security { get; set; }
    }

    public class SecurityInfo
    {
        [JsonPropertyName("vpn")]
        public bool Vpn { get; set; }
    }


}
