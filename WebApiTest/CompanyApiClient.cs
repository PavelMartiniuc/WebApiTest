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

namespace WebApiTest
{
    public class CompanyApiClient
    {
        private readonly HttpClient _client;
        private readonly AppConfiguration _config;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CompanyApiClient(AppConfiguration config, ILogger logger, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
            _logger = logger;
            _client = new HttpClient();
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
            HttpResponseMessage response = await _client.GetAsync(_requestUrl);
            var result = await ProcessResult<CollectionResult<Company>>(response);
            var companies = result.Items;
            return _mapper.Map<List<CompanyDto>>(companies);
        }

        public async Task<CompanyDto> GetCompanyAsync(string companyNumber)
        {
            string _requestUrl = $"company/{companyNumber}";
            HttpResponseMessage response = await _client.GetAsync(_requestUrl);
            var company = await ProcessResult<CompanyDetails>(response);
            return _mapper.Map<CompanyDto>(company);
        }

        public async Task<IEnumerable<OfficerDto>> GetCompanyOfficersAsync(string companyNumber)
        {
            string _requestUrl = $"company/{companyNumber}/officers?{GetIntervalString()}";
            HttpResponseMessage response = await _client.GetAsync(_requestUrl);
            var result = await ProcessResult<CollectionResult<Officer>>(response);
            var officers = result.Items;
            return _mapper.Map<List<OfficerDto>>(officers);
        }

        private string GetIntervalString()
        {
            return $"start_index=1&items_per_page={_config.NumberOfResults}";
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
                    throw new ClientErrorResponse(response.StatusCode, "Error parsing reposne");
                }
            }

            throw new ClientErrorResponse(response.StatusCode, responseBody);
        }
    }
}
