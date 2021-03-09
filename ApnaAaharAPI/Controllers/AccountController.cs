using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApnaAahar.Repository;
using ApnaAahar.Repository.Models;
using ApnaAahar.Services;
using ApnaAaharAPI.ExternalApiServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApnaAaharAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        IAuthenticationService _AuthenticationService;
        private readonly IUserServices _IUserServices;

        public AccountController(IAuthenticationService authenticationService, IUserServices userServices)
        {
            _AuthenticationService = authenticationService;
            _IUserServices = userServices;
        }
        /// <summary>
        /// Post method to check if the user exists in database or not and sending back the user details with jwt token
        /// </summary>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] Users user)
        {
            Users token = await _AuthenticationService.Authenticate(user);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }


        /// <summary>
        /// A post method to send otp to user Email for reseting Password of the account
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("SendOtp")]
        public async Task<IActionResult> SendOtp([FromBody] Users user)
        {
            OtpSendingAndGenerating otpSending = new OtpSendingAndGenerating(_IUserServices);
            return Ok(await otpSending.sendOtp(user));
        }

        /// <summary>
        /// A post method to reset password of User account
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] Users user)
        {
            try
            {
                return Ok(await _IUserServices.ResetPassword(user));
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

    }
}
