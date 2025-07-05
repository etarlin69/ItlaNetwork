using ItlaNetwork.Core.Application.ViewModels.Post;
using System.Collections.Generic;

namespace ItlaNetwork.Core.Application.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<PostViewModel> Posts { get; set; }
        public SavePostViewModel NewPost { get; set; }

        
        public string CurrentUserName { get; set; }
    }
}