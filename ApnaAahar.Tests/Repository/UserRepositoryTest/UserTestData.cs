using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApnaAahar.Tests.Repository.UserRepositoryTest
{
    public class UserTestData
    {
        public List<Users> GetUsers()
        {
            return new List<Users>
            {
                new Users
            {
                UserId=9,
                UserFullName = "John",
                PhoneNumber = "9879856787",
                Email = "john1234@gmail.com",
                Location = "Hyderabad",
                Password = "3460a0fa36ba463ea5383717841e045a",
                UserRole = 2
            },
            new Users
            {
                UserId=10,
                UserFullName = "Ashok",
                PhoneNumber = "9879868412",
                Email = "ashok123@gmail.com",
                Location = "Delhi",
                Password = "d00f5d5217896fb7fd601412cb890830",
                UserRole = 2
            },
            new Users
            {
                UserId=11,
                UserFullName = "Amar",
                PhoneNumber = "1234567890",
                Email = "amar123@gmail.com",
                Location = "kolkata",
                Password = "bb68b7232bb4fcba4cd6bd26b29b544f",
                UserRole = 2
            }
        };
        }
    }
}
