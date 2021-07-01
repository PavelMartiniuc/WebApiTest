using Newtonsoft.Json;
using System;

namespace WebApiTest.DomainObjects
{
    public class CompanyDetails
    {
        [JsonProperty("company_name")]
        public string Name { get; set; }

        [JsonProperty("company_number")]
        public string RegistratioNumber { get; set; }

        [JsonProperty("registered_office_address")]
        public CompanyAddress OfficeAddress { get; set; }

        [JsonProperty("date_of_creation")]
        public DateTime Created { get; set; }
    }
}
