using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Services
{
    public interface IProductServices
    {
        Task<IEnumerable<ProductListingData>> GetProductListings();
        Task<IEnumerable<ProductListingData>> GetProductListings(string query);
        Task<IEnumerable<ProductListingData>> GetProductListingsByFarmer(Users user);

        Task<List<ProductType>> GetAllProductTypes();
        Task<bool> AddProductListings(ProductListingData productListingData);
        Task<int> UpdateProductListings(ProductListingData productListingData);

    }
}
