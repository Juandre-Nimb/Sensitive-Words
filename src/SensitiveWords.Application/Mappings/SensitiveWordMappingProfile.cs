using AutoMapper;
using SensitiveWordsClean.Application.DTOs;
using SensitiveWordsClean.Domain.Entities;

namespace SensitiveWordsClean.Application.Mappings;

public class SensitiveWordMappingProfile : Profile
{
    public SensitiveWordMappingProfile()
    {
        CreateMap<SensitiveWord, SensitiveWordDto>();
        
        CreateMap<CreateSensitiveWordDto, SensitiveWord>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<UpdateSensitiveWordDto, SensitiveWord>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
