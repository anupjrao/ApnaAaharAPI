using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Repository
{
    public interface IFarmerData
    {
        /// <summary>
        /// Adds a new farmer as an individual or community to the database
        /// </summary>
        /// <param name="farmer"></param>
        /// <returns></returns>
        Task<string> AddFarmerRegistrationDetails(FarmerDetails farmer);

        Task<Users> GetFarmerByUserId(int userId);
        Task<List<ContactRequest>> GetBuyers(int userId);
        bool DeleteContactRequests(int contactRequestId);
    }
}
