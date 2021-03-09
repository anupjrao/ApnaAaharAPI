using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ApnaAahar.Exceptions;
using ApnaAahar.Repository.Models;
using ApnaAahar.Services;
using ApnaAaharAPI.ExternalApiServices;
using ApnaAaharAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApnaAaharAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmerController : ControllerBase
    {
        private readonly IFarmerServices _farmerServices;

        public FarmerController(IFarmerServices farmerServices)
        {
            _farmerServices = farmerServices;
        }

        /// <summary>
        /// Post method to add new farmers to database
        /// </summary>
        /// <param name="farmer"></param>
        /// <returns></returns>
        [HttpPost("AddFarmer")]

        public async Task<ResponseModel> AddFarmerRegistrationDetails(FarmerDetails farmer)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel.ResponseMessage = await _farmerServices.AddFarmerRegistrationDetails(farmer);
               
            }
            catch (DataNotSavedException ex)
            {
                responseModel.ResponseMessage = ex.Message;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return responseModel;
        }
        /// <summary>
        /// Returns a list of contact requests based on user id
        /// </summary>
        /// <param name="userId">The user id by which contact requests are returend</param>
        /// <returns>A list of contact requests</returns>
        [HttpGet("GetBuyers/{userId}")]
        [Authorize(Roles = "1")]
        public async Task<List<ContactRequest>> GetBuyers(int userId)
        {
            try
            {
                return await _farmerServices.GetBuyers(userId);
            }
            catch(SqlException ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Sends a response to buyers and farmers depending on the buyerResponse model's isAccepted field
        /// </summary>
        /// <param name="buyerResponse"></param>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        [HttpPost("SendFarmerResponse")]
        public async Task<IActionResult> SendFarmerResponse(BuyerResponse buyerResponse)
        {
            FarmerDetailsSendingAndGenerating sendingResponse = new FarmerDetailsSendingAndGenerating(_farmerServices);
            try
            {
               bool response = await sendingResponse.SendResponseToEmailAsync(buyerResponse);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw e;
            }

        }


    }
}
