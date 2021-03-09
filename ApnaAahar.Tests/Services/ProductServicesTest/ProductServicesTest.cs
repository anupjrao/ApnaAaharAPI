using ApnaAahar.Exceptions;
using ApnaAahar.Repository;
using ApnaAahar.Repository.Models;
using ApnaAahar.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApnaAahar.Tests.Services.Product
{
    [TestClass]
    public class ProductServicesTest
    {
        Mock<IProductData> productDataMock = new Mock<IProductData>();
        IProductServices productServices;

        public ProductServicesTest()
        {
            productServices = new ProductServices(productDataMock.Object);
        }

        [TestMethod]
        public async Task GetListings_ShouldReturnProductListings_WhenListingsExistAsync_Happy()
        {
            ProductListingData productListing1 = new ProductListingData();
            productListing1.ProductListingId = 1;
            productListing1.FarmerId = 1;
            productListing1.Quantity = 20;
            productListing1.Price = 50;
            productListing1.ProductTypeId = 1;

            ProductListingData productListing2 = new ProductListingData();
            productListing2.ProductListingId = 2;
            productListing2.FarmerId = 2;
            productListing2.Quantity = 30;
            productListing2.Price = 40;
            productListing2.ProductTypeId = 2;
            var testListings = new List<ProductListingData>()
            {
                productListing1,
                productListing2
            };

            //Arrange
            productDataMock.Setup(x => x.GetProductListings()).ReturnsAsync(testListings);
            //Act
            List<ProductListingData> testResult = (List<ProductListingData>)await productServices.GetProductListings();
            //Assert
            CollectionAssert.AreEqual(testListings, testResult);
        }

        [TestMethod]
        public async Task GetListings_ShouldReturnNothing_WhenListingsDoNotExistAsync_Sad()
        {
            
            
            //Arrange
            productDataMock.Setup(x => x.GetProductListings()).ReturnsAsync(() => null);
            //Act
            List<ProductListingData> testResult = (List<ProductListingData>)await productServices.GetProductListings();
            //Assert
            Assert.IsNull(testResult);
        }

        [TestMethod]
        public async Task GetListings_ShouldThrowException_WhenException_Bad()
        {
           
            //Arrange
            productDataMock.Setup(x => x.GetProductListings()).Throws(new GeneralException());
            //Assert
            await Assert.ThrowsExceptionAsync<GeneralException>(async() => await productServices.GetProductListings());
            
           
        }

        ////////////////////////////////////////////////////////////////////////
        [TestMethod]
        public async Task GetListingsFiltered_ShouldReturnProductListings_WhenListingsExistAsync_Happy()
        {
            ProductListingData productListing1 = new ProductListingData();
            productListing1.ProductListingId = 1;
            productListing1.FarmerId = 1;
            productListing1.Quantity = 20;
            productListing1.Price = 50;
            productListing1.ProductTypeId = 1;
            productListing1.ProductType = new ProductType() { ProductTypeId=1, ProductType1="Product1"};

            ProductListingData productListing2 = new ProductListingData();
            productListing2.ProductListingId = 2;
            productListing2.FarmerId = 2;
            productListing2.Quantity = 30;
            productListing2.Price = 40;
            productListing2.ProductTypeId = 2;
            productListing2.ProductType = new ProductType() { ProductTypeId = 2, ProductType1 = "Product2" };

            var testListings = new List<ProductListingData>()
            {
                productListing1,
                productListing2
            };

            //Arrange
            productDataMock.Setup(x => x.GetProductListingsFilteredByName("ProductType")).ReturnsAsync(testListings);
            //Act
            List<ProductListingData> testResult = (List<ProductListingData>)await productServices.GetProductListings("ProductType");
            //Assert
            CollectionAssert.AreEqual(testListings, testResult);
        }

        [TestMethod]
        public async Task GetListingsFiltered_ShouldThrowException_WhenException_Bad()
        {

            //Arrange
            productDataMock.Setup(x => x.GetProductListingsFilteredByName("ProductType")).Throws(new GeneralException());
            //Assert
            await Assert.ThrowsExceptionAsync<GeneralException>(async () => await productServices.GetProductListings("ProductType"));


        }


        //////////////////////////////Testing For Add Product and get Product type List///////////

        [TestMethod]
        public async Task GetProducType_ShouldReturnProductTypeList_WhenListingsExistAsync_Happy()
        {
            List<ProductType> productTypeList = new List<ProductType>(){
                new ProductType()
            {
                ProductTypeId = 1,
                ProductType1 = "Rice",
                Msp = 40
            },
                new ProductType()
            {
                ProductTypeId = 2,
                ProductType1 = "Onion",
                Msp = 40
            }
            };
            productDataMock.Setup(t => t.GetAllProductTypes()).ReturnsAsync(productTypeList);

            List<ProductType> result = await productServices.GetAllProductTypes();

            CollectionAssert.AreEqual(productTypeList, result);

        }

        [TestMethod]
        public async Task GetProducType_ShouldReturnProductTypeList_WhenListingsDonotExistAsync_Sad()
        {
            productDataMock.Setup(t => t.GetAllProductTypes()).ReturnsAsync(() => null);

            List<ProductType> result = await productServices.GetAllProductTypes();

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetProducType_ShouldReturnProductTypeList_WhenExceptionAsync_Bad()
        {
            productDataMock.Setup(t => t.GetAllProductTypes()).Throws(new AnySqlException("Server error occured"));

            await Assert.ThrowsExceptionAsync<AnySqlException>(async () => await productServices.GetAllProductTypes());

        }

        [TestMethod]
        public async Task AddProductListings_ShouldReturnTrue_WhenProductAddedAsync_Happy()
        {
            ProductListingData productListing1 = new ProductListingData();
            productListing1.ProductListingId = 1;
            productListing1.FarmerId = 1;
            productListing1.Quantity = 20;
            productListing1.Price = 50;
            productListing1.ProductTypeId = 1;

            productDataMock.Setup(t => t.AddProductListings(It.IsAny<ProductListingData>())).ReturnsAsync(true);

            bool result = await productServices.AddProductListings(productListing1);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task AddProductListings_ShouldReturnTrue_WhenProductNotAddedAsync_Sad()
        {
            ProductListingData productListing1 = new ProductListingData();
            productListing1.ProductListingId = 1;
            productListing1.FarmerId = 1;
            productListing1.Quantity = 20;
            productListing1.Price = 50;
            productListing1.ProductTypeId = 1;

            productDataMock.Setup(t => t.AddProductListings(It.IsAny<ProductListingData>())).ReturnsAsync(false);

            bool result = await productServices.AddProductListings(productListing1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task AddProductListings_ShouldThrowException_WhenExceptionExistAsync_Bad()
        {
            ProductListingData productListing1 = new ProductListingData();
            productListing1.ProductListingId = 1;
            productListing1.FarmerId = 1;
            productListing1.Quantity = 20;
            productListing1.Price = 50;
            productListing1.ProductTypeId = 1;

            productDataMock.Setup(t => t.AddProductListings(It.IsAny<ProductListingData>())).Throws(new AnySqlException("Server error occured!"));

            await Assert.ThrowsExceptionAsync<AnySqlException>(async () => await productServices.AddProductListings(productListing1));
        }

        [TestMethod]
        public async Task GetListingsByFarmer_ShouldReturnProductListings_WhenListingsExistAsync_Happy()
        {
            ProductListingData productListing1 = new ProductListingData();
            productListing1.ProductListingId = 1;
            productListing1.FarmerId = 1;
            productListing1.Quantity = 20;
            productListing1.Price = 50;
            productListing1.ProductTypeId = 1;

            ProductListingData productListing2 = new ProductListingData();
            productListing2.ProductListingId = 2;
            productListing2.FarmerId = 2;
            productListing2.Quantity = 30;
            productListing2.Price = 40;
            productListing2.ProductTypeId = 2;
            var testListings = new List<ProductListingData>()
            {
                productListing1,
                productListing2
            };
            Users user = new Users();
            user.UserId = 1;
            //Arrange
            productDataMock.Setup(x => x.GetProductListingsFilteredByFarmer(user)).ReturnsAsync(testListings);
            //Act
            List<ProductListingData> testResult = (List<ProductListingData>)await productServices.GetProductListingsByFarmer(user);
            //Assert
            CollectionAssert.AreEqual(testListings, testResult);
        }

        [TestMethod]
        public async Task GetListingsByFarmer_ShouldReturnNothing_WhenListingsDoNotExistAsync_Sad()
        {
            Users user = new Users();
            user.UserId = 1;

            //Arrange
            productDataMock.Setup(x => x.GetProductListingsFilteredByFarmer(user)).ReturnsAsync(() => null);
            //Act
            List<ProductListingData> testResult = (List<ProductListingData>)await productServices.GetProductListingsByFarmer(user);
            //Assert
            Assert.IsNull(testResult);
        }

        [TestMethod]
        public async Task GetListingsByFarmer_ShouldThrowException_WhenException_Bad()
        {
            Users user = new Users();
            user.UserId = 1;
            //Arrange
            productDataMock.Setup(x => x.GetProductListingsFilteredByFarmer(user)).Throws(new GeneralException());
            //Assert
            await Assert.ThrowsExceptionAsync<GeneralException>(async() => await productServices.GetProductListingsByFarmer(user));
            
           
        }

        [TestMethod]
        public async Task UpdateListing_ShouldReturnInt_WhenUpdateSuccessful_Happy()
        {
            //Arrange
            productDataMock.Setup(x => x.UpdateProductListings(It.IsAny<ProductListingData>())).ReturnsAsync(1);
            //Act
            var result = await productServices.UpdateProductListings(new ProductListingData() { Price = 60 });
            //Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task UpdateListing_ShouldReturnInt_WhenUpdateNullValue_Sad()
        {
            //Arrange
            productDataMock.Setup(x => x.UpdateProductListings(It.IsAny<ProductListingData>())).ReturnsAsync(0);
            //Act
            var result = await productServices.UpdateProductListings(new ProductListingData() { Price = 60 });
            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task UpdateListing_ShouldThrowException_WhenException_Bad()
        {
            //Arrange
            productDataMock.Setup(x => x.UpdateProductListings(It.IsAny<ProductListingData>())).Throws(new GeneralException());
            //Assert
            await Assert.ThrowsExceptionAsync<GeneralException>(async () => await productServices.UpdateProductListings(new ProductListingData()));
        }
    }
}
