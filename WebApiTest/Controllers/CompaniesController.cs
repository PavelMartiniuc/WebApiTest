using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTest.Common.Configuration;
using WebApiTest.Dto;

namespace WebApiTest.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api")]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly AppConfiguration _config;
        private readonly CompanyApiClient _client;
        private readonly IMapper _mapper;

        public CompaniesController(ILogger<CompaniesController> logger, IOptions<AppConfiguration> config, IMapper mapper)
        {
            _logger = logger;
            _config = config.Value;
            _mapper = mapper;
            _client = new CompanyApiClient(_config, _logger, _mapper);
        }

        [HttpGet("companies/{companyName}")]
        public async Task<IEnumerable<CompanyDto>> GetCompaniesNyName(string companyName)
        {
            var companies = await _client.GetCompaniesAsync(companyName);
            return companies;
        }

        [HttpGet("companies/bynumber/{companyNumber}")]
        public async Task<CompanyDto> GetCompanyByNumber(string companyNumber)
        {
            var company = await _client.GetCompanyAsync(companyNumber);
            return company;
        }

        [HttpGet("companies/bynumber/{companyNumber}/officers")]
        public async Task<IEnumerable<OfficerDto>> GetCompanyOfficers(string companyNumber)
        {
            var officers = await _client.GetCompanyOfficersAsync(companyNumber);
            return officers;
        }
    }
}
