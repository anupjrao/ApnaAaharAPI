using ApnaAahar.Repository;
using ApnaAahar.Repository.Models;
using ApnaAahar.Services.Authentication;
using ApnaAahar.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserData _UserData;

        public UserServices(IUserData UserData)
        {
            _UserData = UserData;
        }

        
        public async Task<int>  UpdateUserLocation(UpdateModel dataCarrier)
        {
            return await _UserData.UpdateUserLocation(dataCarrier);
        }
        public async Task<bool> UpdateUserPassword(UpdateModel dataCarrier)
        {
            Encryption encryption = new Encryption();
            dataCarrier.CurrentPassword= encryption.EncryptingPassword(dataCarrier.CurrentPassword.Trim());
            dataCarrier.NewPassword= encryption.EncryptingPassword(dataCarrier.NewPassword.Trim());
            if (dataCarrier.CurrentPassword.Equals(dataCarrier.NewPassword))
                throw new IdenticalPasswordException("Passwords are Identical");                
            return await _UserData.UpdateUserPassword(dataCarrier);
        }
        public async Task<Users> GetUserByPhoneNo(Users user)
        {
            return await _UserData.GetUserByPhoneNo(user);
        }

        public async Task<Users> GetUsersByEmail(Users user)
        {
            return await _UserData.GetUsersByEmail(user);
        }

        public async Task<bool> ResetPassword(Users user)
        {
            return await _UserData.ResetPassword(user);

        }

        public async Task<bool> AddUserRegistrationDetails(Users user)
        {
            return await _UserData.AddUserRegistrationDetails(user);
        }

        public async Task<bool> AddContactRequest(ProductListingData productListing, Users user)
        {
            try
            {
                return await _UserData.AddContactRequest(productListing, user);
            }
            catch(DataNotSavedException)
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<IEnumerable<ContactRequest>> GetBuyerContactRequests(int buyerId)
        {
            try
            {
                return await _UserData.GetBuyerContactRequests(buyerId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
