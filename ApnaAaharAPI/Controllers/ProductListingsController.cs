using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Authorization Roles:
    /// 0 - Admin
    /// 1 - Farmer
    /// 2 - Buyer
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductListingsController : ControllerBase
    {
        IProductServices _IProductServices;
        public ProductListingsController(IProductServices IProductServices)
        {
            _IProductServices = IProductServices;
        }

        /// <summary>
        /// Get method to get all productListings returned from service layer
        /// </summary>
        /// <returns>ActionResult with product listing data as list</returns>
        [Authorize(Roles ="0,1,2,Visitor")]
        [HttpGet("GetListings")]
        public async Task<ActionResult<IEnumerable<ProductListingData>>> GetListings()
        {
            try
            {
                return Ok(await _IProductServices.GetProductListings());
            }
            catch(GeneralException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetListings/{filter}")]
        public async Task<ActionResult<IEnumerable<ProductListingData>>> GetListings(string filter)
        {
            try
            {
                return Ok(await _IProductServices.GetProductListings(filter));
            }
            catch(GeneralException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get listings by farmer details taken from user object
        /// </summary>
        /// <param name="user"></param>
        /// <returns>ActionResult with product listings as list</returns>
        [Authorize(Roles ="1")]
        [HttpPost("GetListingsByFarmer")]
        public async Task<ActionResult<IEnumerable<ProductListingData>>> GetListings(Users user)
        {
            try
            {
                return Ok(await _IProductServices.GetProductListingsByFarmer(user));
            }
            catch (GeneralException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Method to add prdouctListings
        /// </summary>
        /// <param name="productListingData"></param>
        /// <returns></returns>
        [Authorize(Roles ="1")]
        [HttpPost("AddProductListings")]
        public async Task<ActionResult> AddProductListings(ProductListingData productListingData)
        {
            try
            {
                return Ok(await _IProductServices.AddProductListings(productListingData));
            }
            catch(AnySqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (GeneralException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Internal Error Occured!");
            }
        }

        /// <summary>
        /// Method to get all type of ProductTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllProductTypes")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult> GetAllProductTypes()
        {
            try
            {
                return Ok(await _IProductServices.GetAllProductTypes());
            }
            catch (AnySqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (GeneralException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Internal Error Occured!");
            }
        }

        /// <summary>
        /// Update method to update product Listings data
        /// </summary>
        /// <param name="productListingData"></param>
        /// <returns>ActionResult</returns>
        [Authorize(Roles = "1")]
        [HttpPut("UpdateProductListings")]
        public async Task<ActionResult> UpdateProductListings(ProductListingData productListingData)
        {
            try
            {
                return Ok(await _IProductServices.UpdateProductListings(productListingData));
            }
            catch (AnySqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (GeneralException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Internal Error Occured!");
            }
        }
    }
}
