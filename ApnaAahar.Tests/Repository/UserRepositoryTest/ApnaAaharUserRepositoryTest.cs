using ApnaAahar.Repository;
using ApnaAahar.Exceptions;
using ApnaAahar.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ApnaAahar.Tests.Repository
{
    [TestClass]
    public class ApnaAaharUserRepositoryTest
    {
        DbContextOptions<Orchard1Context> options = new DbContextOptionsBuilder<Orchard1Context>().UseInMemoryDatabase("ApnaAahar").Options;
        Mock<Orchard1Context> _orchard1Context;
        IUserData userRepository;
        public ApnaAaharUserRepositoryTest()
        {
            _orchard1Context = new Mock<Orchard1Context>(options);
            userRepository = new UserData(_orchard1Context.Object);
        }

        [TestMethod]
        [TestCategory("LocationUpdateTest")]
        public async Task UpdateUserLocation_ValidLocation_ReturnsNonZero_GoodFlow()
        {
            //Arrange
            _orchard1Context.Setup(b => b.Users.FindAsync(It.IsAny<int>())).ReturnsAsync(new Users() { UserId = 1 });
            CancellationToken cancellationToken = CancellationToken.None;
            _orchard1Context.Setup(b => b.SaveChangesAsync(cancellationToken)).ReturnsAsync(1);
            //Act
            var result = await userRepository.UpdateUserLocation(new UpdateModel() {
                UserId = 1,
                Location = "Banglore",
            });
            //Assert
            Assert.AreEqual(1, result);
        }
        [TestMethod]
        [TestCategory("LocationUpdateTest")]

        public async Task UpdateUserLocation_NullLocation_ReturnsZero_SadFlow()
        {
            //Arrange
            _orchard1Context.Setup(b => b.Users.FindAsync(It.IsAny<int>())).ReturnsAsync(new Users() { UserId = 1 });
            CancellationToken cancellationToken = CancellationToken.None;
            _orchard1Context.Setup(b => b.SaveChangesAsync(cancellationToken)).ReturnsAsync(0);
            //Act
            var result = await userRepository.UpdateUserLocation(new UpdateModel()
            {
                UserId = 1,
                Location = null,
            });
            //Assert
            Assert.AreEqual(0, result);
        }
        [TestMethod]
        [TestCategory("LocationUpdateTest")]

        public async Task UpdateUserLocation_Location_ThrowsDbCOntextException_BadFlow()
        {
            //Arrange
            _orchard1Context.Setup(b => b.Users.FindAsync(It.IsAny<int>())).ReturnsAsync(null as Users);
            CancellationToken cancellationToken = CancellationToken.None;
            _orchard1Context.Setup(b => b.SaveChangesAsync(cancellationToken)).Throws(new DbUpdateException("db Exception", new Exception()));
            //Act
            //Assert
            await Assert.ThrowsExceptionAsync<DbContextException>(async () => await userRepository.UpdateUserLocation((new UpdateModel
            {
                UserId = 1,
                Location = "Banglore",
                CurrentPassword = "3460a0fa36ba463ea5383717841e045a",
                NewPassword = "4460a0fa36ba463ea5383717841e045a"
            })));
        }

        [TestMethod]
        [TestCategory("PasswordUpdateTest")]
        public async Task UpdateUserPassword_ValidCurrentPassword_ReturnTrue_GoodFlow()
        {
            //Arrange
            _orchard1Context.Setup(b => b.Users.FindAsync(It.IsAny<int>())).ReturnsAsync(new Users() { UserId = 1, Password = "3460a0fa36ba463ea5383717841e045a" });
            CancellationToken cancellationToken = CancellationToken.None;
            _orchard1Context.Setup(b => b.SaveChangesAsync(cancellationToken)).ReturnsAsync(1);
            //Act
            var result = await userRepository.UpdateUserPassword(new UpdateModel
            {
                UserId = 1,
                Location = "Banglore",
                CurrentPassword = "3460a0fa36ba463ea5383717841e045a",
                NewPassword = "4460a0fa36ba463ea5383717841e045a"
            });
            //Assert
            Assert.IsTrue(result);
        }
        [TestMethod]
        [TestCategory("PasswordUpdateTest")]

        public async Task UpdateUserPassword_InvalidCurrentPassword_ReturnFalse_SadFlow()
        {
            //Arrange
            _orchard1Context.Setup(b => b.Users.FindAsync(It.IsAny<int>())).ReturnsAsync(new Users() { UserId = 1, Password = "4460a0fa36ba463ea5383717841dsa3a" });
            CancellationToken cancellationToken = CancellationToken.None;
            _orchard1Context.Setup(b => b.SaveChangesAsync(cancellationToken)).ReturnsAsync(0);
            //Act
            var result = await userRepository.UpdateUserPassword(new UpdateModel
            {
                UserId = 1,
                Location = "Banglore",
                CurrentPassword = "3460a0fa36ba463ea5383717841e045a",
                NewPassword = "4460a0fa36ba463ea5383717841e045a"
            });
            //Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        [TestCategory("PasswordUpdateTest")]
        public async Task UpdateUserPassword_Password_ThrowsDbContextException_BadFlow()
        {
            //Arrange
            _orchard1Context.Setup(b => b.Users.FindAsync(It.IsAny<int>())).ReturnsAsync(null as Users);
            CancellationToken cancellationToken = CancellationToken.None;
            _orchard1Context.Setup(b => b.SaveChangesAsync(cancellationToken)).Throws(new DbUpdateException("db Exception", new Exception()));
            //Act
            //Assert
            await Assert.ThrowsExceptionAsync<DbContextException>(async ()=>await userRepository.UpdateUserPassword((new UpdateModel
            {
                UserId = 1,
                Location = "Banglore",
                CurrentPassword = "3460a0fa36ba463ea5383717841e045a",
                NewPassword = "4460a0fa36ba463ea5383717841e045a"
            })));

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
                UserRole = 1
            };
            CancellationToken cancellationToken = CancellationToken.None;
            _orchard1Context.Setup(o => o.SaveChangesAsync(cancellationToken)).Returns(Task.FromResult(1));
            bool isUserAdded = await userRepository.AddUserRegistrationDetails(user);
            Assert.IsTrue(isUserAdded);
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
                UserRole = 1
            };
            CancellationToken cancellationToken = CancellationToken.None;
            _orchard1Context.Setup(o => o.SaveChangesAsync(cancellationToken)).Returns(Task.FromResult(0));
            bool isUserAdded = await userRepository.AddUserRegistrationDetails(user);
            Assert.IsFalse(isUserAdded);
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
                UserRole = 1
            };
            CancellationToken cancellationToken = CancellationToken.None;
            _orchard1Context.Setup(o =>o.SaveChangesAsync(cancellationToken)).ThrowsAsync(new DataNotSavedException());
            await Assert.ThrowsExceptionAsync<DataNotSavedException>(async () => await userRepository.AddUserRegistrationDetails(user));
        }

    }
}
