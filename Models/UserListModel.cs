using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace INTEX.Models
{
    public class UserListModel
    {
        public List<IdentityUser> Users { get; set; }
        public Dictionary<string, string> UserRoles { get; set; }

        public UserListModel()
        {
            UserRoles = new Dictionary<string, string>();
        }
    }

}

