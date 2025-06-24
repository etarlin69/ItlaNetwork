namespace ItlaNetwork.Core.Application.ViewModels.Friendship
{
    public class FriendViewModel
    {
        public int Id { get; set; } // El Id de la entidad Friendship
        public string UserId { get; set; } // El Id del amigo (el string de Identity)
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}