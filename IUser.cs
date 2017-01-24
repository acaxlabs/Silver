using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silver
{
    public interface IUser : Microsoft.AspNet.Identity.IUser<Guid>
    {
        string ProviderName { get; set; }
        string ProviderKey { get; set; }
        string Email { get; set; }
    }
}
