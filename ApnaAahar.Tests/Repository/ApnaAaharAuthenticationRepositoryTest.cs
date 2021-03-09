using ApnaAahar.Exceptions;
using ApnaAahar.Repository;
using ApnaAahar.Repository.Models;
using ApnaAahar.Tests.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApnaAahar.Tests.Services
{
    [TestClass]
    public class ApnaAaharAuthenticationRepositoryTest
    {
        private DbContextOptions<Orchard1Context> options = new DbContextOptionsBuilder<Orchard1Context>().UseInMemoryDatabase("Users").Options;
        IUserData userData;
        IUserData _mockedUserData;
        Orchard1Context _orchard1Context;
        Mock<Orchard1Context> _mockedOrchardContext;
        public ApnaAaharAuthenticationRepositoryTest()
        {
            _orchard1Context = new Orchard1Context(options);
            _mockedOrchardContext = new Mock<Orchard1Context>(options);
            _mockedUserData = new UserData(_mockedOrchardContext.Object);
            userData = new UserData(_orchard1Context);
        }

        /// <summary>
        /// A list of users for seeding in inMemoryDb
        /// </summary>
        /// <returns></returns>
        public List<Users> GetUsers()
        {
            List<Users> userExisting = new List<Users>();

            Users user1 = new Users()
            {
                UserId = 1,
                Email = "royankita07@gmail.com",
                Password = "Ankita0707",
            };

            Users user2 = new Users()
            {
                UserId = 2,
                Email = "ar@gmail.com",
                Password = "Ankita07070",

            };
            userExisting.Add(user2);
            userExisting.Add(user1);

            return userExisting;
        }
        public List<ProductListingData> GetProductListingData()
        {
            List<ProductListingData> productListings = new List<ProductListingData>() {
                new ProductListingData(){ ProductListingId = 1},
                new ProductListingData(){ ProductListingId = 2}
            };
            return productListings;
        }

        public List<ContactRequest> GetContactRequest()
        {
            List<ContactRequest> productListings = new List<ContactRequest>() {
                new ContactRequest(){ ProductListingId = 1, BuyerId=1},
                new ContactRequest(){ ProductListingId = 2, BuyerId=2}
            };
            return productListings;
        }

        [TestMethod]
        public void GetUser_ShouldReturnUser_WithGivenEmail_Happy()
        {
            _orchard1Context.Database.EnsureDeleted();
            Users user = new Users();
            user.Email = "royankita07@gmail.com";

            Seed(_orchard1Context);
            Users expectedResult = _orchard1Context.Users.ToList()[1];
            var results = userData.GetUsersByEmail(user);
            Assert.AreEqual(expectedResult, results.Result);
        }

        [TestMethod]
        public void GetUser_ShouldReturnNull_WithGivenEmail_Sad()
        {
            _orchard1Context.Database.EnsureDeleted();
            Users user = new Users();
            user.Email = "royankita0@gmail.com";
            Seed(_orchard1Context);
            var results = userData.GetUsersByEmail(user);
            Assert.IsNull(results.Result);
        }
        [TestMethod]
        public void GetUser_ShouldThrowException_WithGivenEmail_Bad()
        {
            Users user = new Users();
            user.Email = "royankita0@gmail.com";
            _mockedOrchardContext.Setup(u => u.Users).Throws(new AnySqlException());
             Assert.ThrowsExceptionAsync<AnySqlException>( () => userData.GetUsersByEmail(user));


        }

        private void Seed(Orchard1Context _orchard1Context)
        {
            _orchard1Context.Database.EnsureDeleted();
            _orchard1Context.AddRange(GetUsers());
            _orchard1Context.AddRange(GetContactRequest());
            _orchard1Context.AddRange(GetProductListingData());
            _orchard1Context.SaveChanges();
        }
        [TestMethod]
        public void AddContactRequest_ShouldReturnBool_Sad()
        {
            Users user = new Users();
            user.UserId = 1;
            ProductListingData productListingData = new ProductListingData();
            productListingData.ProductListingId = 1;
            Seed(_orchard1Context);
            var result = userData.AddContactRequest(productListingData,user);
            Assert.IsFalse(result.Result);
        }
        [TestMethod]
        public void AddContactRequest_ShouldReturnBool_Happy()
        {
            _orchard1Context.Database.EnsureDeleted();
            Users user = new Users();
            user.UserId = 3;
            ProductListingData productListingData = new ProductListingData();
            productListingData.ProductListingId = 1;
            Seed(_orchard1Context);
            var result = userData.AddContactRequest(productListingData, user);
            Assert.IsTrue(result.Result);
        }
        [TestMethod]
        public async Task AddContactRequest_ShouldThrowException_WhenException_Bad()
        {
            Users user = new Users();
            user.UserId = 3;
            ProductListingData productListingData = new ProductListingData();
            productListingData.ProductListingId = 1;
            //Arrange
            _mockedOrchardContext.Setup(o => o.ProductListingData).Throws(new GeneralException());
            //Act
            //Assert
            await Assert.ThrowsExceptionAsync<DataNotSavedException>(async () => await _mockedUserData.AddContactRequest(productListingData, user));
        }
        [TestMethod]
        public async Task GetBuyerContactRequests_ShouldReturnBool_Happy()
        {
            _orchard1Context.Database.EnsureDeleted();
            Seed(_orchard1Context);
            List<ContactRequest> result = (List<ContactRequest>)await userData.GetBuyerContactRequests(1);
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(ContactRequest));
        }

        [TestMethod]
        public async Task GetBuyerContactRequests_ShouldThrowException_WhenException_Bad()
        {
            //Arrange
            _mockedOrchardContext.Setup(mockedContext => mockedContext.ContactRequest).Throws(new Exception());
            //Act
            //Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _mockedUserData.GetBuyerContactRequests(1));
        }
    }
}


