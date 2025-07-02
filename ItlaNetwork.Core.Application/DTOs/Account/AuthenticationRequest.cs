namespace ItlaNetwork.Core.Application.DTOs.Account
{
    public class AuthenticationRequest
    {
        // FIX: Changed from 'Email' to align with the LoginViewModel.
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
    }
}