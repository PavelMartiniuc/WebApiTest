using AutoMapper;
using System;
using WebApiTest.Common;
using WebApiTest.DomainObjects;
using WebApiTest.Dto;

namespace WebApiTest.AutoMapper
{
    public class OfficerProfile : Profile
    {
        public OfficerProfile()
        {
            CreateMap<Officer, OfficerDto>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => new DateTime(src.DateOfBitth.Year, src.DateOfBitth.Month, 1).ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Age, opt => 
                    opt.MapFrom(src => AgeHelper.GetAproximateAge(src.DateOfBitth.Year, src.DateOfBitth.Month)));
        }
    }
}
