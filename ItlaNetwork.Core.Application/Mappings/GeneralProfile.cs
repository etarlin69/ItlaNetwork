using AutoMapper;
using ItlaNetwork.Core.Application.DTOs.Account;
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
            // POSTS
            CreateMap<Post, SavePostViewModel>()
                .ReverseMap();
            CreateMap<Post, PostViewModel>()
                .ReverseMap();

            // AUTENTICACIÓN
            CreateMap<LoginViewModel, AuthenticationRequest>()
                .ReverseMap();
            CreateMap<RegisterViewModel, RegisterRequest>()
                .ReverseMap();

            // COMENTARIOS
            CreateMap<SaveCommentViewModel, Comment>()
                .ReverseMap();
            CreateMap<Comment, CommentViewModel>()
                .ForMember(dest => dest.PostId,
                           opt => opt.MapFrom(src => src.PostId))
                .ReverseMap();

            // REACCIONES
            CreateMap<SaveReactionViewModel, Reaction>()
                .ReverseMap();
            CreateMap<Reaction, ReactionViewModel>()
                .ReverseMap();

            // AMIGOS
            CreateMap<User, FriendViewModel>()
                .ForMember(dest => dest.Id,
                           opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId,
                           opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            // SOLICITUDES DE AMISTAD
            CreateMap<FriendRequest, FriendRequestViewModel>()
                .ForMember(dest => dest.Status,
                           opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap();
        }
    }
}
