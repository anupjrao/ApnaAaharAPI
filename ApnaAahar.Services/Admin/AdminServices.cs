using ApnaAahar.Repository.Admin;
using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Services.Admin
{
    public class AdminServices:IAdminServices
    {
        private readonly IAdminData _AdminData;

        public AdminServices(IAdminData AdminData)
        {
            _AdminData = AdminData;
        }
        public async Task<IEnumerable<Users>> GetRegisteredFarmers()
        {
            return await _AdminData.GetRegisteredFarmers();
        }

        public async Task<IEnumerable<Users>> GetRequestingFarmers()
        {
            return await _AdminData.GetRequestingFarmers();
        }

        public async Task<bool> DisableUser(Users user)
        {
            return await _AdminData.DisableUser(user);
        }
        public async Task<bool> AcceptRequest(Users user)
        {
            return await _AdminData.AcceptRequest(user);
        }
        public async Task<bool> DeclineRequest(Users user)
        {
            return await _AdminData.DeclineRequest(user);
        }

        public async Task<List<ProductType>> GetProductTypes()
        {
            return await _AdminData.GetProductTypes();
        }

        public async Task<bool> UpdateProductMsp(ProductType productType)
        {
            return await _AdminData.UpdateProductMsp(productType);
        }
    }
}
