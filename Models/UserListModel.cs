using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace INTEX.Models
{
    public class UserListModel
    {
        public List<IdentityUser> Users { get; set; }
    }
}

