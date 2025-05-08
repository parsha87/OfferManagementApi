using AutoMapper;
using OfferManagementApi.Data.Entities;
using OfferManagementApi.Models;

namespace OfferManagementApi.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
                   
            CreateMap<TechnicalDetailsMappingViewModel, InquiryBrandMapping>();
            // Create mappings between DTO and Entity
            CreateMap<BrandDto, Brand>().ReverseMap();
            CreateMap<CustomerDto, Customer>().ReverseMap();
            CreateMap<FrequencyDto, Frequency>().ReverseMap();
            CreateMap<ListOfValueDto, ListOfValue>().ReverseMap();
        }
    }
}
