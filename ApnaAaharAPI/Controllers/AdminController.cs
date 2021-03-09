using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApnaAahar.Exceptions;
using ApnaAahar.Repository.Models;
using ApnaAahar.Services.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace ApnaAaharAPI.Controllers
{
    /// <summary>
    /// Authorization Roles:
    /// 0 - Admin
    /// 1 - Farmer
    /// 2 - Buyer
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;

        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }
        /// <summary>
        /// Method to get list of registered Users
        /// </summary>
        /// <returns>IEnumerable<Users></returns>
        [Authorize(Roles = "0")]
        [HttpGet("GetRegisteredFarmers")]
        public async Task<IActionResult> GetRegisteredFarmers()
        {
            try
            {
                return Ok(await _adminServices.GetRegisteredFarmers());
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Method to get list of requesting Users
        /// </summary>
        /// <returns>IEnumerable<Users></returns>
        [Authorize(Roles = "0")]
        [HttpGet("GetRequestingFarmers")]
        public async Task<IActionResult> GetRequestingFarmers()
        {
            try
            {
                return Ok(await _adminServices.GetRequestingFarmers());
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        /// <summary>
        /// Method to disable registered user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize(Roles = "0")]
        [HttpPost("DisableUser")]
        public async Task<IActionResult> DisableUser(Users user)
        {
            try
            {
                return Ok(await _adminServices.DisableUser(user));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Method to Accept request of registering Users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize(Roles = "0")]
        [HttpPost("AcceptRequest")]
        public async Task<IActionResult> AcceptRequest(Users user)
        {
            try
            {
                return Ok(await _adminServices.AcceptRequest(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Method to decline request of registering users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize(Roles = "0")]
        [HttpPost("DeclineRequest")]
        public async Task<IActionResult> DeclineRequest(Users user)
        {
            try
            {
                return Ok(await _adminServices.DeclineRequest(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Returns all product types
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "0")]
        [HttpGet]
        [Route("GetProductTypes")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            try
            {
                return Ok(await _adminServices.GetProductTypes());
            }
            catch (GeneralException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates msp of particular product type
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        [Authorize(Roles = "0")]
        [HttpPost]
        [Route("UpdateMsp")]
        public async Task<ActionResult> UpdateProductMsp(ProductType productType)
        {
            try
            {
                return Ok(await _adminServices.UpdateProductMsp(productType));
            }
            catch (DataNotSavedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
