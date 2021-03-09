using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Repository
{
    public interface IUserData
    {
        /// <summary>
        /// Update User location for User based on user Id 
        /// </summary>
        /// <param type="UpdateModel"></param>
        /// <returns>integer</returns>
        Task<int> UpdateUserLocation(UpdateModel dataCarrier);
        /// <summary>
        /// Update User Password for User based on user Id 
        /// </summary>
        /// <param type="UpdateModel"></param>
        /// <returns>boolean</returns>
        Task<bool> UpdateUserPassword(UpdateModel dataCarrier);

        /// <summary>
        /// Adds new user to the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> AddUserRegistrationDetails(Users user);
        Task<Users> GetUsersByEmail(Users user);

        Task<Users> GetUserByPhoneNo(Users user);

        Task<bool> ResetPassword(Users user);
        /// <summary>
        /// Adds contact request to db context for the user parameter
        /// </summary>
        /// <param name="productListing"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> AddContactRequest(ProductListingData productListing, Users user);
        /// <summary>
        /// Returns an IEnumerable of Contact Requests for a particular buyer ID.
        /// </summary>
        /// <param name="buyerId">Buyer id of which contact requests are returned</param>
        /// <returns></returns>
        Task<IEnumerable<ContactRequest>> GetBuyerContactRequests(int buyerId);
    }
}
