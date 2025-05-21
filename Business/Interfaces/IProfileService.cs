using Business.Models.Requests;
using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IProfileService
{
    Task<AccountProfileResult<Profile>> GetProfileByUserIdAsync(string userId);
    Task<AccountProfileResult<IEnumerable<Profile>>> GetAllProfilesAsync();
    Task<AccountProfileResult> CreateProfileAsync(CreateAccountProfileRequest request);
    Task<AccountProfileResult> UpdateProfileAsync(UpdateAccountProfileRequest request);
    Task<AccountProfileResult> DeleteProfileAsync(string userId);
}
