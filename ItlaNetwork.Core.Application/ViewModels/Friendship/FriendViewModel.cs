namespace ItlaNetwork.Core.Application.ViewModels.Friendship
{
    public class FriendViewModel
    {
        // FIX: Added 'Id' property to solve the error in Friend/Index.cshtml.
        // It's the same as UserId but matches what the view expects.
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}