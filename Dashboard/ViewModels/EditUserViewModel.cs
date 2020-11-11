using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles = new List<string>();
            Claims = new List<string>();
        }

        [Required]
        public string Email { get; set; }
        public string City { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Id { get; set; }
        public IList<string> Roles { get; set; }
        public IList<string> Claims { get; set; }
    }
}
