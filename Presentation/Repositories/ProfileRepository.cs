using Presentation.Data;
using Presentation.Entities;
using Presentation.Interfaces;

namespace Presentation.Repositories;

public class ProfileRepository(DataContex context) : BaseRepository<ProfileEntity>(context), IProfileRepository
{
}
