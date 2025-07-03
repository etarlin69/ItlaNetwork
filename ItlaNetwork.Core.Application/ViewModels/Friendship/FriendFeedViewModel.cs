using ItlaNetwork.Core.Application.ViewModels.Friendship;
using ItlaNetwork.Core.Application.ViewModels.Post;
using System.Collections.Generic;

namespace ItlaNetwork.Core.Application.ViewModels.Friendship
{
    public class FriendFeedViewModel
    {
        public List<FriendViewModel> Friends { get; set; }
        public List<PostViewModel> Posts { get; set; }
    }
}
