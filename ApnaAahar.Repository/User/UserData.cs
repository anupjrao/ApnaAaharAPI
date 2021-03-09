using ApnaAahar.Exceptions;
using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ApnaAahar.Repository
{
    public class UserData : IUserData
    {
        private readonly Orchard1Context _ApnaAaharContext;
        public UserData(Orchard1Context ApnaAaharContext)
        {
            _ApnaAaharContext = ApnaAaharContext;
        }

       

        public async Task<int> UpdateUserLocation(UpdateModel data)
        {
            try
            {
                if (data.Location != null)
                {
                    Users user = await _ApnaAaharContext.Users.FindAsync(data.UserId);
                    if (user != null)
                        user.Location = data.Location.Trim();
                }
                return await _ApnaAaharContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new DbContextException("DataBase exception Occured", ex);
            }
            catch (Exception ex)
            {
                throw new GeneralException("Non DataBase exception Occured", ex);
            }
        }

        public async Task<bool> UpdateUserPassword(UpdateModel data)
        {
            try
            {
                Users user = await _ApnaAaharContext.Users.FindAsync(data.UserId);
                if (user != null)
                {
                    if (user.Password.Trim() == data.CurrentPassword.Trim())
                    {
                        user.Password = data.NewPassword;
                    }
                    else
                        return false;
                }
                int i = await _ApnaAaharContext.SaveChangesAsync();
                if (i > 0)
                    return true;
                return false;
            }
            catch(DbUpdateException ex)
            {
                throw new DbContextException("DataBase exception Occured",ex);
            }
            catch(Exception ex)
            {
                throw new GeneralException("Non DataBase exception Occured", ex);
            }
        }

        public async Task<bool> AddUserRegistrationDetails(Users user)
        {
            int isUserDataSaved = 0;
            try
            {
                await _ApnaAaharContext.AddAsync(user);
                isUserDataSaved = await _ApnaAaharContext.SaveChangesAsync();
                if (isUserDataSaved == 0)
                {
                    return false;
                }
            }
            catch (DbUpdateException ex)
            {
                throw new DataNotSavedException("Failed to save details", ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return true;
        }

        /// <summary>
        /// repo method to get User from database based on their Phone No
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Users> GetUserByPhoneNo(Users user)
        {

            try
            {
                Users userFound =await _ApnaAaharContext.Users.Include(users => users.FarmerDetails).FirstOrDefaultAsync(userFirst => userFirst.PhoneNumber == user.PhoneNumber);
                return userFound;
            }
            catch (SqlException)
            {
                throw new AnySqlException("Internal error Occured!");
            }
        }

        /// <summary>
        /// repo method to get User from database based on their email id
        /// </summary>
        /// <returns></returns>
        public async Task<Users> GetUsersByEmail(Users user)
        {
            try
            {
                Users userFound= await _ApnaAaharContext.Users.Include(users=>users.FarmerDetails).FirstOrDefaultAsync(userFirst => userFirst.Email == user.Email);
                return userFound;
            }
            catch(SqlException)
            {
                throw new AnySqlException("Internal Error Occured!");
            }


        }

        /// <summary>
        /// this method to reset password of user account
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> ResetPassword(Users user)
        {
            int i = 0;
            try
            {
                Users userLocal;
                if (user.Email.Length != 0)
                {
                    userLocal = await _ApnaAaharContext.Users.FirstOrDefaultAsync(userFound => userFound.Email == user.Email);
                }
                else
                {
                    userLocal = await _ApnaAaharContext.Users.FirstOrDefaultAsync(userFound => userFound.PhoneNumber == user.PhoneNumber);
                }
                userLocal.Password = user.Password;
                i = await _ApnaAaharContext.SaveChangesAsync();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new AnySqlException("sql error occured!", ex);
            }
        }

        public async Task<bool> AddContactRequest(ProductListingData productListing, Users user)
        {
            try
            {
                ContactRequest contactRequest = await (from req in _ApnaAaharContext.ContactRequest where req.ProductListingId == productListing.ProductListingId && req.BuyerId == user.UserId select req).FirstOrDefaultAsync();
                if(contactRequest==null)
                {
                    await _ApnaAaharContext.ContactRequest.AddAsync(new ContactRequest() { BuyerId = user.UserId, ProductListingId = productListing.ProductListingId});
                }
            }
            catch (Exception)
            {
                throw new DataNotSavedException();
            }
            int results = await _ApnaAaharContext.SaveChangesAsync();
            return (results > 0) ? true : false;
        }

        public async Task<IEnumerable<ContactRequest>> GetBuyerContactRequests(int buyerId)
        {
            try
            {
                return await _ApnaAaharContext.ContactRequest.Where(contactRequest => contactRequest.BuyerId == buyerId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
