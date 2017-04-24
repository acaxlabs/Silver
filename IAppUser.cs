using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silver
{
    public interface IAppUser : IUser<string>
    {
        string ProviderName { get; set; }
        string ProviderKey { get; set; }
        string Email { get; set; }
    }
}