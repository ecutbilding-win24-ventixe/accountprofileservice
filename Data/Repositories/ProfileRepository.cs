using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class ProfileRepository(DataContext context) : BaseRepository<ProfileEntity, Profile>(context), IProfileRepository
{
}
