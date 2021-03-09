using ApnaAahar.Exceptions;
using ApnaAahar.Repository;
using ApnaAahar.Repository.Models;
using ApnaAahar.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApnaAahar.Tests.Services
{
    [TestClass]

    public class ApnaAaharAuthenticationServicesTest
    {

       
            Mock<IUserData> _UserDataMock = new Mock<IUserData>();
            IUserServices _UserServices;
            IAuthenticationService _AuthService;
            public ApnaAaharAuthenticationServicesTest()
            {
                var configuration = new Mock<IConfiguration>();
                var configurationSection = new Mock<IConfigurationSection>();
                configurationSection.Setup(a => a.Value).Returns("SECRETKEYTOBEADDED");
                configuration.Setup(a => a.GetSection("SecretKey")).Returns(configurationSection.Object);

                _UserServices = new UserServices(_UserDataMock.Object);
                _AuthService = new AuthenticationService(configuration.Object, _UserDataMock.Object);
            }
            [TestCategory("AuthenticatePasswordTest")]
            [TestMethod]
            public void Authenticate_Password_And_Email_Happy()
            {
                Users user = new Users();
                user.Email = "royankita0707@gmail.com";
                user.Password = "Ankita0707";

                Users userAuthenticated = new Users();
                userAuthenticated.Email = "royankita0707@gmail.com";
                userAuthenticated.Password = "cf7a9638630cfb70024c637d7bf3e605";
                userAuthenticated.UserRole = 1;

                _UserDataMock.Setup(u => u.GetUsersByEmail(user)).ReturnsAsync(userAuthenticated);
                var sut = _AuthService.Authenticate(user);
                Assert.IsNotNull(sut.Result);
            
        }

        [TestCategory("Check_NoUser_Found_Sad")]
        [TestMethod]
        public void Authenticate_NoUser_Found_Sad()
        {
            Users user = new Users();
            user.Email = "royankita070@gmail.com";
            user.Password = "Ankita0707";

            Users userAuthenticated = new Users();
            userAuthenticated.Email = "royankita0707@gmail.com";
            userAuthenticated.Password = "cf7a9638630cfb70024c637d7bf3e605";
            userAuthenticated.UserRole = 1;
            Users users = null;

            _UserDataMock.Setup(u => u.GetUsersByEmail(user)).ReturnsAsync(users);
            var userFound = _AuthService.Authenticate(user);
            Assert.IsNull(userFound.Result);
        }

        [TestCategory("Authenticate_UserBy_Password_Sad")]
        [TestMethod]
        public void Authenticate_UserBy_Password_Sad()
        {
            Users user = new Users();
            user.Email = "royankita0707@gmail.com";
            user.Password = "Ankita070";

            Users userAuthenticated = new Users();
            userAuthenticated.Email = "royankita0707@gmail.com";
            userAuthenticated.Password = "cf7a9638630cfb70024c637d7bf3e605";
            userAuthenticated.UserRole = 1;

            _UserDataMock.Setup(u => u.GetUsersByEmail(user)).ReturnsAsync(userAuthenticated);
            var userFound = _AuthService.Authenticate(user);
            Assert.IsNull(userFound.Result);

        }

        [TestMethod]
        public async Task Authenticate_UserBy_Email_Bad()
        {
            //Arrange
            Users user = new Users
            {
                UserFullName = "John",
                PhoneNumber = "9879856787",
                Email = "john123@gmail.com",
                Password = "3460a0fa36ba463ea5383717841e045a",
                UserRole = 2
            };
            //Act
            _UserDataMock.Setup(b => b.GetUsersByEmail(It.IsAny<Users>())).Throws(new AnySqlException());
            //Assert
            await Assert.ThrowsExceptionAsync<AnySqlException>(async () => await _AuthService.Authenticate(user));
        }


    }
}