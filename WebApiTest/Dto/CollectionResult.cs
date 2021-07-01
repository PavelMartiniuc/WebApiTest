using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebApiTest.Dto
{
    public class CollectionResult<T>
    {
        [JsonProperty("items")]
        public List<T> Items { get; set; }
    }
}
