using AutoMapper;
using System;
using WebApiTest.Common;
using WebApiTest.DomainObjects;
using WebApiTest.Dto;

namespace WebApiTest.AutoMapper
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Age, opt => 
                    opt.MapFrom(src => AgeHelper.GetAge(src.Created)));
            CreateMap<CompanyDetails, CompanyDto>()
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => GetCompanyAddress(src.OfficeAddress)))
               .ForMember(dest => dest.Age, opt =>
                   opt.MapFrom(src => AgeHelper.GetAge(src.Created)));
        }

        private string GetCompanyAddress(CompanyAddress address)
        {
            return $"{address.Locality}, {address.Address}, {address.PostalCode}";
        }
    }
}
