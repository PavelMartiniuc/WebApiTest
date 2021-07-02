using System;
using System.Collections.Generic;
using System.Net.Http;
using WebApiTest.Common.Configuration;
using System.Text;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApiTest.Common;
using WebApiTest.Dto;
using WebApiTest.DomainObjects;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace WebApiTest
{
    public class CompanyApiClient
    {
        private readonly HttpClient _client;
        private readonly AppConfiguration _config;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private Dictionary<string, CacheItem> _cacheItems;

        public CompanyApiClient(AppConfiguration config, ILogger logger, IMapper mapper, IMemoryCache cache)
        {
            _config = config;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _client = new HttpClient();
            _cacheItems = new Dictionary<string, CacheItem>();
            Init(_client);
        }

        private void Init(HttpClient client)
        {
            client.Timeout = TimeSpan.FromSeconds(_config.TimeoutSecs);
            var apiKey = _config.ApiKey;
            var encriptedkey = Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey));
            var authHeaderValue = new AuthenticationHeaderValue("Basic", encriptedkey);
            client.DefaultRequestHeaders.Authorization = authHeaderValue;
            client.BaseAddress = new Uri(_config.BaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public async Task<IEnumerable<CompanyDto>> GetCompaniesAsync(string companyName)
        {
            string _requestUrl = $"search/companies?q={Uri.EscapeDataString(companyName)}&{GetIntervalString()}";
            _logger.LogInformation($"Perform call to {_requestUrl}");
            var companies = GetFromCache<List<Company>>(_requestUrl);
            if (companies == null)
            {
                HttpResponseMessage response = await _client.GetAsync(_requestUrl);
                var result = await ProcessResult<CollectionResult<Company>>(response);
                companies = result.Items;
                SaveInCache(_requestUrl, companies);
            }
            return _mapper.Map<List<CompanyDto>>(companies);
        }

        public async Task<CompanyDto> GetCompanyAsync(string companyNumber)
        {
            string _requestUrl = $"company/{companyNumber}";
            _logger.LogInformation($"Perform call to {_requestUrl}");
            var company = GetFromCache<CompanyDetails>(_requestUrl);
            if (company == null)
            {
                HttpResponseMessage response = await _client.GetAsync(_requestUrl);
                company = await ProcessResult<CompanyDetails>(response);
                SaveInCache(_requestUrl, company);
            }
            return _mapper.Map<CompanyDto>(company);
        }

        public async Task<IEnumerable<OfficerDto>> GetCompanyOfficersAsync(string companyNumber)
        {
            string _requestUrl = $"company/{companyNumber}/officers?{GetIntervalString()}";
            _logger.LogInformation($"Perform call to {_requestUrl}");
            var officers = GetFromCache<List<Officer>>(_requestUrl);
            if (officers == null)
            {
                HttpResponseMessage response = await _client.GetAsync(_requestUrl);
                var result = await ProcessResult<CollectionResult<Officer>>(response);
                officers = result.Items;
                SaveInCache(_requestUrl, officers);
            }
            return _mapper.Map<List<OfficerDto>>(officers);
        }

        private string GetIntervalString()
        {
            return $"start_index=1&items_per_page={_config.NumberOfResults}";
        }

        private T GetFromCache<T>(string key)
        {
            T cacheEntry;
            if (_cache.TryGetValue<T>(key, out cacheEntry))
                return cacheEntry;
            return default(T);
        }

        private void SaveInCache<T>(string key, T value)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(_config.CacheTimeoutSecs));
            _cache.Set(key, value, cacheEntryOptions);
            var cacheType = CacheType.Campany;            
            if (typeof(T) == typeof(Company))
                cacheType = CacheType.Campany;
            if (typeof(T) == typeof(List<Company>))
                cacheType = CacheType.Companies;
            if (typeof(T) == typeof(List<Officer>))
                cacheType = CacheType.Officers;

            _cacheItems[key] = new CacheItem
            {
                Key = key,
                Type = cacheType,
                JsonData = JsonConvert.SerializeObject(value)
            };
            //try
            //{
            //    var formatter = new BinaryFormatter();
            //    var stream = new FileStream("CacheData.txt", FileMode.Create, FileAccess.Write);
            //    #pragma warning disable SYSLIB0011
            //    formatter.Serialize(stream, _cacheItems);
            //    #pragma warning restore SYSLIB0011
            //    stream.Close();
            //}
            //catch(Exception ex)
            //{

            //}
        }

        private async Task<T> ProcessResult<T>(HttpResponseMessage response)
        {
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(responseBody);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw new ClientErrorResponse(response.StatusCode, "Error parsing API response");
                }
            }
            string error = "Not OK response form API";
            _logger.LogWarning($"{error}: {responseBody}");
            throw new ClientErrorResponse(response.StatusCode, error);
        }
    }
}
