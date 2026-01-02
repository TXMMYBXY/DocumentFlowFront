using AutoMapper;
using DocumentFlowing.Client.Authorization.Dtos;
using DocumentFlowing.Client.Authorization.ViewModels;
using System.DirectoryServices.ActiveDirectory;

namespace DocumentFlowing.Client.Authorization;

public class AuthorizationMappingProfile : Profile
{
    public AuthorizationMappingProfile()
    {
        CreateMap<RefreshTokenToLoginResponseViewModel, RefreshTokenResponseViewModel>()
            .ForMember(dest => dest.ExpiresAt, opt => opt.MapFrom(src => src.RefreshTokenDto.ExpiresAt))
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.RefreshTokenDto.Token))
            .ReverseMap();

        CreateMap<AccessTokenViewModelResponse, LoginResponseDto>().ReverseMap();
    }
}