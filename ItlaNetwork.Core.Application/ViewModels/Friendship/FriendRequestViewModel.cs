public class FriendRequestViewModel
{
    public int Id { get; set; }
    public string RequestingUserId { get; set; }
    public string RequestingUserName { get; set; }
    public string? RequestingUserProfilePictureUrl { get; set; }
    public DateTime Created { get; set; }
    public string Status { get; set; } // "Pending", etc.
}
