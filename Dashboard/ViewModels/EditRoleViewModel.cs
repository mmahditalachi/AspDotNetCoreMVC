using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            _Users = new List<string>();
        }
        public string Id { get; set; }

        [Required(ErrorMessage = "RoleName is required")]
        public string RoleName { get; set; }

        public List<string> _Users { get; set; }
    }
}
