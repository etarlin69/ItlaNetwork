using ItlaNetwork.Core.Application.DTOs.Account;
using ItlaNetwork.Core.Domain.Entities; // Required for the User return type
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItlaNetwork.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task SignOutAsync();

        // --- New methods needed for "stitching" data in other services ---
        Task<List<User>> GetAllUsersAsync();
        Task<List<User>> GetUsersByIdsAsync(List<string> userIds);
        Task<User> GetUserByIdAsync(string userId);
    }
}