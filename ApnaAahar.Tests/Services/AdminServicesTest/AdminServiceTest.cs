using ApnaAahar.Exceptions;
using ApnaAahar.Repository.Admin;
using ApnaAahar.Repository.Models;
using ApnaAahar.Services.Admin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApnaAahar.Tests.Services.AdminServicesTest
{
    [TestClass]
    public class AdminServiceTest
    {
        Mock<IAdminData> _adminRepositoryMock = new Mock<IAdminData>();
        IAdminServices _adminServices;
        public AdminServiceTest()
        {
            _adminServices = new AdminServices(_adminRepositoryMock.Object);
        }

        [TestMethod]
        public async Task GetRegisteredFarmers_ReturnUserLists_WhenUsersExist_Happy()
        {
            var testUserData = new List<Users>
            {
                new Users
                {
                    UserFullName="charan kiran",
                    Location="Banglore",
                    Email="abcd@gmail.com"
                },
                new Users
                {
                      UserFullName="ramesh kiran",
                    Location="chennai",
                    Email="abcd1234@gmail.com"
                }
            };
            _adminRepositoryMock.Setup(p => p.GetRegisteredFarmers()).ReturnsAsync(testUserData);
            var testResult =await _adminServices.GetRegisteredFarmers();
            CollectionAssert.Equals(testUserData, testResult);
        }

        [TestMethod]
        public async Task GetRegisteredFarmers_ReturnNull_WhenNoUsersExist_Sad()
        {   
            _adminRepositoryMock.Setup(p => p.GetRegisteredFarmers()).ReturnsAsync(()=>null);
            var testResult =await _adminServices.GetRegisteredFarmers();
            Assert.IsNull(testResult);
        }
        [TestMethod]
        public async Task GetRegisteredFarmers_ThrowException_WhenException_Bad()
        {
            _adminRepositoryMock.Setup(p => p.GetRegisteredFarmers()).ThrowsAsync(new GeneralException("unknown Exception Occured",new Exception()));
            await Assert.ThrowsExceptionAsync<GeneralException>(async() => await _adminServices.GetRegisteredFarmers());

        }

        [TestMethod]
        public async Task GetRequestingFarmers_ReturnUserLists_WhenUsersExist_Happy()
        {
            var testUserData = new List<Users>
            {
                new Users
                {
                    UserFullName="charan kiran",
                    Location="Banglore",
                    Email="abcd@gmail.com"
                },
                new Users
                {
                      UserFullName="ramesh kiran",
                    Location="chennai",
                    Email="abcd1234@gmail.com"
                }
            };
            _adminRepositoryMock.Setup(p => p.GetRegisteredFarmers()).ReturnsAsync(testUserData);
            var testResult = await _adminServices.GetRequestingFarmers();
            CollectionAssert.Equals(testUserData, testResult);
        }

        [TestMethod]
        public async Task GetRequestingFarmers_ReturnNull_WhenNoUsersExist_Sad()
        {
            _adminRepositoryMock.Setup(p => p.GetRequestingFarmers()).ReturnsAsync(() => null);
            var testResult = await _adminServices.GetRequestingFarmers();
            Assert.IsNull(testResult);
        }
        [TestMethod]
        public async Task GetRequestingFarmers_ThrowException_WhenException_Bad()
        {
            _adminRepositoryMock.Setup(p => p.GetRequestingFarmers()).ThrowsAsync(new GeneralException("unknown Exception Occured", new Exception()));
            await Assert.ThrowsExceptionAsync<GeneralException>(async () => await _adminServices.GetRequestingFarmers());

        }

        [TestMethod]
        public async Task DisableUser_ReturnTrue_Happpy()
        {
            Users user = new Users() {
                UserId = 1
            };
            _adminRepositoryMock.Setup(p => p.DisableUser(It.IsAny<Users>())).ReturnsAsync(true);
            var result = await _adminServices.DisableUser(user);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public async Task DisableUser_ThrowException_Bad()
        {
            Users user = new Users()
            {
                UserId = 1
            };
            _adminRepositoryMock.Setup(p => p.DisableUser(It.IsAny<Users>())).ThrowsAsync(new DbContextException());
            await Assert.ThrowsExceptionAsync<DbContextException>(async () => await _adminServices.DisableUser(user));
        }
        [TestMethod]
        public async Task AcceptRequest_ReturnTrue_Happpy()
        {
            Users user = new Users()
            {
                UserId = 1
            };
            _adminRepositoryMock.Setup(p => p.AcceptRequest(It.IsAny<Users>())).ReturnsAsync(true);
            var result = await _adminServices.AcceptRequest(user);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public async Task AcceptRequest_ThrowException_Bad()
        {
            Users user = new Users()
            {
                UserId = 1
            };
            _adminRepositoryMock.Setup(p => p.AcceptRequest(It.IsAny<Users>())).ThrowsAsync(new DbContextException());
            await Assert.ThrowsExceptionAsync<DbContextException>(async () => await _adminServices.AcceptRequest(user));
        }

        [TestMethod]
        public async Task DeclineRequest_ReturnTrue_Happpy()
        {
            Users user = new Users()
            {
                UserId = 1
            };
            _adminRepositoryMock.Setup(p => p.DeclineRequest(It.IsAny<Users>())).ReturnsAsync(true);
            var result = await _adminServices.DeclineRequest(user);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public async Task DeclineRequest_ThrowException_Bad()
        {
            Users user = new Users()
            {
                UserId = 1
            };
            _adminRepositoryMock.Setup(p => p.DeclineRequest(It.IsAny<Users>())).ThrowsAsync(new DbContextException());
            await Assert.ThrowsExceptionAsync<DbContextException>(async () => await _adminServices.DeclineRequest(user));
        }

        [TestMethod]
        public async Task GetProductTypes_ShouldReturnAllProductsTypes_Happy()
        {
            List<ProductType> productTypes = new List<ProductType>
            {
                new ProductType(){ProductTypeId=1,ProductType1="tomato",Msp=50},
                new ProductType(){ProductTypeId=2,ProductType1="potato",Msp=60}
            };
            _adminRepositoryMock.Setup(s => s.GetProductTypes()).ReturnsAsync(productTypes);
            List<ProductType> testResult = await _adminServices.GetProductTypes();
            CollectionAssert.AreEqual(productTypes, testResult);
        }

        [TestMethod]
        public async Task GetProductTypes_ShouldReturnNothing_Sad()
        {
            _adminRepositoryMock.Setup(s => s.GetProductTypes()).ReturnsAsync(() => null);
            List<ProductType> testResult = await _adminServices.GetProductTypes();
            Assert.IsNull(testResult);
        }

        [TestMethod]
        public async Task GetProductTypes_ThrowsException_Bad()
        {
            _adminRepositoryMock.Setup(s => s.GetProductTypes()).ThrowsAsync(new GeneralException());
            await Assert.ThrowsExceptionAsync<GeneralException>(async () => await _adminServices.GetProductTypes());
        }
        [TestMethod]
        public async Task UpdateProductMsp_ShouldReturnTrue_Happy()
        {
            ProductType productType = new ProductType()
            {
                ProductTypeId = 1,
                ProductType1 = "tomato",
                Msp = 50
            };
            _adminRepositoryMock.Setup(s => s.UpdateProductMsp(It.IsAny<ProductType>())).ReturnsAsync(true);
            bool testResult = await _adminServices.UpdateProductMsp(productType);
            Assert.IsTrue(testResult);
        }
        [TestMethod]
        public async Task UpdateProductMsp_ShouldReturnFalse_Sad()
        {
            ProductType productType = new ProductType()
            {
                ProductTypeId = 1,
                ProductType1 = "tomato",
                Msp = 50
            };
            _adminRepositoryMock.Setup(s => s.UpdateProductMsp(It.IsAny<ProductType>())).ReturnsAsync(false);
            bool testResult = await _adminServices.UpdateProductMsp(productType);
            Assert.IsFalse(testResult);
        }
        [TestMethod]
        public async Task UpdateProductMsp_ThrowsException_Bad()
        {
            ProductType productType = new ProductType()
            {
                ProductTypeId = 1,
                ProductType1 = "tomato",
                Msp = 50
            };
            _adminRepositoryMock.Setup(s => s.UpdateProductMsp(It.IsAny<ProductType>())).ThrowsAsync(new DataNotSavedException());
            await Assert.ThrowsExceptionAsync<DataNotSavedException>(async () => await _adminServices.UpdateProductMsp(productType));
        }

    }
}
