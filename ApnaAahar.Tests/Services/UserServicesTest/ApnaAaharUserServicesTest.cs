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

namespace ApnaAahar.Tests.Services
{
    [TestClass]
    public class ApnaAaharUserServicesTest
    {
        Mock<IUserData> _userRepositoryMock = new Mock<IUserData>();
        UserServices _userServices;
        public ApnaAaharUserServicesTest()
        {
            _userServices = new UserServices(_userRepositoryMock.Object);
        }
       [TestMethod]
       public async Task AddUserAsBuyer_UniqueEmailPhoneNumber_HappyFlow()
       {
            Users user = new Users
            {
                UserFullName = "John",
                PhoneNumber = "9879856787",
                Email = "john123@gmail.com",
                Location = "Hyderabad",
                Password = "3460a0fa36ba463ea5383717841e045a",
                UserRole = 2
            };
            _userRepositoryMock.Setup(b => b.AddUserRegistrationDetails(It.IsAny<Users>())).ReturnsAsync(true);
            var testResult = await _userServices.AddUserRegistrationDetails(user);
            Assert.AreEqual(true, testResult);
       }

        [TestMethod]
        public async Task AddUserAsBuyer_UniqueEmailPhoneNumber_SadFlow()
        {
            Users user = new Users
            {
                UserFullName = "John",
                PhoneNumber = "9879856787",
                Email = "john123@gmail.com",
                Location = "Hyderabad",
                Password = "3460a0fa36ba463ea5383717841e045a",
                UserRole = 2
            };
            _userRepositoryMock.Setup(b => b.AddUserRegistrationDetails(It.IsAny<Users>())).ReturnsAsync(false);
            var testResult = await _userServices.AddUserRegistrationDetails(user);
            Assert.IsFalse(testResult);
        }

        [TestMethod]
        [TestCategory("LocationUpdateTest")]
        public async Task UpdateUserLocation_ValidLocation_ReturnsNonZero_HappyFlow()
        {
            _userRepositoryMock.Setup(b => b.UpdateUserLocation(It.IsAny<UpdateModel>())).ReturnsAsync(1);
            var result = await _userServices.UpdateUserLocation(new UpdateModel() { Location = "chennai" });
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [TestCategory("LocationUpdateTest")]
        public async Task UpdateUserLocation_ValidLocation_ThrowsDbContextException_BadFlow()
        {
            _userRepositoryMock.Setup(b => b.UpdateUserLocation(It.IsAny<UpdateModel>())).Throws(new DbContextException("db Exception",new Exception()));
            await Assert.ThrowsExceptionAsync<DbContextException>(async ()=>await _userServices.UpdateUserLocation(new UpdateModel() { Location = "chennai" }));
        }
        [TestMethod]
        [TestCategory("LocationUpdateTest")]
        public async Task UpdateUserLocation_NullLocation_ReturnsZero_SadFlow()
        {
            _userRepositoryMock.Setup(b => b.UpdateUserLocation(It.IsAny<UpdateModel>())).ReturnsAsync(0);
            var result = await _userServices.UpdateUserLocation(new UpdateModel() { Location = null });
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        [TestCategory("PasswordUpdateTest")]
        public async Task UpdateUserPassword_ValidCurrentPassword_ReturnTrue_HappyFlow()
        {
            _userRepositoryMock.Setup(b => b.UpdateUserPassword(It.IsAny<UpdateModel>())).ReturnsAsync(true);
            var result = await _userServices.UpdateUserPassword(new UpdateModel
            {
                UserId = 1,
                Location = "Banglore",
                CurrentPassword = "ram",
                NewPassword = "sriram"
            });
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        [TestCategory("PasswordUpdateTest")]

        public async Task UpdateUserPassword_InvalidCurrentPassword_ReturnFalse_SadFlow()
        {
            _userRepositoryMock.Setup(b => b.UpdateUserPassword(It.IsAny<UpdateModel>())).ReturnsAsync(false);
            var result = await _userServices.UpdateUserPassword(new UpdateModel
            {
                UserId = 1,
                Location = "Banglore",
                CurrentPassword = "ram",
                NewPassword = "sriram"
            });
            Assert.IsFalse(result);
        }
        [TestMethod]
        [TestCategory("PasswordUpdateTest")]

        public async Task UpdateUserPassword_IndenticalPasswords_ThrowsIdenticalPasswordException_BadFlow()
        {
            _userRepositoryMock.Setup(b => b.UpdateUserPassword(It.IsAny<UpdateModel>())).ReturnsAsync(false);
           
            await Assert.ThrowsExceptionAsync<IdenticalPasswordException>(async () => await _userServices.UpdateUserPassword(new UpdateModel
            {
                UserId = 1,
                Location = "Banglore",
                CurrentPassword = "sriram",
                NewPassword = "sriram"
            }));
           
        }

        [TestMethod]
        [TestCategory("PasswordUpdateTest")]

        public async Task UpdateUserPassword_ValidPasswords_ThrowsDbContextException_BadFlow()
        {
            _userRepositoryMock.Setup(b => b.UpdateUserPassword(It.IsAny<UpdateModel>())).Throws(new DbContextException("Db exception",new Exception()));

            await Assert.ThrowsExceptionAsync<DbContextException>(async () => await _userServices.UpdateUserPassword(new UpdateModel
            {
                UserId = 1,
                Location = "Banglore",
                CurrentPassword = "sriram",
                NewPassword = "ram"
            }));

        }

        [TestMethod]
        public async Task AddUserAsBuyer_DuplicateEmailPhoneNumber_BadFlow()

        {
            Users user = new Users
            {
                UserFullName = "John",
                PhoneNumber = "9879856787",
                Email = "john123@gmail.com",
                Location = "Hyderabad",
                Password = "3460a0fa36ba463ea5383717841e045a",
                UserRole = 2
            };
            _userRepositoryMock.Setup(b => b.AddUserRegistrationDetails(It.IsAny<Users>())).ThrowsAsync(new DataNotSavedException());
            await Assert.ThrowsExceptionAsync<DataNotSavedException>(async () => await _userServices.AddUserRegistrationDetails(user));
        }

        [TestMethod]
        public void GettingUserByGmail_ShouldreturnUser_WhenEmailmatch_HappyFlow()
        {
            Users UserFormUI = new Users();
            UserFormUI.Email = "testemail@gmail.com";
            Users users = new Users();
            users.Email = "testemail@gmail.com";
            users.PhoneNumber = "9831427449";
            _userRepositoryMock.Setup(u => u.GetUsersByEmail(It.IsAny<Users>())).ReturnsAsync(users);
            var result = _userServices.GetUsersByEmail(UserFormUI);
            Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void GettingUserByGmail_ShouldreturnUser_WhenEmailmatch_SadFlow()
        {
            Users UserFormUI = new Users();
            UserFormUI.Email = "testemail@gmail.com";
            Users users = null;
            _userRepositoryMock.Setup(u => u.GetUsersByEmail(It.IsAny<Users>())).ReturnsAsync(users);
            var sut = _userServices.GetUsersByEmail(UserFormUI);
            Assert.IsNull(sut.Result);
        }

        [TestMethod]
        public async Task GettingUserByGmail_ShouldreturnUser_WhenEmailmatch_BadFlow()
        {
            Users UserFormUI = new Users();
            UserFormUI.Email = "testemail@gmail.com";
            Users users = new Users();
            users.Email = "testemail@gmail.com";
            users.PhoneNumber = "9831427449";
            _userRepositoryMock.Setup(u => u.GetUsersByEmail(It.IsAny<Users>())).Throws(new AnySqlException());
            await Assert.ThrowsExceptionAsync<AnySqlException>(async () => await _userServices.GetUsersByEmail(UserFormUI));
        }

        [TestMethod]
        public void GettingUserByPhone_ShouldreturnUser_WhenPhonematch_HappyFlow()
        {
            Users UserFormUI = new Users();
            UserFormUI.PhoneNumber = "9831427449";
            Users users = new Users();
            users.Email = "testemail@gmail.com";
            users.PhoneNumber = "9831427449";
            _userRepositoryMock.Setup(u => u.GetUserByPhoneNo(It.IsAny<Users>())).ReturnsAsync(users);
            var sut = _userServices.GetUserByPhoneNo(UserFormUI);
            Assert.IsNotNull(sut.Result);
        }

        [TestMethod]
        public void GettingUserByPhone_ShouldreturnUser_WhenPhonematch_SadFlow()
        {
            Users UserFormUI = new Users();
            UserFormUI.PhoneNumber = "9831427449";
            Users users = new Users();
            users = null;
            _userRepositoryMock.Setup(u => u.GetUserByPhoneNo(It.IsAny<Users>())).ReturnsAsync(users);
            var sut = _userServices.GetUserByPhoneNo(UserFormUI);
            Assert.IsNull(sut.Result);
        }

        [TestMethod]
        public async Task GettingUserByPhone_ShouldreturnUser_WhenPhonematch_BadFlow()
        {
            Users UserFormUI = new Users();
            UserFormUI.PhoneNumber = "9831427449";
            Users users = new Users();
            users.Email = "testemail@gmail.com";
            users.PhoneNumber = "9831427449";
            _userRepositoryMock.Setup(u => u.GetUserByPhoneNo(It.IsAny<Users>())).Throws(new AnySqlException());
            await Assert.ThrowsExceptionAsync<AnySqlException>(async () => await _userServices.GetUserByPhoneNo(UserFormUI));
        }


        [TestMethod]
        public void ResetPassword_Shouldreturnbool_WhenPasswordUpdated_HappyFlow()
        {
            Users UserFormUI = new Users();
            UserFormUI.Email = "testemail@gmail.com";
            UserFormUI.Password = "d00f5d5217896fb7fd601412cb890830";
            _userRepositoryMock.Setup(u => u.ResetPassword(It.IsAny<Users>())).ReturnsAsync(true);
            var result = _userServices.ResetPassword(UserFormUI);
            Assert.IsTrue(result.Result);
        }

        [TestMethod]
        public void ResetPassword_Shouldreturnbool_WhenPasswordUpdated_Sad()
        {
            Users UserFormUI = new Users();
            UserFormUI.Email = "testemail@gmail.com";
            UserFormUI.Password = "d00f5d5217896fb7fd601412cb890830";
            _userRepositoryMock.Setup(u => u.ResetPassword(It.IsAny<Users>())).ReturnsAsync(false);
            var result = _userServices.ResetPassword(UserFormUI);
            Assert.IsFalse(result.Result);
        }

        [TestMethod]
        public async Task ResetPassword_Shouldreturnbool_WhenPasswordUpdated_BadFlow()
        {
            Users UserFormUI = new Users();
            UserFormUI.Email = "testemail@gmail.com";
            UserFormUI.Password = "d00f5d5217896fb7fd601412cb890830";
            _userRepositoryMock.Setup(u => u.ResetPassword(It.IsAny<Users>())).Throws(new AnySqlException());
            await Assert.ThrowsExceptionAsync<AnySqlException>(async () => await _userServices.ResetPassword(UserFormUI));
        }

        [TestMethod]
        public async Task AddContactRequest_ShouldReturnBool_WhenRequestAdded_HappyFlow()
        {
            Users user = new Users();
            ProductListingData productListingData = new ProductListingData();
            _userRepositoryMock.Setup(u => u.AddContactRequest(productListingData, user)).ReturnsAsync(true);
            Boolean addedRequest = await _userServices.AddContactRequest(productListingData,user);
            Assert.IsTrue(addedRequest);
        }
        [TestMethod]
        public async Task AddContactRequest_ShouldReturnBool_WhenRequestAdded_BadFlow()
        {
            Users user = new Users();
            ProductListingData productListingData = new ProductListingData();
            _userRepositoryMock.Setup(u => u.AddContactRequest(productListingData, user)).ThrowsAsync(new DataNotSavedException());
            Boolean addedRequest = await _userServices.AddContactRequest(productListingData, user);
            Assert.IsFalse(addedRequest);
        }
        [TestMethod]
        public async Task GetBuyerContactRequests_ShouldReturnList_WhenPresentAsync()
        {
            List<ContactRequest> contactRequests = new List<ContactRequest>()
            {
                new ContactRequest(){ BuyerId = 1, ContactRequestId = 1, ProductListingId=1},
                new ContactRequest(){ BuyerId = 1, ContactRequestId = 2, ProductListingId=2}
            };
            _userRepositoryMock.Setup(mockRepo => mockRepo.GetBuyerContactRequests(1)).ReturnsAsync(contactRequests);
            List<ContactRequest> contactRequestsActual = (List<ContactRequest>)await _userServices.GetBuyerContactRequests(1);
            CollectionAssert.AreEquivalent(contactRequests, contactRequestsActual);
        }
        [TestMethod]
        public async Task GetBuyerContactRequests_ShouldThrowException_BadFlow()
        {
            _userRepositoryMock.Setup(mockRepo => mockRepo.GetBuyerContactRequests(1)).ThrowsAsync(new Exception());
            await Assert.ThrowsExceptionAsync<Exception>(async()=>{
                await _userServices.GetBuyerContactRequests(1);
            });
        }
    }
}
