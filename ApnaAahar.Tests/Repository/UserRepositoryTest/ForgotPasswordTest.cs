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
namespace ApnaAahar.Tests.Repository.UserRepositoryTest
{
    [TestClass]
    public class ForgotPasswordTest
    {
        DbContextOptions<Orchard1Context> options = new DbContextOptionsBuilder<Orchard1Context>().UseInMemoryDatabase("ApnaAahar").Options;
        Mock<Orchard1Context> _orchard1Context;
        Orchard1Context orcharContext;
        IUserData userRepository;
        private readonly UserData userError;

        public object CancellationTokencancellationToken { get; }
        public ForgotPasswordTest()
        {
            _orchard1Context = new Mock<Orchard1Context>(options);
            orcharContext = new Orchard1Context(options);
            orcharContext.Database.EnsureDeleted();
            userRepository = new UserData(orcharContext);
            userError = new UserData(_orchard1Context.Object);
        }

        [TestMethod]
        public async Task GetUser_ByEmail_ShouldreturnUser_Happy()
        {

            Users users = new Users();
            Seed(orcharContext);
            users.Email = "john1234@gmail.com";
            Users results = await userRepository.GetUsersByEmail(users);
            List<Users> userList = orcharContext.Users.ToList();
            Users temp = userList.FirstOrDefault(t => t.Email == "john1234@gmail.com");
            orcharContext.Dispose();
            Assert.AreEqual(temp, results);
        }

        [TestMethod]
        public async Task GetUser_ByEmail_ShouldreturnUser_Sad()
        {

            Users users = new Users();
            Seed(orcharContext);
            users.Email = "john12345@gmail.com";
            Users results = await userRepository.GetUsersByEmail(users);
            orcharContext.Dispose();
            Assert.IsNull(results);
        }

        [TestMethod]
        public async Task GetUser_ByEmail_ShouldreturnUser_Bad()
        {

            Users users = new Users();
            users.Email = "john1234@gmail.com";
            _orchard1Context.Setup(u => u.Users).Throws(new AnySqlException());
            await Assert.ThrowsExceptionAsync<AnySqlException>(async () => await userError.GetUsersByEmail(users));
        }

        [TestMethod]
        public async Task GetUser_ByPhone_ShouldreturnUser_Happy()
        {

            Users users = new Users();
            Seed(orcharContext);
            users.PhoneNumber = "1234567890";
            var results = await userRepository.GetUserByPhoneNo(users);
            List<Users> userList = orcharContext.Users.ToList(); ;
            var temp = userList.FirstOrDefault(t=>t.PhoneNumber=="1234567890");
            orcharContext.Dispose();
            Assert.AreEqual(temp, results);
        }

        [TestMethod]
        public async Task GetUser_ByPhone_ShouldreturnUser_Sad()
        {

            Users users = new Users();
            Seed(orcharContext);
            users.PhoneNumber = "9678787986";
            var results = await userRepository.GetUserByPhoneNo(users);
            Assert.IsNull(results);
        }

        [TestMethod]
        public async Task GetUser_ByPhone_ShouldreturnUser_Bad()
        {

            Users users = new Users();
            users.PhoneNumber = "9678787986";
            _orchard1Context.Setup(u => u.Users).Throws(new AnySqlException());
            await Assert.ThrowsExceptionAsync<AnySqlException>(async () => await userError.GetUserByPhoneNo(users));
        }

        [TestMethod]
        public async Task ResetPaasword_ByPhone_ShouldReturnTrue_Happy()
        {

            Users users = new Users();
            Seed(orcharContext);
            users.Email = "";
            users.PhoneNumber = "1234567890";
            users.Password = "d00f5d5217896fb7fd601412cb890830";
            var results = await userRepository.ResetPassword(users);
            orcharContext.Dispose();
            Assert.IsTrue(results);
        }

        [TestMethod]
        public async Task ResetPaasword_ByPhone_ShouldReturnTrue_Sad()
        {

            Users users = new Users();
            Seed(orcharContext);
            List<Users> userList = orcharContext.Users.ToList();
            users.Email = "";
            users.PhoneNumber = "1234567890";
            users.Password = userList.FirstOrDefault(t => t.PhoneNumber == "1234567890").Password;
            var results = await userRepository.ResetPassword(users);
            orcharContext.Dispose();
            Assert.IsFalse(results);
        }

        [TestMethod]
        public async Task ResetPaasword_ByPhone_ShouldReturnTrue_Bad()
        {

            Users users = new Users();
            Seed(orcharContext);
            List<Users> userList = orcharContext.Users.ToList();
            users.Email = "";
            users.PhoneNumber = "1234567891";
            users.Password = "d00f5d5217896fb7fd601412cb890830";
            await Assert.ThrowsExceptionAsync<AnySqlException>(async () => await userRepository.ResetPassword(users));
        }

        [TestMethod]
        public async Task ResetPaasword_ByEmail_ShouldReturnTrue_Haapy()
        {

            Users users = new Users();
            Seed(orcharContext);
            users.Email = "john1234@gmail.com";
            users.PhoneNumber = "";
            users.Password = "d00f5d5217896fb7fd601412cb890830";
            var results = await userRepository.ResetPassword(users);
            orcharContext.Dispose();
            Assert.IsTrue(results);
        }

        [TestMethod]
        public async Task ResetPaasword_ByEmail_ShouldReturnTrue_Sad()
        {
            Users users = new Users();
            Seed(orcharContext);
            List<Users> userList = orcharContext.Users.ToList();
            users.Email = "john1234@gmail.com";
            users.PhoneNumber = "";
            users.Password = userList.FirstOrDefault(t => t.Email == "john1234@gmail.com").Password;
            var results = await userRepository.ResetPassword(users);
            orcharContext.Dispose();
            Assert.IsFalse(results);
        }

        [TestMethod]
        public async Task ResetPaasword_ByEmail_ShouldReturnTrue_Bad()
        {
            Users users = new Users();
            Seed(orcharContext);
            List<Users> userList = orcharContext.Users.ToList();
            users.Email = "john1267@gmail.com";
            users.PhoneNumber = "";
            users.Password = "d00f5d5217896fb7fd601412cb890830";
            await Assert.ThrowsExceptionAsync<AnySqlException>(async () => await userRepository.ResetPassword(users));
        }

        private void Seed(Orchard1Context orchard1Context)
        {
            List<Users> check = orchard1Context.Users.ToList();
            if (check.Count==0)
            {
                orchard1Context.AddRange(new UserTestData().GetUsers().ToArray());
                orchard1Context.SaveChanges();
            }

        }
    }
}
