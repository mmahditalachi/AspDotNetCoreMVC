using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    public class SQLUsersRepository : IUsersRepository
    {

        private readonly AppDbContext context;

        public SQLUsersRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Users Add(Users user)
        {
            context._User.Add(user);
            context.SaveChanges();
            return user;
        }

        public Users Delete(Guid UserID)
        {
            Users user = context._User.Find(UserID);
            if (user != null)
            {
                context._User.Remove(user);
                context.SaveChanges();
            }
            return user;
        }

        public IEnumerable<Users> GetAllUsers()
        {
            return context._User;
        }

        public Users GetUser(Guid UserID)
        {
            return context._User.Find(UserID);
        }

        public Users Update(Users user)
        {
            var changedUser = context._User.Attach(user);
            changedUser.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return user;

        }
    }
}
