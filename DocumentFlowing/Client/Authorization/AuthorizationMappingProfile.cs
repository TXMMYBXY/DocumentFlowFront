using AutoMapper;
using DocumentFlowing.Client.Authorization.ViewModel;

namespace DocumentFlowing.Client.Authorization;

public class AuthorizationMappingProfile : Profile
{
    public AuthorizationMappingProfile()
    {
        CreateMap<RefreshTokenToLoginResponseViewModel, RefreshTokenResponseViewModel>()
            .ReverseMap();
    }
}