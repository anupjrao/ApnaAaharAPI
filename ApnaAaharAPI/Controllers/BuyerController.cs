using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApnaAahar.Repository.Models;
using ApnaAahar.Services;
using Microsoft.AspNetCore.Authorization;
using ApnaAahar.Exceptions;
using ApnaAaharAPI.Model;
using ApnaAaharAPI.ExternalApiServices;

namespace ApnaAaharAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public BuyerController(IUserServices userServices)
        {
            this._userServices = userServices;
        }
        /// <summary>
        /// Method to update password of User account
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateUserPassword")]
        public async Task<IActionResult> UpdateUserPassword(UpdateModel dataCarrier)
        {
            try {
                return Ok(await _userServices.UpdateUserPassword(dataCarrier));
            }
            catch (DbContextException ex)
            {
                return NotFound(ex.Message);
            }
            catch (IdenticalPasswordException ex)
            {

                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Something went wrong");
            }

        }
        /// <summary>
        /// Method to update Location of User account
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateUserLocation")]
        public async Task<ActionResult> UpdateUserLocation(UpdateModel dataCarrier)
        {
            try {

                return Ok(await _userServices.UpdateUserLocation(dataCarrier));

            }
            catch (DbContextException ex)
            {
                return NotFound(ex.Message);
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }
        /// <summary>
        /// Returns a list of contact requests made by particular buyer
        /// </summary>
        /// <param name="buyerId">Buyer id of which contact requests are returned</param>
        /// <returns></returns>
        [Authorize(Roles ="2")]
        [HttpGet("GetBuyerContactRequests/{buyerId}")]
        public async Task<ActionResult<IEnumerable<ContactRequest>>> GetBuyerContactRequests(int buyerId)
        {
            try
            {
                return Ok(await _userServices.GetBuyerContactRequests(buyerId));
            }
            catch (Exception)
            {
                return BadRequest("Error getting contact request");
            }
        }
    }
}