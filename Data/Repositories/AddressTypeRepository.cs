﻿using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class AddressTypeRepository(DataContext context) : BaseRepository<AddressTypeEntity, AddressTypeModel>(context), IAddressTypeRepository
{
}