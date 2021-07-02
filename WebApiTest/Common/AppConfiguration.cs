namespace WebApiTest.Common.Configuration
{
    public class AppConfiguration
    {
        public string ApiKey { get; set; }
        public int NumberOfResults { get; set; }
        public string BaseAddress { get; set; }
        public int TimeoutSecs { get; set; }
        public int CacheTimeoutSecs { get; set; }
    }
}
