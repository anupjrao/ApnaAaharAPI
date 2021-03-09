using ApnaAahar.Exceptions;
using ApnaAahar.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ApnaAahar.Repository
{
    public class FarmerData : IFarmerData
    {
        private readonly Orchard1Context _Orchard1Context;
        public FarmerData(Orchard1Context Orchard1Context)
        {
            _Orchard1Context = Orchard1Context;
        }

        public async Task<string> AddFarmerRegistrationDetails(FarmerDetails farmer)
        {
            Users userDataForEmailCheck = await _Orchard1Context.Users.FirstOrDefaultAsync(user => user.Email == farmer.User.Email);
            Users userDataForMobileNumberCheck = await _Orchard1Context.Users.FirstOrDefaultAsync(userData => userData.PhoneNumber == farmer.User.PhoneNumber);
            if (userDataForEmailCheck == null && userDataForMobileNumberCheck == null)
            {
                FarmerDetails farmerDetails = await _Orchard1Context.FarmerDetails.FirstOrDefaultAsync(farmerData => farmerData.FarmerId == farmer.FarmerId);
                if (farmer.Community == null && farmerDetails == null)
                {
                    try
                    {
                        Users user = farmer.User;
                        await _Orchard1Context.AddAsync(user);
                        
                        farmer.UserId = user.UserId;
                        await _Orchard1Context.AddAsync(farmer);
                        
                    }
                    catch (SqlException ex)
                    {
                        throw new DataNotSavedException("Data not saved", ex);
                    }
                }
                else if (farmer.Community != null && farmerDetails == null)
                {
                    CommunityDetails communityDetails = await _Orchard1Context.CommunityDetails.FirstOrDefaultAsync(community => community.CommunityName == farmer.Community.CommunityName);
                    if (communityDetails == null)
                    {
                        try
                        {
                            Users userData = farmer.User;
                            await _Orchard1Context.AddAsync(userData);
                            
                            CommunityDetails communityData = farmer.Community;
                            await _Orchard1Context.AddAsync(communityData);
                            
                            farmer.UserId = userData.UserId;
                            farmer.CommunityId = communityData.CommunityId;
                            await _Orchard1Context.AddAsync(farmer);
                            

                        }
                        catch (SqlException ex)
                        {
                            throw new DataNotSavedException("Data not saved", ex);
                        }
                    }
                    else
                    {
                        
                        int isFarmerAdded = await _Orchard1Context.SaveChangesAsync();
                        if (isFarmerAdded == 0)
                        {
                            throw new DataNotSavedException("Community Name Already Exists");
                        }
                    }
                }
                else if (farmerDetails != null)
                {
                    
                    int isFarmerAdded = await _Orchard1Context.SaveChangesAsync();
                    if (isFarmerAdded == 0)
                    {
                        throw new DataNotSavedException("FarmerId already exists");
                    }
                }
            }
            else
            {
                int isFarmerAdded = await _Orchard1Context.SaveChangesAsync();
                if (isFarmerAdded == 0)
                {
                    throw new DataNotSavedException("Duplication");
                }
                
            }
            int isDataAdded = await _Orchard1Context.SaveChangesAsync();
            if (isDataAdded > 0)
            {
                return "Successfull";
            }
            else
            {
                throw new DataNotSavedException("Duplication");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactRequestId"></param>
        /// <returns></returns>
        public bool DeleteContactRequests(int contactRequestId)
        {
            try
            {
             _Orchard1Context.ContactRequest.Remove(_Orchard1Context.ContactRequest.Find(contactRequestId));
             
            }
            catch(Exception e)
            {
                throw new AnySqlException("Something Went Wrong",e);
            }
            int deleted = _Orchard1Context.SaveChanges();

            return (deleted > 0) ? true : false;
        }

        /// <summary>
        /// Returns the list of contactRequest with ProductListingData and buyers data
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ContactRequest>> GetBuyers(int userId)
        {
            try
            {
                int farmerId = (await _Orchard1Context.FarmerDetails.FirstOrDefaultAsync(farmer => farmer.UserId == userId)).FarmerId;

                List<ProductListingData> productTypes = await (Task<List<ProductListingData>>)(from productListing in _Orchard1Context.ProductListingData
                                                                                               join productType in _Orchard1Context.ProductType on productListing.ProductType equals productType
                                                                                               where productListing.FarmerId == farmerId
                                                                                               select productListing).ToListAsync();
                foreach (ProductListingData productLists in productTypes)
                {

                    productLists.ProductType = new ProductType();
                    productLists.ProductType = (ProductType)(from productType in _Orchard1Context.ProductType
                                                  join productList in productTypes
                                                  on productType.ProductTypeId equals productList.ProductTypeId
                                                  select productType).First();

                }

                List<ContactRequest> contactRequests = await (Task<List<ContactRequest>>)(from contacts in _Orchard1Context.ContactRequest
                                                                                          join products in productTypes on contacts.ProductListingId equals products.ProductListingId
                                                                                          select contacts
                                                                                          ).Include(buyers => buyers.Buyer).Include(productType => productType.ProductListing.ProductType).AsNoTracking().ToListAsync();

                foreach(ContactRequest contactRequest in contactRequests)
                {
                    contactRequest.Buyer.Password = null;
                }
                return contactRequests;



            }
            catch (SqlException)
            {
                throw new AnySqlException("Something Went Wrong");
            }
            catch(Exception)
            {
                throw new Exception();
            }
         
        }
        /// <summary>
        /// Finds the farmer with this userId and returns it
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Users</returns>
        public async Task<Users> GetFarmerByUserId(int userId)
        {
            try
            {
                return await _Orchard1Context.Users.FirstOrDefaultAsync(user => user.UserId == userId);

            }
            catch (SqlException)
            {
                throw new AnySqlException("Something went wrong");
            }
        }
    }
}
