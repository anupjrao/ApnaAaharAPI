using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApnaAahar.Exceptions;

namespace ApnaAahar.Repository
{
    public class BaseRepository : IDisposable
    {

        Orchard1Context _OrchardContext;
        public BaseRepository(Orchard1Context OrchardContext)
        {
            _OrchardContext = OrchardContext;
        }
        /// <summary>
        /// Common method to get Farmer details based on farmer id in productListing from database
        /// </summary>
        /// <param name="productListing"></param>
        /// <returns></returns>
        public async Task<FarmerDetails> GetFarmerDetailsByProductListing(ProductListingData productListing)
        {
            FarmerDetails farmer = new FarmerDetails();
            Users user = new Users();
            try
            {
                farmer = await (Task<FarmerDetails>)(from farmerDetails in _OrchardContext.FarmerDetails
                                                     join userData in _OrchardContext.Users on farmerDetails.UserId equals userData.UserId
                                                     where farmerDetails.FarmerId == productListing.FarmerId
                                                     select farmerDetails).FirstAsync();
                user = await (Task<Users>)(from farmerDetails in _OrchardContext.FarmerDetails
                                           join userDetails in _OrchardContext.Users on farmerDetails.UserId equals userDetails.UserId
                                           where farmerDetails.FarmerId == productListing.FarmerId
                                           select userDetails).FirstAsync();
            }
            catch (Exception)
            {
                throw new GeneralException("Something went wrong");
            }
            farmer.User = new Users() { UserFullName = user.UserFullName, Location = user.Location};
            return farmer;
        }
        /// <summary>
        /// Common method to get ProductType details based on producttype id in productListing from database
        /// </summary>
        /// <param name="productListing"></param>
        /// <returns></returns>
        public async Task<ProductType> GetProductTypeByProductListing(ProductListingData productListing)
        {
            ProductType productType = new ProductType();
            try
            {
                productType = await (Task<ProductType>)(from productTypeData in _OrchardContext.ProductType
                                                        where productTypeData.ProductTypeId == productListing.ProductTypeId
                                                        select productTypeData).FirstAsync();
            }
            catch (Exception)
            {
                throw new GeneralException("Something went wrong");
            }
            
            return productType;
        }
        /// <summary>
        /// Common method to get User Details based on EmailId in Users from database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Users> GetUserDetailsByEmail(Users user)
        {
            Users userDetails = new Users();
            try
            {
               userDetails = await (Task<Users>)(from userData in _OrchardContext.Users
                                                  where userData.Email == user.Email
                                                  select userData).FirstAsync();
            }
            catch (Exception)
            {
                throw new GeneralException("Something went wrong");
            }

            return userDetails;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
