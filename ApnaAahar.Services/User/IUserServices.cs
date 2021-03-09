using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Services
{
    public interface IUserServices
    {
        /// <summary>
        /// Registers a new user as a buyer
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>      
        Task<bool> AddUserRegistrationDetails(Users user);
        /// <summary>
        /// requests repository for Updating user Location for User based on user Id 
        /// </summary>
        /// <param type="UpdateModel"></param>
        /// <returns>integer</returns>
        Task<int> UpdateUserLocation(UpdateModel dataCarrier);
        /// <summary>
        /// requests repository for Updating User Password for User based on user Id 
        /// </summary>
        /// <param type="UpdateModel"></param>
        /// <returns>boolean</returns>
        Task<bool> UpdateUserPassword(UpdateModel dataCarrier);
        Task<Users> GetUsersByEmail(Users user);
        Task<Users> GetUserByPhoneNo(Users user);
        Task<bool> ResetPassword(Users user);
        /// <summary>
        /// Handles adding contact request for a particular user
        /// </summary>
        /// <param name="productListing"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> AddContactRequest(ProductListingData productListing,Users user);
        /// <summary>
        /// Handles getting contact requests for a buyer from the repository
        /// </summary>
        /// <param name="buyerId">Buyer id of which contact requests are returned</param>
        /// <returns></returns>
        Task<IEnumerable<ContactRequest>> GetBuyerContactRequests(int buyerId);
    }

    
}
