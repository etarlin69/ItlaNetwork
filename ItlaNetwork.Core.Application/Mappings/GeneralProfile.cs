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
            // El mapeo de Post a SavePostViewModel y viceversa ya funciona
            // correctamente gracias a que AutoMapper mapea por nombre de propiedad.
            CreateMap<Post, SavePostViewModel>().ReverseMap();

            // El resto de tus mapeos...
            CreateMap<LoginViewModel, AuthenticationRequest>().ReverseMap();
            CreateMap<RegisterViewModel, RegisterRequest>().ReverseMap();
            CreateMap<User, FriendViewModel>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).ReverseMap();
            CreateMap<Post, PostViewModel>().ReverseMap();
            CreateMap<SaveCommentViewModel, Comment>().ReverseMap();
            CreateMap<Comment, CommentViewModel>().ReverseMap();
            CreateMap<SaveReactionViewModel, Reaction>().ReverseMap();
            CreateMap<Reaction, ReactionViewModel>().ReverseMap();
            CreateMap<FriendRequest, FriendRequestViewModel>().ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())).ReverseMap();
        }
    }
}