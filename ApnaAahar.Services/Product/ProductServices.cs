using ApnaAahar.Repository;
using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IProductData _ProductData;


        public ProductServices(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        /// <summary>
        /// Method to add prdouctListings
        /// </summary>
        /// <param name="productListingData"></param>
        /// <returns></returns>
        public async Task<bool> AddProductListings(ProductListingData productListingData)
        {
            return await _ProductData.AddProductListings(productListingData);
        }

        /// <summary>
        /// Method to get all type of ProductTypes
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductType>> GetAllProductTypes()
        {
            return await _ProductData.GetAllProductTypes();
        }

        /// <summary>
        /// service method to get productListings returned from repo layer
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ProductListingData>> GetProductListings()
        {
            return await _ProductData.GetProductListings();
        }

        public async Task<IEnumerable<ProductListingData>> GetProductListings(string query)
        {
            return await _ProductData.GetProductListingsFilteredByName(query);
        }
        /// <summary>
        /// service method to get product listings by farmer from repo layer
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProductListingData>> GetProductListingsByFarmer(Users user)
        {
            return await _ProductData.GetProductListingsFilteredByFarmer(user);
        }
        /// <summary>
        /// service method to update product listing
        /// </summary>
        /// <param name="productListingData"></param>
        /// <returns></returns>
        public async Task<int> UpdateProductListings(ProductListingData productListingData)
        {
            return await _ProductData.UpdateProductListings(productListingData);
        }
    }
}
