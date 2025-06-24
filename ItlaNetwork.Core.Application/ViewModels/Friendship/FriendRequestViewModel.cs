using System;

namespace ItlaNetwork.Core.Application.ViewModels.Friendship
{
    public class FriendRequestViewModel
    {
        public int Id { get; set; } // El Id de la solicitud
        public string RequestingUserId { get; set; }
        public string RequestingUserName { get; set; }
        public string RequestingUserProfilePictureUrl { get; set; }
        public int CommonFriendsCount { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
    }
}