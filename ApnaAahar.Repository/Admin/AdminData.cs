using ApnaAahar.Exceptions;
using ApnaAahar.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace ApnaAahar.Repository.Admin
{
    public class AdminData : IAdminData
    {
        private readonly Orchard1Context _ApnaAaharContext;

        public AdminData(Orchard1Context ApnaAaharContext)
        {
            _ApnaAaharContext = ApnaAaharContext;
        }

        public async Task<IEnumerable<Users>> GetRegisteredFarmers()
        {
            try
            {
                List<Users> list = await (from user in _ApnaAaharContext.Users
                                          join farmer in _ApnaAaharContext.FarmerDetails
                                          on user.UserId equals farmer.UserId
                                          where farmer.IsApproved == true && farmer.IsAccountDisabled == false
                                          select user).ToListAsync();
                foreach(Users user in list)
                {
                    user.Password = null;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new GeneralException("Unable to get registered farmers", ex);
            }
        }

        public async Task<IEnumerable<Users>> GetRequestingFarmers()
        {
            try
            {
                List<Users> list = await (from user in _ApnaAaharContext.Users
                                          join farmer in _ApnaAaharContext.FarmerDetails
                                          on user.UserId equals farmer.UserId
                                          where user.IsDeleted == false && farmer.IsApproved == false && farmer.IsAccountDisabled==false
                                          select user
                                         ).ToListAsync();
                                          
                foreach(Users user in list)
                {
                    user.Password = null;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new GeneralException("Unable to get registered farmers", ex);
            }
        }

        public async Task<bool> DisableUser(Users user)
        {
            try
            {
                FarmerDetails farmer = await _ApnaAaharContext.FarmerDetails.FirstOrDefaultAsync(users=>users.UserId==user.UserId);
                farmer.IsAccountDisabled=true;
                farmer.IsApproved = false;
                if (_ApnaAaharContext.SaveChanges() > 0)
                    return true;
                return false;
            }
            catch(DbUpdateException ex)
            {
                throw new DbContextException("DataBase Exception occured",ex);
            }
        }

        public async Task<bool> AcceptRequest(Users user)
        {
            try
            {
                FarmerDetails farmer = await _ApnaAaharContext.FarmerDetails.FirstOrDefaultAsync(users => users.UserId == user.UserId);
                farmer.IsApproved = true;
                if (_ApnaAaharContext.SaveChanges() > 0)
                    return true;
                return false;
            }
            catch (DbUpdateException ex)
            {
                throw new DbContextException("DataBase Exception occured", ex);
            }

        }
        public async Task<bool> DeclineRequest(Users user)
        {
            try
            {
                Users FoundUser = await _ApnaAaharContext.Users.FindAsync(user.UserId);
                FoundUser.IsDeleted = true;
                if (_ApnaAaharContext.SaveChanges() > 0)
                    return true;
                return false;
            }
            catch (DbUpdateException ex)
            {
                throw new DbContextException("DataBase Exception occured", ex);
            }
        }

        public async Task<List<ProductType>> GetProductTypes()
        {
            List<ProductType> productTypes = new List<ProductType>();
            try
            {
               productTypes = await _ApnaAaharContext.ProductType.ToListAsync();
            }
            catch(Exception)
            {
                throw new GeneralException("Failed to fetch data");
            }
            return productTypes;
        }

        public async Task<bool> UpdateProductMsp(ProductType productType)
        {
            int isDataSaved = 0;
            ProductType productData = await _ApnaAaharContext.ProductType.FindAsync(productType.ProductTypeId);
            if (productData != null)
            {
                productData.Msp = productType.Msp;
            }
            try
            {
                isDataSaved = await _ApnaAaharContext.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw new DataNotSavedException("unable to save data");
            }
            catch (Exception)
            {
                throw new DataNotSavedException("unable to save data");
            }
            if (isDataSaved > 0)
            {
                return true;
            }
            return false;
        }
    }
}
