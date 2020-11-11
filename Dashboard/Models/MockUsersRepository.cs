using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    public class MockUsersRepository : IUsersRepository
    {

        private List<Users> _Users;
        public MockUsersRepository()
        {
            _Users = new List<Users>()
            {
                new Users(){UserID= new Guid("146b2aa8-3f53-4ab9-a552-c65be767499b"),Email="test@test.com",Password= "c8309721-8078-46d2-bc59-efff39cab224",Username="user1"},
                new Users(){UserID= new Guid("b89ac067-5c2f-47aa-9463-ac0cc29e2895"),Email="test2@test.com",Password= "5283b931-d372-4619-a713-6f65c3ae2e6f",Username="user2"}
            };
        }

        public Users Add(Users user)
        {
            user.UserID = new Guid();
            _Users.Add(user);
            return user;
        }

        public Users Delete(Guid UserID)
        {
            Users user = _Users.FirstOrDefault<Users>(e => e.UserID == UserID);
            if (user != null)
                _Users.Remove(user);

            return user;
        }

        public IEnumerable<Users> GetAllUsers()
        {
            return _Users;
        }

        public Users GetUser(Guid UserID)
        {
            return _Users.FirstOrDefault<Users>(e => e.UserID == UserID);
            
        }

        public Users Update(Users changedUser)
        {
            Users user = _Users.FirstOrDefault<Users>(e => e.UserID == changedUser.UserID);
            if (user != null)
            {
                user.Password = changedUser.Password;
                user.Username = changedUser.Username;
                user.Email = changedUser.Email;
            }

            return changedUser;
        }
    }
}
