using System;
using Newtonsoft.Json;

namespace WebApiTest.DomainObjects
{
    public class Company
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("company_number")]
        public string RegistratioNumber { get; set; }

        [JsonProperty("address_snippet")]
        public string Address { get; set; }

        [JsonProperty("date_of_creation")]
        public DateTime Created { get; set; }
    }
}
