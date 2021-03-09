using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ApnaAahar.Exceptions;
using ApnaAahar.Repository.Models;
using ApnaAahar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApnaAaharAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        /// <summary>
        /// Post method to add new users to database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("AddUser")]
        public async Task<bool> AddUserRegistrationDetails(Users user)
        {
            bool isUserValid;
            try
            {
                isUserValid = await _userServices.AddUserRegistrationDetails(user);
            }
            catch (DataNotSavedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUserValid;
        }
    }
}
