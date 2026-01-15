using AutoMapper;
using DocumentFlowing.Client.Authorization.Dtos;
using System.DirectoryServices.ActiveDirectory;

namespace DocumentFlowing.Client.Authorization;

public class AuthorizationMappingProfile : Profile
{
    public AuthorizationMappingProfile()
    {
        CreateMap<AccessTokenResponseDto, LoginResponseDto>().ReverseMap();
    }
}