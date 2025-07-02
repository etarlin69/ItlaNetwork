namespace ItlaNetwork.Core.Domain.Entities
{
    
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}