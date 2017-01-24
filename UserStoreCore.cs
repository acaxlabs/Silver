using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Silver
{
    public class UserStoreCore<TUser> : IUserStore<TUser, Guid>, IUserLoginStore<TUser, Guid>, IUserRoleStore<TUser, Guid>
        where TUser : class, IAppUser 
    {
        IAppUserStore<TUser> store;

        public UserStoreCore(IAppUserStore<TUser> store)
        {
            this.store = store;
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            return Task.Factory.StartNew(() => 
            {
                user.ProviderName = login.LoginProvider;
                user.ProviderKey = login.ProviderKey;
                store.ModifyUser(user);
            });
        }

        public Task AddToRoleAsync(TUser user, string roleName)
        {
            return Task.Factory.StartNew(() => 
            {
                store.AddToRole(user, roleName);
            });
        }

        public Task CreateAsync(TUser user)
        {
            return Task.Factory.StartNew(() => 
            {
                store.CreateUser(user);
            });
        }

        public Task DeleteAsync(TUser user)
        {
            return Task.Factory.StartNew(() => 
            {
                store.DeleteUser(user);
            });
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            return Task.Factory.StartNew<TUser>(() =>
            {
                return store.FindByProvider(login.LoginProvider, login.ProviderKey);
            });
        }

        public Task<TUser> FindByIdAsync(Guid userId)
        {
            return Task.Factory.StartNew<TUser>(() =>
            {
                return store.FindById(userId);
            });
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            return Task.Factory.StartNew<TUser>(() =>
            {
                return store.FindByEmail(userName);
            });
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            return Task.Factory.StartNew<IList<UserLoginInfo>>(() => 
            {
                var info = new UserLoginInfo(user.ProviderName, user.ProviderKey);
                var list = new List<UserLoginInfo>();
                list.Add(info);
                return list;
            });
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            return Task.Factory.StartNew<IList<string>>(() => 
            {
                return store.GetRoles(user);
            });
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            return Task.Factory.StartNew<bool>(() => 
            {
                return store.IsInRole(user, roleName);
            });
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            return Task.Factory.StartNew(() => 
            {
                store.RemoveFromRole(user, roleName);
            });
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TUser user)
        {
            return Task.Factory.StartNew(() => 
            {
                store.ModifyUser(user);
            });
        }

        public void Dispose()
        {
            store.Dispose();
        }
    }
}