using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Repository
{
    public interface IProductData
    {
        Task<IEnumerable<ProductListingData>> GetProductListings();
        Task<IEnumerable<ProductListingData>> GetProductListingsFilteredByName(string typeFilter);

        Task<List<ProductType>> GetAllProductTypes();
        Task<bool> AddProductListings(ProductListingData productListingData);
        Task<IEnumerable<ProductListingData>> GetProductListingsFilteredByFarmer(Users user);
        Task<int> UpdateProductListings(ProductListingData productListingDataNew);


    }
}
