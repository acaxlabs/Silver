﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Host;
using Sherlock.Web.Models;
using Microsoft.Owin;
using Owin;
using Silver;

namespace Sherlock.Web.Models
{
    public class OwinUtil
    {
        /// <summary>
        /// Use the setup method in your App_Start/Startup.Auth class to get the UserStore, UserManager, and SignInManager configured in Owin.
        /// </summary>
        /// <typeparam name="TUser">User model implementing IAppUser</typeparam>
        /// <typeparam name="TUserStore">UserStore implementing IAppUserStore</typeparam>
        /// <param name="app"></param>
        public static void Setup<TUser, TUserStore>(IAppBuilder app) where TUser : class, IAppUser where TUserStore : IAppUserStore<TUser>, new()
        {
            app.CreatePerOwinContext<IUserStore<TUser, Guid>>((IdentityFactoryOptions<IUserStore<TUser, Guid>> options, IOwinContext context) => 
            {
                return new UserStoreCore<TUser>(new TUserStore());
            });
            app.CreatePerOwinContext<UserManager<TUser, Guid>>((IdentityFactoryOptions<UserManager<TUser, Guid>> options, IOwinContext context) => 
            {
                return new UserManager<TUser, Guid>(context.Get<IUserStore<TUser, Guid>>());
            });
            app.CreatePerOwinContext<SignInManager<TUser, Guid>>((IdentityFactoryOptions<SignInManager<TUser, Guid>> options, IOwinContext context) => 
            {
                return new SignInManager<TUser, Guid>(context.GetUserManager<UserManager<TUser, Guid>>(), context.Authentication);
            });
        }

        /// <summary>
        /// Fetch the AuthenticationManager from the Owin context.
        /// </summary>
        /// <returns></returns>
        public static IAuthenticationManager GetAuthenticationManager()
        {
            return HttpContext.Current.GetOwinContext().Authentication;
        }
        
        /// <summary>
        /// Fetch the SignInManager from the Owin context.
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <returns></returns>
        public static SignInManager<TUser, Guid> GetSignInManager<TUser>() where TUser : class, IAppUser
        {
            return HttpContext.Current.GetOwinContext().Get<SignInManager<TUser, Guid>>();
        }

        /// <summary>
        /// Fetch the UserManager from the Owin context.
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <returns></returns>
        public static UserManager<TUser, Guid> GetUserManager<TUser>() where TUser : class, IAppUser
        {
            return HttpContext.Current.GetOwinContext().GetUserManager<UserManager<TUser, Guid>>();
        }
    }
}