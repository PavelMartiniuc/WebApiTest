
namespace WebApiTest.DomainObjects
{
    public class CacheItem
    {
        public string Key { get; set; }
        public CacheType Type { get; set; }
        public string JsonData { get; set; }
    }

    public enum CacheType
    {
        Companies,
        Campany,
        Officers
    }
}
