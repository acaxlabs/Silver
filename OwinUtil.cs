using System;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Silver
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
            app.CreatePerOwinContext<IUserStore<TUser, string>>((IdentityFactoryOptions<IUserStore<TUser, string>> options, IOwinContext context) => 
            {
                return new UserStoreCore<TUser>(new TUserStore());
            });
            app.CreatePerOwinContext<UserManager<TUser, string>>((IdentityFactoryOptions<UserManager<TUser, string>> options, IOwinContext context) => 
            {
                return new UserManager<TUser, string>(context.Get<IUserStore<TUser, string>>());
            });
            app.CreatePerOwinContext<SignInManager<TUser, string>>((IdentityFactoryOptions<SignInManager<TUser, string>> options, IOwinContext context) => 
            {
                return new SignInManager<TUser, string>(context.GetUserManager<UserManager<TUser, string>>(), context.Authentication);
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
        public static SignInManager<TUser, string> GetSignInManager<TUser>() where TUser : class, IAppUser
        {
            return HttpContext.Current.GetOwinContext().Get<SignInManager<TUser, string>>();
        }

        /// <summary>
        /// Fetch the UserManager from the Owin context.
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <returns></returns>
        public static UserManager<TUser, string> GetUserManager<TUser>() where TUser : class, IAppUser
        {
            return HttpContext.Current.GetOwinContext().GetUserManager<UserManager<TUser, string>>();
        }
        /// <summary>
        /// Signins in the user and stores the json serialized userdata into ClaimTypes.UserData
        /// </summary>
        /// <param name="user"></param>
        public static void SignIn(dynamic user)
        {
            GetAuthenticationManager().SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", user.ProviderName),
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user))
            };
            identity.AddClaims(claims);
            foreach (var item in user.ClaimRoles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, item));
            }
            AuthenticationProperties props = new AuthenticationProperties()
            {
                IsPersistent = false
            };
            GetAuthenticationManager().SignIn(props, identity);

        }
    }
}
