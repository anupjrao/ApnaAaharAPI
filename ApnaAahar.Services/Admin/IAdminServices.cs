using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Services.Admin
{
    public interface IAdminServices
    {
        /// <summary>
        /// Request repository to get list of registered farmers from User database
        /// </summary>
        /// <returns>List of Users</returns>
        Task<IEnumerable<Users>> GetRegisteredFarmers();
        /// <summary>
        /// Reqest repository to get list of requesting farmers from User database
        /// </summary>
        /// <returns>List of Users</returns>
        Task<IEnumerable<Users>> GetRequestingFarmers();
        /// <summary>
        /// Request repository to disable regitered user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>boolean</returns>
        Task<bool> DisableUser(Users user);
        /// <summary>
        /// Request repository to Accept request of user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>boolean</returns>
        Task<bool> AcceptRequest(Users user);
        /// <summary>
        /// Request repository to decline request of user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>boolean</returns>
        Task<bool> DeclineRequest(Users user);
        /// <summary>
        /// returns all the product types
        /// </summary>
        /// <returns></returns>
        Task<List<ProductType>> GetProductTypes();
        /// <summary>
        /// Updates the msp of particular product type
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        Task<bool> UpdateProductMsp(ProductType productType);
    }
}
