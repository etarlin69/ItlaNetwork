using System.Collections.Generic;
using ItlaNetwork.Core.Domain.Entities;

namespace ItlaNetwork.Core.Application.ViewModels.Friendship
{
    public class AddFriendViewModel
    {
        
        public List<FriendViewModel> Users { get; set; }
        public string? UserName { get; set; }
        public string SelectedUserId { get; set; }
    }
}