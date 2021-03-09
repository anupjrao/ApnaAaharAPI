using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApnaAahar.Services;
using ApnaAaharAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApnaAaharAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BuyerContactRequestController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public BuyerContactRequestController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        /// <summary>
        /// Contact request sent by buyer is validated
        /// </summary>
        /// <param name="contactRequest"></param>
        /// <returns></returns>
        [Authorize(Roles = "2")]
        //[AllowAnonymous]
        [HttpPost("AddContactRequest")]
        public async Task<bool> AddContactRequest([FromBody] BuyerContactRequest contactRequest)
        {
            return await _userServices.AddContactRequest(contactRequest.ProductListing, contactRequest.User);
        }
    }
}
