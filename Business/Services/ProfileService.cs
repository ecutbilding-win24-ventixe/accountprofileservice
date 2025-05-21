using Business.Interfaces;
using Business.Models;
using Business.Models.Requests;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.Models;

namespace Business.Services;

public class ProfileService(IProfileRepository profileRepository, IAddressInfoRepository addressInfoRepository, IAddressTypeRepository addressTypeRepository) : IProfileService
{
    private readonly IProfileRepository _profileRepository = profileRepository;
    private readonly IAddressInfoRepository _addressInfoRepository = addressInfoRepository;
    private readonly IAddressTypeRepository _addressTypeRepository = addressTypeRepository;


    public async Task<AccountProfileResult<Profile>> GetProfileByUserIdAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("Event ID cannot be null or empty.", nameof(userId));

        try
        {
            var result = await _profileRepository.GetAsync(x => x.UserId == userId, x => x.AddressInfo );
            if (!result.Succeeded || result.Result == null)
                return new AccountProfileResult<Profile> { Succeeded = false, StatusCode = 404, Message = "Profile not found." };

            return new AccountProfileResult<Profile>{ Succeeded = true, StatusCode = 200, Message = "Profile retrieved successfully.",Result = result.Result };
        }
        catch (Exception ex) 
        {
            return new AccountProfileResult<Profile> { Succeeded = false, Message = $"An error occurred while retrieving the profile.{ex.Message}" };
        }
    }

    public async Task<AccountProfileResult<IEnumerable<Profile>>> GetAllProfilesAsync()
    {
        try
        {
            var result = await _profileRepository.GetAllAsync(orderByDescending: true, sortBy: x => x.FirstName!, includes: 
                [
                    x => x.AddressInfo,
                    x => x.AddressInfo.AddressType
                ]
            );

            if (!result.Succeeded || result.Result == null)
                return new AccountProfileResult<IEnumerable<Profile>> { Succeeded = false, StatusCode = 404, Message = "No profiles found." };

            return new AccountProfileResult<IEnumerable<Profile>> { Succeeded = true, StatusCode = 200, Message = "Profiles retrieved successfully.", Result = result.Result };
        }
        catch (Exception ex)
        {
            return new AccountProfileResult<IEnumerable<Profile>> { Succeeded = false, Message = $"An error occurred while retrieving the profiles.{ex.Message}" };
        }
    }

    public async Task<AccountProfileResult> CreateProfileAsync(CreateAccountProfileRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request),"Request cannot be null! Im sorry :/");

        try
        {
            await _profileRepository.BeginTransactionAsync();

            var newAddress = new AddressInfoEntity
            {
                StreetName = request.StreetName ?? "",
                StreetNumber = request.StreetNumber ?? "",
                PostalCode = request.PostalCode ?? "",
                City = request.City ?? "",
                AddressTypeId = request.AddressTypeId != 0 ? request.AddressTypeId : 1,
            };

            var addressResult = await _addressInfoRepository.AddAsync(newAddress);
            if (!addressResult.Succeeded)
            {
                await _profileRepository.RollbackTransactionAsync();
                return new AccountProfileResult { Succeeded = false, StatusCode = 400, Message = "Failed to create address." };
            }

            var newProfile = new ProfileEntity
            {
                UserId = request.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                AddressInfoId = newAddress.Id,
            };

            var profileResult = await _profileRepository.AddAsync(newProfile);
            if (!profileResult.Succeeded)
            {
                await _profileRepository.RollbackTransactionAsync();
                return new AccountProfileResult { Succeeded = false, StatusCode = 400, Message = "Failed to create profile." };
            }
            await _profileRepository.CommitTransactionAsync();
            return new AccountProfileResult { Succeeded = true, StatusCode = 201, Message = "Profile created successfully." };

        }
        catch (Exception ex)
        {
            await _profileRepository.RollbackTransactionAsync();
            return new AccountProfileResult { Succeeded = false, Message = $"An error occurred while creating the profile.{ex.Message}" };
        }
    }

    public async Task<AccountProfileResult> UpdateProfileAsync(UpdateAccountProfileRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request), "Request cannot be null.");

        try
        {
            await _profileRepository.BeginTransactionAsync();
            var profileResult = await _profileRepository.GetAsync(x => x.UserId == request.UserId, x => x.AddressInfo);
            if (!profileResult.Succeeded || profileResult.Result == null)
                return new AccountProfileResult { Succeeded = false, StatusCode = 404, Message = "Profile not found." };

            var existingProfile = profileResult.Result.MapTo<ProfileEntity>();

            existingProfile.FirstName = request.FirstName;
            existingProfile.LastName = request.LastName;
            existingProfile.AddressInfo.StreetName = request.StreetName;
            existingProfile.AddressInfo.StreetNumber = request.StreetNumber;
            existingProfile.AddressInfo.PostalCode = request.PostalCode;
            existingProfile.AddressInfo.City = request.City;
            existingProfile.AddressInfo.AddressTypeId = request.AddressTypeId;

            var updateResult = await _profileRepository.UpdateAsync(existingProfile);
            await _profileRepository.CommitTransactionAsync();
            return updateResult.Succeeded
                ? new AccountProfileResult { Succeeded = true, StatusCode = 200, Message = "Profile updated successfully." }
                : new AccountProfileResult { Succeeded = false, StatusCode = 400, Message = "Failed to update profile." };
        }
        catch (Exception ex)
        {
           await _profileRepository.RollbackTransactionAsync();
            return new AccountProfileResult { Succeeded = false, StatusCode = 500, Message = $"An error occurred while updating the profile.{ex.Message}" };
        }
    }

    public async Task<AccountProfileResult> DeleteProfileAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

        try
        {
            await _profileRepository.BeginTransactionAsync();
            var profileResult = await _profileRepository.GetAsync(x => x.UserId == userId);
            if (!profileResult.Succeeded || profileResult.Result == null)
                return new AccountProfileResult { Succeeded = false, StatusCode = 404, Message = "Profile not found." };

            var profileEntity = profileResult.Result.MapTo<ProfileEntity>();
            var deleteResult = await _profileRepository.DeleteAsync(profileEntity);

            await _profileRepository.CommitTransactionAsync();
            return deleteResult.Succeeded 
                ? new AccountProfileResult { Succeeded = true, StatusCode = 200, Message = "Profile deleted successfully." }
                : new AccountProfileResult { Succeeded = false, StatusCode = 400, Message = "Failed to delete profile." };
        }
        catch (Exception ex)
        {
            await _profileRepository.RollbackTransactionAsync();
            return new AccountProfileResult { Succeeded = false, StatusCode = 500, Message = $"An error occurred while deleting the profile. {ex.Message}" };
        }
    }
}
