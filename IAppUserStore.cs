using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silver
{
    public interface IAppUserStore<TUser> : IDisposable where TUser : class, IAppUser
    {
        void ModifyUser(TUser user);
        void AddToRole(TUser user, string roleName);
        void CreateUser(TUser user);
        void DeleteUser(TUser user);
        TUser FindByProvider(string loginProvider, string providerKey);
        TUser FindById(Guid userId);
        TUser FindByEmail(string email);
        IList<string> GetRoles(TUser user);
        bool IsInRole(TUser user, string roleName);
        void RemoveFromRole(TUser user, string roleName);
    }
}