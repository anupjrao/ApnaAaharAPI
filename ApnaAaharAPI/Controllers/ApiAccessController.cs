using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApnaAahar.Services;
using ApnaAaharAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ApnaAaharAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAccessController : ControllerBase
    {
        public IAuthenticationService _Authentication { get; }
        private readonly IConfiguration _Configuration;
        public ApiAccessController(IConfiguration Configuration, IAuthenticationService authenticationServices)
        {
            _Configuration = Configuration;
            _Authentication = authenticationServices;
        }

        /// <summary>
        /// method to get authorize token to acess api as a vistor
        /// </summary>
        /// <param name="generalUser"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost("AuthorizeApi")]
        public async Task<IActionResult> GetAcessOfApi([FromBody] GeneralUser generalUser)
        {
            if (generalUser.UserId == "hdhhdd")
            {
                string AuthToken = await _Authentication.AuthenticateVisitor();
                return Ok(new { Token = AuthToken });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { Message = "User Not Found" });
            }
        }
    }
}
