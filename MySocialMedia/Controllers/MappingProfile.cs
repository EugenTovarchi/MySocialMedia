using AutoMapper;
using MySocialMedia.Extensions;
using MySocialMedia.Models.Users;
using MySocialMedia.ViewModels.Account;

namespace MySocialMedia.Controllers;

public class MappingProfile : Profile
{
    /// <summary>
    /// В конструкторе настроим соответствие сущностей при маппинге
    /// </summary>
    public MappingProfile()
    {

        CreateMap<RegisterViewModel, User>().ForMember(x => x.BirthDate, opt => opt.MapFrom(src => src.BirthDate));
        //.ForMember(x => x.BirthDate, opt => opt.MapFrom(c => new DateTime((int)c.Year, (int)c.Month, (int)c.Date)))
        //.ForMember(x => x.Email, opt => opt.MapFrom(c => c.EmailReg))
        //.ForMember(x => x.UserName, opt => opt.MapFrom(c => c.Login));
        CreateMap<LoginViewModel, User>();

        CreateMap<UserEditViewModel, User>();
        CreateMap<User, UserEditViewModel>().ForMember(x => x.UserId, opt => opt.MapFrom(c => c.Id));

        CreateMap<UserWithFriendExt, User>();
        CreateMap<User, UserWithFriendExt>();

        CreateMap<RegisterViewModel, User>()
            .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.EmailReg))
            .ForMember(x => x.Email, opt => opt.MapFrom(src => src.EmailReg));
    }
}

