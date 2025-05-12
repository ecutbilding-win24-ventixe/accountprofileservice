using Microsoft.EntityFrameworkCore;
using Presentation.Data;

namespace Presentation.Repositories;

public abstract class BaseRepository<TEntity> where TEntity : class
{
    protected readonly DataContex _context;
    protected readonly DbSet<TEntity> _table;

    protected BaseRepository(DataContex context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }
}
