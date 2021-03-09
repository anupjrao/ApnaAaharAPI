using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApnaAahar.Repository.Models;

namespace ApnaAahar.Repository.Admin
{
    public interface IAdminData
    {
        /// <summary>
        /// Get list of registered farmers from User database
        /// </summary>
        /// <returns>List of Users</returns>
        Task<IEnumerable<Users>> GetRegisteredFarmers();
        /// <summary>
        /// Get list of requesting farmers from User database
        /// </summary>
        /// <returns>List of Users</returns>
        Task<IEnumerable<Users>> GetRequestingFarmers();
        /// <summary>
        /// Disable regitered user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>boolean</returns>
        Task<bool> DisableUser(Users user);
        /// <summary>
        /// Accept request of user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>boolean</returns>
        Task<bool> AcceptRequest(Users user);
        /// <summary>
        /// Decline request of user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>boolean</returns>
        Task<bool> DeclineRequest(Users user);
        /// <summary>
        /// Returns all the product types from the database
        /// </summary>
        /// <returns></returns>
        Task<List<ProductType>> GetProductTypes();
        /// <summary>
        /// Updates msp of particular product type
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        Task<bool> UpdateProductMsp(ProductType productType);
    }
}
