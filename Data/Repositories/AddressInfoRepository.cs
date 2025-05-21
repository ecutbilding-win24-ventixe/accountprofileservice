using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class AddressInfoRepository(DataContext context) : BaseRepository<AddressInfoEntity, AddressInfo>(context), IAddressInfoRepository
{
}
