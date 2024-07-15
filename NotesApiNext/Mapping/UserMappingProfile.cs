using AutoMapper;
using NotesApiNext.ApiTypes;
using NotesApiNext.Interfaces;
using NotesApiNext.Models.User;

namespace NotesApiNext.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile(IDateTimeProvider dateTimeProvider)
        {
            CreateMap<UserDto, User>()
                //.ForMember GuidUserId
                .ForMember(user => user.UserId, opt => opt.MapFrom(_ => Guid.NewGuid()))
                //.ForMember DateTime
                .ForMember(user => user.RegistrDateTime, opt => opt.MapFrom(_ => dateTimeProvider.UtcNow))
                .ForMember(user => user.UserName, opt => opt.MapFrom(user => user.UserName))
                .ForMember(user => user.Email, opt => opt.MapFrom(user => user.Email))
                .ForMember(user => user.Password, opt => opt.MapFrom(user => user.Password)); //Encode
        }
    }
}
