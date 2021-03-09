using ApnaAahar.Exceptions;
using ApnaAahar.Repository;
using ApnaAahar.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApnaAahar.Tests.Repository.ProductRepositoryTest
{
    [TestClass]
    public class ProductRepositoryTest
    {

        DbContextOptions<Orchard1Context> options = new DbContextOptionsBuilder<Orchard1Context>().UseInMemoryDatabase("OrchardDB").Options;
        Orchard1Context Orchard1Context;
        IProductData productData;
        IProductData mockedProductData;
        //DbContextOptions<Orchard1Context> options = new DbContextOptionsBuilder<Orchard1Context>().UseInMemoryDatabase("OrchardDB").Options;
        Mock<Orchard1Context> orchardContext;
        //IProductData productData;
        public ProductRepositoryTest()
        {
            orchardContext = new Mock<Orchard1Context>(options);
            //productData = new ProductData(orchardContext.Object);
            Orchard1Context = new Orchard1Context(options);
            productData = new ProductData(Orchard1Context);

            mockedProductData = new ProductData(orchardContext.Object);
        }
        /*
        MOCK DATA
        */
        public List<ProductListingData> GetListings()
        {
            return new List<ProductListingData>() {
                            new ProductListingData() {
                                ProductListingId = 1,
                                FarmerId = 1,
                                Quantity = 20,
                                Price = 50,
                                ProductTypeId = 1
                             },
                            new ProductListingData() {
                               ProductListingId = 2,
                                FarmerId = 2,
                                Quantity = 30,
                                Price = 40,
                                ProductTypeId = 2
                            }
            };
        }
        
        public List<FarmerDetails> GetFarmerDetails()
        {
            return new List<FarmerDetails>() {
                new FarmerDetails() { FarmerId = 1, UserId = 1},
                new FarmerDetails() { FarmerId = 2, UserId = 2},
            };
        }
        public List<Users> GetUserDetails()
        {
            return new List<Users>() {
                new Users() { UserId = 1},
                new Users() { UserId = 2},
            };
        }
        public List<ProductType> GetProductTypes()
        {
            return new List<ProductType>() {
                new ProductType() { ProductTypeId = 1, ProductType1="ProductType1",Msp=40},
                new ProductType() { ProductTypeId = 2, ProductType1="ProductType2",Msp=60},
            };
        }

        /*
        MOCK DATA
        */

        public void SeedProductListing()
        {
            Orchard1Context.ProductListingData.AddRange(GetListings().ToArray());
            Orchard1Context.SaveChanges();

        }
        public void SeedUsers()
        {
            Orchard1Context.Users.AddRange(GetUserDetails().ToArray());
            Orchard1Context.SaveChanges();

        }
        public void SeedFarmerDetails()
        {
            Orchard1Context.FarmerDetails.AddRange(GetFarmerDetails().ToArray());
            Orchard1Context.SaveChanges();

        }
        public void SeedProductType()
        {
            Orchard1Context.ProductType.AddRange(GetProductTypes().ToArray());
            Orchard1Context.SaveChanges();

        }
        public void SeedContext()
        {
            SeedProductListing();
            SeedUsers();
            SeedFarmerDetails();
            SeedProductType();
            Orchard1Context.SaveChanges();
        }

        
        [TestMethod]
        public void GetProductListingsAsync_ShouldReturnProductListings_WhenListingsExist_Happy()
        {
            //Arrange
            SeedContext();
            //SeedProductListing();
            List<ProductListingData> productListings = GetListings();
            //Act
            var results = productData.GetProductListings();
            //Assert.AreEqual(productListings, results.Result);
            //Assert.IsNotNull(results.Result);
            CollectionAssert.AllItemsAreInstancesOfType((List<ProductListingData>)results.Result, typeof(ProductListingData));
        }

        [TestMethod]
        public async Task GetProductListingsAsync_ShouldThrowException_WhenException_Bad()
        {
            //Arrange
            orchardContext.Setup(o => o.ProductListingData).Throws(new GeneralException());
            //Act
            
            //Assert
            await Assert.ThrowsExceptionAsync<GeneralException>(async() => await mockedProductData.GetProductListings());
        }

        [TestMethod]
        public void GetProductListingsFilteredByName_ShouldReturnProductListings_WhenListingsExist_Happy()
        {
            //Arrange
            
            //SeedProductListing();
            List<ProductListingData> productListings = GetListings();
            //Act
            var results = productData.GetProductListingsFilteredByName("Product");
            //Assert
            //Assert.AreEqual(productListings, results.Result);
            //Assert.IsNotNull(results.Result);
            CollectionAssert.AllItemsAreInstancesOfType((List<ProductListingData>)results.Result, typeof(ProductListingData));
        }
        [TestMethod]
        public void GetProductListingsFilteredByName_ShouldReturnProductListings_WhenException_Bad()
        {
            //Arrange
            orchardContext.Setup(o => o.ProductListingData).Throws(new GeneralException());
            //Act
            //Assert
            Assert.ThrowsExceptionAsync<GeneralException>(async () => await mockedProductData.GetProductListingsFilteredByName("Abcd"));
        }

        [TestMethod]
        public void GetProductTypeList_ShouldReturnProductTypeList_WhenListingsExist_Happy()
        {
            List<ProductType> productTypes = Orchard1Context.ProductType.ToList();
            var result = productData.GetAllProductTypes();
            CollectionAssert.AreEqual(result.Result, productTypes);
        }

        [TestMethod]
        public void GetProductTypeList_ShouldTrowException_WhenExceptionExist_Bad()
        {
            //Arrange
            orchardContext.Setup(o => o.ProductType).Throws(new AnySqlException());
            //Act
            //Assert
            Assert.ThrowsExceptionAsync<AnySqlException> (async () => await mockedProductData.GetAllProductTypes());
        }

        [TestMethod]
        public async Task AddProdcutData_ShouldReturnTrue_WhenProductAdded_Happy()
        {
            Orchard1Context.Database.EnsureDeleted();
            ProductListingData productListing1 = new ProductListingData();
            productListing1.FarmerId = 1;
            productListing1.Quantity = 20;
            productListing1.Price = 50;
            productListing1.ProductTypeId = 1;
            SeedFarmerDetails();
            bool result = await productData.AddProductListings(productListing1);
            Assert.IsTrue(result);
            Orchard1Context.Database.EnsureDeleted();
        }

        [TestMethod]
        public  void AddProdcutData_ShouldThrowException_WhenExceptionExist_Bad()
        {
            Orchard1Context.Database.EnsureDeleted();
            ProductListingData productListing1 = new ProductListingData();
            productListing1.FarmerId = 1;
            productListing1.Quantity = 20;
            productListing1.Price = 50;
            productListing1.ProductTypeId = 1;
            SeedFarmerDetails();
            orchardContext.Setup(o => o.ProductListingData).Throws(new AnySqlException());
            Assert.ThrowsExceptionAsync<AnySqlException>(async () => await mockedProductData.AddProductListings(productListing1));
            Orchard1Context.Database.EnsureDeleted();
        }
        [TestMethod]
        public void GetProductListingsFilteredByFarmerAsync_ShouldReturnProductListings_WhenListingsExist_Happy()
        {
            Orchard1Context.Database.EnsureDeleted();

            //Arrange
            SeedContext();
            //SeedProductListing();
            List<ProductListingData> productListings = GetListings();
            Users user = new Users();
            user.UserId = 1;
            
            //Act
            var results = productData.GetProductListingsFilteredByFarmer(user);
            //Assert.AreEqual(productListings, results.Result);
            //Assert.IsNotNull(results.Result);
            CollectionAssert.AllItemsAreInstancesOfType((List<ProductListingData>)results.Result, typeof(ProductListingData));
        }

        [TestMethod]
        public void GetProductListingsFilteredByFarmer_ShouldReturnProductListings_WhenException_Bad()
        {
            //Arrange
            orchardContext.Setup(o => o.ProductListingData).Throws(new GeneralException());
            Users user = new Users();
            user.UserId = 1;
            //Act
            //Assert
            Assert.ThrowsExceptionAsync<GeneralException>(async () => await mockedProductData.GetProductListingsFilteredByFarmer(user));
        }

        [TestMethod]
        public async Task UpdateProductListings_ShouldReturnNonZero_WhenUpdateSuccessful_Happy()
        {
            orchardContext.Setup(b => b.ProductListingData.FindAsync(It.IsAny<int>())).ReturnsAsync(new ProductListingData() { ProductListingId = 1 });
            CancellationToken cancellationToken = CancellationToken.None;
            orchardContext.Setup(b => b.SaveChangesAsync(cancellationToken)).ReturnsAsync(1);
            var result = await mockedProductData.UpdateProductListings(new ProductListingData()
            {
                ProductListingId = 1,
                Price = 65 ,
            });
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task UpdateProductListings_ShouldThrowException_WhenException_Bad()
        {
            orchardContext.Setup(b => b.ProductListingData.FindAsync(It.IsAny<int>())).ReturnsAsync(null as ProductListingData);
            CancellationToken cancellationToken = CancellationToken.None;
            orchardContext.Setup(b => b.SaveChangesAsync(cancellationToken)).Throws(new GeneralException());
            await Assert.ThrowsExceptionAsync<GeneralException>(async () => await mockedProductData.UpdateProductListings((new ProductListingData
            {
                ProductListingId = 1,
                Price = 0
            }))) ;
        }
    }
}
