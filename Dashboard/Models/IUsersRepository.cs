using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    interface IUsersRepository
    {
        Users GetUser(Guid UserID);
        IEnumerable<Users> GetAllUsers();
        Users Add(Users user);
        Users Update(Users user);
        Users Delete(Guid UserID);
    }
}
