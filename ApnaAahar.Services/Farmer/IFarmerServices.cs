using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Services
{
    public interface IFarmerServices
    {
        /// <summary>
        /// Registers a new farmer as an individual or community
        /// </summary>
        /// <param name="farmer"></param>
        /// <returns></returns>
        Task<string> AddFarmerRegistrationDetails(FarmerDetails farmer);
        Task<List<ContactRequest>> GetBuyers(int userId);

        Task<Users> GetFarmersDetailsByUserId(int userId);
        bool DeleteBuyerRequest(int contactRequestId);
    }
}
