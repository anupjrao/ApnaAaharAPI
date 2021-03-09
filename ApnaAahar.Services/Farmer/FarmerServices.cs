using ApnaAahar.Exceptions;
using ApnaAahar.Repository;
using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Services
{
    public class FarmerServices : IFarmerServices
    {
        private readonly IFarmerData _FarmerData;
        
        public FarmerServices(IFarmerData FarmerData)
        {
            _FarmerData = FarmerData;
        }

        public async Task<string> AddFarmerRegistrationDetails(FarmerDetails farmer)
        {
            try
            {
                return await _FarmerData.AddFarmerRegistrationDetails(farmer);
            }
            catch (DataNotSavedException ex)
            {
                throw new DataNotSavedException(ex.Message);
            }
        }

        public bool DeleteBuyerRequest(int contactRequestId)
        {
            try
            {
                return _FarmerData.DeleteContactRequests(contactRequestId);
            }
            catch(AnySqlException e)
            {
                throw new AnySqlException(e.Message);
            }
        }
        /// <summary>
        /// Goes to repository layer and fetches the contactRequest details depending on this userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        public async Task<List<ContactRequest>> GetBuyers(int userId)
        {
            try
            {
                return await _FarmerData.GetBuyers(userId);
            }
            catch (AnySqlException e)
            {
                throw new AnySqlException(e.Message);
            }
        }
        /// <summary>
        /// Goes to repository layer and returns the farmer with this user ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        public async Task<Users> GetFarmersDetailsByUserId(int userId)
        {
            try
            {
                return await _FarmerData.GetFarmerByUserId(userId);
            }
            catch (AnySqlException e)
            {
                throw new AnySqlException(e.Message);
            }
        }
    }
}
