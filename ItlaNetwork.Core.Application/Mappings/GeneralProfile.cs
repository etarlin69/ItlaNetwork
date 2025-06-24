using AutoMapper;
using ItlaNetwork.Core.Application.ViewModels.Account;
using ItlaNetwork.Core.Application.ViewModels.Comment;
using ItlaNetwork.Core.Application.ViewModels.Friendship;
using ItlaNetwork.Core.Application.ViewModels.Post;
using ItlaNetwork.Core.Application.ViewModels.Reaction;
using ItlaNetwork.Core.Domain.Entities;

namespace ItlaNetwork.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            // ... (Mapeos existentes de User y Post)
            CreateMap<RegisterViewModel, User>()
               .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true))
               .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => true));

            CreateMap<Post, SavePostViewModel>().ReverseMap();

            CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.AuthorUserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.AuthorProfilePictureUrl, opt => opt.MapFrom(src => src.User.ProfilePictureUrl))
                .ReverseMap();

            // --- MAPEADOS AÑADIDOS ---
            CreateMap<Comment, CommentViewModel>()
                .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.AuthorProfilePictureUrl, opt => opt.MapFrom(src => src.User.ProfilePictureUrl))
                .ReverseMap();

            // Este es el nuevo mapeo para el formulario de comentarios
            CreateMap<SaveCommentViewModel, Comment>().ReverseMap();

            CreateMap<SaveReactionViewModel, Reaction>()
    .ForMember(dest => dest.ReactionType, opt => opt.MapFrom(src => (int)src.ReactionType))
    .ReverseMap();

            CreateMap<Friendship, FriendViewModel>()
    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.FriendId))
    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Friend.FirstName))
    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Friend.LastName))
    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Friend.UserName))
    .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.Friend.ProfilePictureUrl))
    .ReverseMap();

            CreateMap<FriendRequest, FriendRequestViewModel>()
                .ForMember(dest => dest.RequestingUserName, opt => opt.MapFrom(src => src.Sender.UserName))
                .ForMember(dest => dest.RequestingUserProfilePictureUrl, opt => opt.MapFrom(src => src.Sender.ProfilePictureUrl))
                .ReverseMap();
        }
    }
}