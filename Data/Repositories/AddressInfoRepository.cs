using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Data.Repositories;

public class AddressInfoRepository(DataContext context) : BaseRepository<AddressInfoEntity, AddressInfo>(context), IAddressInfoRepository
{
    public override async Task<RepositoryResult<AddressInfo>> GetAsync(Expression<Func<AddressInfoEntity, bool>> where, params Expression<Func<AddressInfoEntity, object>>[] includes)
    {
        try
        {
            IQueryable<AddressInfoEntity> query = _table;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var entity = await query
                .Include(x => x.AddressType)
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(where);

            if (entity == null)
                return new RepositoryResult<AddressInfo> { Succeeded = false, StatusCode = 404, Message = "Event not found." };

            var domainModel = new AddressInfo
            {
                Id = entity.Id,
                StreetName = entity.StreetName,
                StreetNumber = entity.StreetNumber,
                City = entity.City,
                PostalCode = entity.PostalCode,
                UserId = new Profile
                {
                    Id = entity.UserId,
                    FirstName = entity.Profile.FirstName,
                    LastName = entity.Profile.LastName,
                },
                AddressType = new AddressTypeModel
                {
                    AddressType = entity.AddressType.AddressType                
                }
            };

            return new RepositoryResult<AddressInfo> { StatusCode = 200, Succeeded = true, Result = domainModel };
        }
        catch (Exception ex)
        {
            return new RepositoryResult<AddressInfo>
            {
                Succeeded = false,
                StatusCode = 500,
                Message = $"An error occurred while retrieving the address info {ex.Message}"
            };
        }
    }

    public override async Task<RepositoryResult<IEnumerable<AddressInfo>>> GetAllAsync(bool orderByDescending = false, Expression<Func<AddressInfoEntity, object>>? sortBy = null, Expression<Func<AddressInfoEntity, bool>>? where = null, params Expression<Func<AddressInfoEntity, object>>[] includes)
    {
        try
        {
            IQueryable<AddressInfoEntity> query = _table;
            if (where != null)
                query = query.Where(where);

            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            query = query
               .Include(x => x.AddressType)
               .Include(x => x.Profile);

            if (sortBy != null)
                query = orderByDescending ? query.OrderByDescending(sortBy) : query.OrderBy(sortBy);

            var entities = await query.ToListAsync();

            var domainModels = entities.Select(entity => new AddressInfo
            {
                Id = entity.Id,
                StreetName = entity.StreetName,
                StreetNumber = entity.StreetNumber,
                City = entity.City,
                PostalCode = entity.PostalCode,
                UserId = new Profile
                {
                    Id = entity.UserId,
                    FirstName = entity.Profile.FirstName,
                    LastName = entity.Profile.LastName,
                },
                AddressType = new AddressTypeModel
                {
                    AddressType = entity.AddressType.AddressType
                }
            }).ToList();

            return new RepositoryResult<IEnumerable<AddressInfo>> { Succeeded = true, StatusCode = 200, Result = domainModels };
        }
        catch (Exception ex)
        {
            return new RepositoryResult<IEnumerable<AddressInfo>>
            {
                Succeeded = false,
                StatusCode = 500,
                Message = $"An error occurred while retrieving the address info {ex.Message}"
            };
        }
    }
}
