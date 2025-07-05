using System.Collections.Generic;
using System.Threading.Tasks;
using ItlaNetwork.Core.Application.DTOs.Account;
using ItlaNetwork.Core.Application.ViewModels.Account;
using ItlaNetwork.Core.Domain.Entities; 

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

        
        Task<List<User>> GetAllUsersAsync();
        Task<List<User>> GetUsersByIdsAsync(List<string> userIds);
        Task<User> GetUserByIdAsync(string userId);
        Task UpdateProfileAsync(ProfileViewModel model);
        Task<ChangePasswordResponse> ChangePasswordAsync(string userId, ChangePasswordViewModel vm);
    }
}