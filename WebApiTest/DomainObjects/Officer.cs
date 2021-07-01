using Newtonsoft.Json;
using System;

namespace WebApiTest.DomainObjects
{
    public class Officer
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("date_of_birth")]
        public OfficerDateOfBirth DateOfBitth { get; set; }

        [JsonProperty("officer_role")]
        public string Role { get; set; }
    }
}
