using AutoMapper;
using DocumentFlowing.Client.Authorization.Dtos;
using DocumentFlowing.Client.Authorization.ViewModels;
using System.DirectoryServices.ActiveDirectory;

namespace DocumentFlowing.Client.Authorization;

public class AuthorizationMappingProfile : Profile
{
    public AuthorizationMappingProfile()
    {
        CreateMap<AccessTokenViewModelResponse, LoginResponseDto>().ReverseMap();
    }
}