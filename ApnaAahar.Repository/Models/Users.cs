using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApnaAahar.Repository.Models
{
    public partial class Users
    {
        public Users()
        {
            ContactRequest = new HashSet<ContactRequest>();
            FarmerDetails = new HashSet<FarmerDetails>();
            Otp = new HashSet<Otp>();
        }

        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Password { get; set; }
        public int UserRole { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public string AuthToken { get; set; }
        public UserRole UserRoleNavigation { get; set; }
        public ICollection<ContactRequest> ContactRequest { get; set; }
        public ICollection<FarmerDetails> FarmerDetails { get; set; }
        public ICollection<Otp> Otp { get; set; }

        public static implicit operator List<object>(Users v)
        {
            throw new NotImplementedException();
        }
    }
}
