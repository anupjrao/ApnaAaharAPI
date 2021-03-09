using ApnaAahar.Repository;
using ApnaAahar.Repository.Models;
using ApnaAahar.Services.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Services
{
    public class AuthenticationService : IAuthenticationService
    {


        private readonly IConfiguration _Configuration;
        private readonly IUserData _UserData;
     
      
        public AuthenticationService(IConfiguration Configuration, IUserData UserData)
        {
            _Configuration = Configuration;
            _UserData = UserData;
   

        }
      
        public Encryption encryption = new Encryption();

        /// <summary>
        /// Authenticating the user and generating jwt token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Users> Authenticate(Users user)
        {
            string encryptedPassword = encryption.EncryptingPassword(user.Password);
            try
            {
                Users userFound = await _UserData.GetUsersByEmail(user);
                if (userFound != null)
                {
                    if (encryptedPassword != userFound.Password)
                    {
                        return null;
                    }
                    else
                    {

                        JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
                        byte[] tokenKey = Encoding.ASCII.GetBytes(_Configuration.GetValue<string>("SecretKey"));
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Password),
                    new Claim(ClaimTypes.Role, $"{userFound.UserRole.ToString()}")
                }),
                            Expires = DateTime.UtcNow.AddHours(1),
                            SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = jwtHandler.CreateToken(tokenDescriptor);
                        userFound.Password = null;
                        userFound.AuthToken = jwtHandler.WriteToken(token);
                        return userFound;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> AuthenticateVisitor()
        {
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            byte[] tokenKey = Encoding.ASCII.GetBytes(_Configuration.GetValue<string>("SecretKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Role, "Visitor")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtHandler.CreateToken(tokenDescriptor);
            var AuthToken = jwtHandler.WriteToken(token);
            return await Task.FromResult(AuthToken);

        }

    }
}



