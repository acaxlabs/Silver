using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silver
{
    public interface IUserRole
    {
        Guid Id { get; set; }
        Guid UserId { get; set; }
        string Role { get; set; }
    }
}
