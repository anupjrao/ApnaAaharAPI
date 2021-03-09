using ApnaAahar.Exceptions;
using ApnaAahar.Repository.Admin;
using ApnaAahar.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApnaAahar.Tests.Repository.AdminRepositoryTest
{
    [TestClass]
    public class AdminRepositoryTest
    {
        DbContextOptions<Orchard1Context> options = new DbContextOptionsBuilder<Orchard1Context>().UseInMemoryDatabase("ApnaAahar").Options;
        Mock<Orchard1Context> orchard1ContextMock;
        IAdminData adminRepository;
        Orchard1Context _orchard1Context;
        IAdminData _adminRepository;
      
        public AdminRepositoryTest()
        {
             orchard1ContextMock = new Mock<Orchard1Context>(options);
             adminRepository = new AdminData(orchard1ContextMock.Object);
            _orchard1Context = new Orchard1Context(options);
            _adminRepository = new AdminData(_orchard1Context);
        }

        /*MockData*/
        public List<Users> GetUsersList()
        {
            return new List<Users>
            {
                new Users
                {
                    UserId =1,UserFullName="charan kiran",Location="Banglore",Email="abcd@gmail.com",IsDeleted=false
                },
                new Users
                {
                      UserId=2,UserFullName="ramesh kiran",Location="chennai",Email="abcd1234@gmail.com",IsDeleted=false
                },
                new Users
                {
                      UserId=3,UserFullName="Username",Location="kavali",Email="abcdef1234@gmail.com",IsDeleted=false

                }
            };
        }

        public List<FarmerDetails> GetFarmerDetails()
        {
            return new List<FarmerDetails>
            {
                new FarmerDetails { UserId = 1, FarmerId=1234,IsApproved = false, IsAccountDisabled = false },
                new FarmerDetails { UserId = 2,FarmerId=2345 ,IsApproved = true, IsAccountDisabled = false},
                new FarmerDetails { UserId = 3,FarmerId=1223 ,IsApproved = false, IsAccountDisabled = false}

            };
        }
      
        public void SeedUsers()
        {
            //Arrange
            _orchard1Context.Users.AddRange(GetUsersList().ToArray());
            _orchard1Context.SaveChanges();
        }
        public void SeedFarmers()
        {
            _orchard1Context.FarmerDetails.AddRange(GetFarmerDetails().ToArray());
            _orchard1Context.SaveChanges();
        }

        public static void SeedContext()
        {
            AdminRepositoryTest a = new AdminRepositoryTest();
            a.SeedUsers();
            a.SeedFarmers();
        }
        [ClassInitialize]
        public static void InitializeTest(TestContext t)
        {
            SeedContext();
        }
        [TestMethod]
        public async Task GetRegisteredUsers_ReturnList_RequestsExists_Happy()
        {
            //Arrange
            
            //Act
            List<Users> result=(List<Users>)await _adminRepository.GetRegisteredFarmers();
           
            CollectionAssert.AllItemsAreInstancesOfType(result,typeof(Users));
        }
        [TestMethod]
        public async Task GetRegisteredUsers_ThrowsException_WhenException_Bad()
        {
            orchard1ContextMock.Setup(a => a.Users).Throws(new GeneralException());
            await Assert.ThrowsExceptionAsync<GeneralException>(async () => await adminRepository.GetRegisteredFarmers());
        }

        [TestMethod]
        public async Task GetRequestedUsers_ReturnList_RequestsExists_Happy()
        {
            //Act
            List<Users> result = (List<Users>)await _adminRepository.GetRequestingFarmers();

            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(Users));
        }
        [TestMethod]
        public async Task GetRequestedUsers_ThrowsException_WhenException_Bad()
        {
            orchard1ContextMock.Setup(a => a.Users).Throws(new GeneralException());
            await Assert.ThrowsExceptionAsync<GeneralException>(async () => await adminRepository.GetRequestingFarmers());
        }

        [TestMethod]
        public async Task AcceptRequest_ReturnTrue_Happy()
        {
            //Act
            var result = await _adminRepository.AcceptRequest(new Users { UserId = 1 });
            Assert.IsTrue(result);
        }
        [TestMethod]
        public async Task AcceptRequest_ThrowsException_Bad()
        {
            orchard1ContextMock.Setup(a => a.Users).Throws(new GeneralException());
            await Assert.ThrowsExceptionAsync<GeneralException>(async () => await adminRepository.GetRequestingFarmers());
        }

        [TestMethod]
        public async Task DisableUser_ReturnTrue_Happy()
        {
            //Act
            var result = await _adminRepository.DisableUser(new Users { UserId = 2 });
            Assert.IsTrue(result);
        }
        [TestMethod]
        public async Task DisableUser_ThrowsException_Bad()
        {
            //Arrange
            orchard1ContextMock.Setup(a => a.Users).Throws(new GeneralException());
            //Act
            await Assert.ThrowsExceptionAsync<GeneralException>(async () => await adminRepository.GetRequestingFarmers());
        }

        [TestMethod]
        public async Task DiclineRequest_ReturnTrue_Happy()
        {
            //Act
            var result = await _adminRepository.DeclineRequest(new Users { UserId = 3 });
            //Assert
            Assert.IsTrue(result);
        }
        [TestMethod]
        public async Task DeclineRequest_ThrowsException_Bad()
        {
            //Arrange
            orchard1ContextMock.Setup(a => a.Users).Throws(new GeneralException());
            //Act
            await Assert.ThrowsExceptionAsync<GeneralException>(async () => await adminRepository.GetRequestingFarmers());
        }    
        [TestMethod]
        public async Task GetProductTypes_ShouldReturnAllProductTypes_HappyFlow()
        {
            List<ProductType> productTypes = new List<ProductType>
            {
                new ProductType(){ProductTypeId=1,ProductType1="tomato",Msp=50},
                new ProductType(){ProductTypeId=2,ProductType1="potato",Msp=60}
            };
            _orchard1Context.AddRange(productTypes.ToArray());
            _orchard1Context.SaveChanges();
            List<ProductType> testResult = await _adminRepository.GetProductTypes();
            CollectionAssert.AreEqual(productTypes, testResult);
        }
        [TestMethod]
        public async Task GetProductTypes_ThrowsException_BadFlow()
        {
            orchard1ContextMock.Setup(s => s.ProductType).Throws(new GeneralException());
            await Assert.ThrowsExceptionAsync<GeneralException>(async () => await adminRepository.GetProductTypes());
        }
        [TestMethod]
        public async Task UpdateProductMsp_ShouldReturnTrue_HappyFlow()
        {
            orchard1ContextMock.Setup(s => s.ProductType.FindAsync(It.IsAny<int>())).ReturnsAsync(new ProductType() { ProductTypeId = 1, ProductType1 = "tomato", Msp = 50 });
            CancellationToken cancellationToken = CancellationToken.None;
            orchard1ContextMock.Setup(s => s.SaveChangesAsync(cancellationToken)).ReturnsAsync(1);
            bool testResult = await adminRepository.UpdateProductMsp(new ProductType() { ProductTypeId = 1, ProductType1 = "tomato", Msp = 60 });
            
            Assert.IsTrue(testResult);
        }

        [TestMethod]
        public async Task UpdateProductMsp_ShouldReturnFalse_SadFlow()
        {
            orchard1ContextMock.Setup(s => s.ProductType.FindAsync(It.IsAny<int>())).ReturnsAsync(new ProductType() { ProductTypeId = 1, ProductType1 = "tomato", Msp = 50 });
            CancellationToken cancellationToken = CancellationToken.None;
            orchard1ContextMock.Setup(s => s.SaveChangesAsync(cancellationToken)).ReturnsAsync(0);
            bool testResult = await adminRepository.UpdateProductMsp(new ProductType() { ProductTypeId = 1, ProductType1 = "tomato", Msp = 60 });
            Assert.IsFalse(testResult);
        }

        [TestMethod]
        public async Task UpdateProductMsp_ThrowsException_BadFlow()
        {
            orchard1ContextMock.Setup(s => s.ProductType.FindAsync(It.IsAny<int>())).ReturnsAsync(new ProductType() { ProductTypeId = 1, ProductType1 = "tomato", Msp = 50 });
            CancellationToken cancellationToken = CancellationToken.None;
            orchard1ContextMock.Setup(s => s.SaveChangesAsync(cancellationToken)).ThrowsAsync(new DbUpdateException("db exception",new DataNotSavedException()));

            await Assert.ThrowsExceptionAsync<DataNotSavedException>(async () => await adminRepository.UpdateProductMsp(new ProductType() { ProductTypeId = 1, ProductType1 = "tomato", Msp = 60 }));
        }

        [TestMethod]
        public async Task UpdateProductMsp_ThrowsGenericException_BadFlow()
        {
            orchard1ContextMock.Setup(s => s.ProductType.FindAsync(It.IsAny<int>())).ReturnsAsync(new ProductType() { ProductTypeId = 1, ProductType1 = "tomato", Msp = 50 });
            CancellationToken cancellationToken = CancellationToken.None;
            orchard1ContextMock.Setup(s => s.SaveChangesAsync(cancellationToken)).ThrowsAsync(new Exception("db exception", new DataNotSavedException()));

            await Assert.ThrowsExceptionAsync<DataNotSavedException>(async () => await adminRepository.UpdateProductMsp(new ProductType() { ProductTypeId = 1, ProductType1 = "tomato", Msp = 60 }));
        }
    }
}
