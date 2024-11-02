using Application.Dtos.Account;
using Application.ViewModels.User;

using AutoMapper;

namespace Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            #region User Profile

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(u => u.HasError, opt => opt.Ignore())
                .ForMember(u => u.Error, opt => opt.Ignore())                
                .ReverseMap();

            CreateMap<RegisterResponse, SaveUserViewModel>()
                .ReverseMap();

            //CreateMap<UpdateUserRequest, SaveUserViewModel>()
            //  .ForMember(u => u.HasError, opt => opt.Ignore())
            //  .ForMember(u => u.Error, opt => opt.Ignore())              
            //  .ReverseMap();

            CreateMap<RegisterResponse, AuthenticationResponse>();

            CreateMap<AuthenticationResponse, UserViewModel>()                
                .ReverseMap();

            //CreateMap<AuthenticationResponse, UpdateUserRequest>()
            //  .ReverseMap();                        
              


            CreateMap<SaveUserViewModel, MyProfileViewModel>()
              .ReverseMap();

            #endregion

            #region Login Profile

            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(u => u.HasError, opt => opt.Ignore())
                .ForMember(u => u.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ResetPasswordRequest, ResetPasswordViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ForgotPasswordRequest, ForgotPasswordViewModel>()
              .ForMember(x => x.Error, opt => opt.Ignore())
              .ForMember(x => x.HasError, opt => opt.Ignore())
              .ReverseMap();

            #endregion
        }
    }
}
