using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dashboard.ViewModels
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Claim = new List<UserClaim>();
        }

        public string UserId { get; set; }
        public List<UserClaim> Claim { get; set; }
    }
}
