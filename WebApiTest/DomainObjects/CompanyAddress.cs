using Newtonsoft.Json;

namespace WebApiTest.DomainObjects
{
    public class CompanyAddress
    {
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("address_line_1")]
        public string Address { get; set; }

        [JsonProperty("locality")]
        public string Locality { get; set; }
    }
}
