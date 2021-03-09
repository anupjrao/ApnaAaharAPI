using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Services
{
    public interface IAuthenticationService
    {
        Task<Users> Authenticate(Users user);
        Task<string> AuthenticateVisitor();
    }
}
