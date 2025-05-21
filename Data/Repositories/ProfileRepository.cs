using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public class ProfileRepository(DataContext context) : BaseRepository<ProfileEntity, Profile>(context), IProfileRepository
{
    public override async Task<RepositoryResult<Profile>> GetAsync(Expression<Func<ProfileEntity, bool>> where, params Expression<Func<ProfileEntity, object>>[] includes)
    {
        try
        {
            IQueryable<ProfileEntity> query = _table;

            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            var entity = await query
                .Include(p => p.AddressInfo)
                .Include(p => p.AddressInfo.AddressType)
                .FirstOrDefaultAsync(where);

            if (entity == null)
                return new RepositoryResult<Profile> { Succeeded = false, StatusCode = 404, Result = null };

            var domainModel = new Profile
            {
                Id = entity.UserId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                AddressInfo = new AddressInfo
                {
                    StreetName = entity.AddressInfo.StreetName,
                    StreetNumber = entity.AddressInfo.StreetNumber,
                    City = entity.AddressInfo.City,
                    PostalCode = entity.AddressInfo.PostalCode,
                    AddressType = new AddressTypeModel
                    {
                        AddressType = entity.AddressInfo.AddressType.AddressType,
                    }
                }
            };
            return new RepositoryResult<Profile> { Succeeded = true, StatusCode = 200, Result = domainModel };

        }
        catch (Exception ex)
        {
            return new RepositoryResult<Profile> { Succeeded = false, StatusCode = 500, Message = ex.Message };
        }
    }

    public override async Task<RepositoryResult<IEnumerable<Profile>>> GetAllAsync(bool orderByDescending = false, Expression<Func<ProfileEntity, object>>? sortBy = null, Expression<Func<ProfileEntity, bool>>? where = null, params Expression<Func<ProfileEntity, object>>[] includes)
    {
        try
        {
            IQueryable<ProfileEntity> query = _table;

            if (where != null)
                query = query.Where(where);

            if (includes != null && includes.Length > 0)
                foreach (var include in includes)
                    query = query.Include(include);

            query = query
                .Include(p => p.AddressInfo)
                .Include(p => p.AddressInfo.AddressType);

            if (sortBy != null)
                query = orderByDescending
                    ? query.OrderByDescending(sortBy)
                    : query.OrderBy(sortBy);

            var entities = await query.ToListAsync();

            var result = entities.Select(entity => new Profile
            {
                Id = entity.UserId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                AddressInfo = new AddressInfo
                {
                    StreetName = entity.AddressInfo.StreetName,
                    StreetNumber = entity.AddressInfo.StreetNumber,
                    City = entity.AddressInfo.City,
                    PostalCode = entity.AddressInfo.PostalCode,
                    AddressType = new AddressTypeModel
                    {
                        AddressType = entity.AddressInfo.AddressType.AddressType,
                    }
                }
            }).ToList();
            return new RepositoryResult<IEnumerable<Profile>> { Succeeded = true, StatusCode = 200, Result = result };
        }
        catch (Exception ex)
        {
            return new RepositoryResult<IEnumerable<Profile>> { Succeeded = false, StatusCode = 500, Message = ex.Message };
        }
    }
}
