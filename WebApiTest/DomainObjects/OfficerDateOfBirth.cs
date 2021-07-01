using Newtonsoft.Json;

namespace WebApiTest.DomainObjects
{
    public class OfficerDateOfBirth
    {
        [JsonProperty("month")]
        public int Month { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }
    }
}
