using ApnaAahar.Exceptions;
using ApnaAahar.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Repository
{
    public class ProductData : IProductData
    {
        private readonly Orchard1Context _ApnaAaharContext;
        public ProductData(Orchard1Context ApnaAaharContext)
        {
            _ApnaAaharContext = ApnaAaharContext;
        }

        /// <summary>
        /// Method to add prdouctListings
        /// </summary>
        /// <param name="productListingData"></param>
        /// <returns></returns>
        public async Task<bool> AddProductListings(ProductListingData productListingData)
        {
            try
            {
                int userId = productListingData.FarmerId;
                FarmerDetails farmer = await _ApnaAaharContext.FarmerDetails.SingleOrDefaultAsync(farmerFound => farmerFound.UserId == userId);
                int FramerId = farmer.FarmerId;
                productListingData.FarmerId = FramerId;
                int rowAffected = 0;
                _ApnaAaharContext.Add(productListingData);
                rowAffected = await _ApnaAaharContext.SaveChangesAsync();
                if (rowAffected == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw new AnySqlException("Internal Error Occured!", ex);
            }
            catch (Exception ex)
            {
                throw new GeneralException("Unchecked error occured", ex);
            }
        }

        /// <summary>
        /// Method to get all type of ProductTypes
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductType>> GetAllProductTypes()
        {
            try
            {
                List<ProductType> productTypes = new List<ProductType>();
                productTypes = await _ApnaAaharContext.ProductType.ToListAsync();
                return productTypes;
            }
            catch (SqlException ex)
            {
                throw new AnySqlException("Internal Error Occured!", ex);
            }
            catch (Exception ex)
            {
                throw new GeneralException("Unchecked error occured", ex);
            }
        }

        /// <summary>
        /// repo method to get all productListings from database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ProductListingData>> GetProductListings()
        {
            List<ProductListingData> productListings;
            try
            {
                productListings = await _ApnaAaharContext.ProductListingData.Include(farmer => farmer.Farmer).ThenInclude(farmer => farmer.User).Include(productType => productType.ProductType).AsNoTracking().ToListAsync();

                foreach (ProductListingData productListing in productListings)
                {
                    productListing.Farmer.User.FarmerDetails = null;
                    productListing.ProductType.ProductListingData = null;
                }
            }
            catch (Exception)
            {
                throw new GeneralException("Something went wrong");
            }
            return productListings;
        }
        /// <summary>
        /// method to get productListings by productType Name
        /// </summary>
        /// <param name="typeFilter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProductListingData>> GetProductListingsFilteredByName(string typeFilter)
        {
            List<ProductListingData> productListings = new List<ProductListingData>();
            try
            {


                productListings = await _ApnaAaharContext.ProductListingData.Include(farmer => farmer.Farmer).ThenInclude(farmer => farmer.User).Include(productType => productType.ProductType).Where(productListingData=> productListingData.ProductType.ProductType1.Contains(typeFilter)).AsNoTracking().ToListAsync();

                foreach (ProductListingData productListing in productListings)
                {
                    productListing.Farmer.User.FarmerDetails = null;
                    productListing.ProductType.ProductListingData = null;
                }

            }
            catch (Exception)
            {
                throw new GeneralException("Something went wrong");
            }
            return productListings;
        }

        /// <summary>
        /// method to get productListings by Farmer Id
        /// </summary>
        /// <param name="typeFilter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProductListingData>> GetProductListingsFilteredByFarmer(Users user)
        {
            List<ProductListingData> productListings = new List<ProductListingData>();
            try
            {
                FarmerDetails farmer = await _ApnaAaharContext.FarmerDetails.FirstOrDefaultAsync(findFarmer => findFarmer.UserId == user.UserId);
                int farmerId = farmer.FarmerId;

                productListings = await _ApnaAaharContext.ProductListingData.Where(productListingData => productListingData.FarmerId == farmerId).Include(farmers => farmers.Farmer).ThenInclude(farmers => farmers.User).Include(productType => productType.ProductType).AsNoTracking().ToListAsync();
                foreach (ProductListingData productListing in productListings)
                {
                    productListing.Farmer.User.FarmerDetails = null;
                    productListing.ProductType.ProductListingData = null;
                }
            }
            catch (Exception)
            {
                throw new GeneralException("Something went wrong");
            }
            return productListings;
        }
        /// <summary>
        /// method to update product listing data
        /// </summary>
        /// <param name="productListingDataNew"></param>
        /// <returns>int based on savechanges</returns>
        public async Task<int> UpdateProductListings(ProductListingData productListingDataNew)
        {
            try
            {
                ProductListingData productListingDataOld = await _ApnaAaharContext.ProductListingData
                    .FindAsync(productListingDataNew.ProductListingId);

                productListingDataOld.Price = productListingDataNew.Price;
                productListingDataOld.Quantity = productListingDataNew.Quantity;
                return await _ApnaAaharContext.SaveChangesAsync();
            }
            catch (SqlException ex)
            {
                throw new AnySqlException("Internal Error Occured!", ex);
            }
            catch (Exception ex)
            {
                throw new GeneralException("Unchecked error occured", ex);
            }
        }

    }
}
