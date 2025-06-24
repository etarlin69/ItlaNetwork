using System.Collections.Generic;

namespace ItlaNetwork.Core.Application.ViewModels.Friendship
{
    public class FriendRequestPageViewModel
    {
        public List<FriendRequestViewModel> ReceivedRequests { get; set; }
        public List<FriendRequestViewModel> SentRequests { get; set; }
    }
}