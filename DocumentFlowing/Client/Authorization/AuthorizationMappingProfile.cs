using AutoMapper;
using DocumentFlowing.Client.Authorization.ViewModels;

namespace DocumentFlowing.Client.Authorization;

public class AuthorizationMappingProfile : Profile
{
    public AuthorizationMappingProfile()
    {
        CreateMap<RefreshTokenToLoginResponseViewModel, RefreshTokenResponseViewModel>()
            .ReverseMap();
    }
}