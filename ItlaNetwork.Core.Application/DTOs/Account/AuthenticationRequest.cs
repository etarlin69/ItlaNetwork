namespace ItlaNetwork.Core.Application.DTOs.Account
{
    public class AuthenticationRequest
    {
        
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
    }
}