using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Silver
{
    public interface ISilverDb : IDisposable
    {
        DbSet<IUser> Users { get; set; }
        DbSet<IUserRole> UserRoles { get; set; }
        Task<int> SaveChangesAsync();
        DbEntityEntry<T> Entry<T>(T entity) where T : class;
    }
}