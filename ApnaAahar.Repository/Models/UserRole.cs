using System;
using System.Collections.Generic;

namespace ApnaAahar.Repository.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            Users = new HashSet<Users>();
        }

        public int TypeId { get; set; }
        public string UserRole1 { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
