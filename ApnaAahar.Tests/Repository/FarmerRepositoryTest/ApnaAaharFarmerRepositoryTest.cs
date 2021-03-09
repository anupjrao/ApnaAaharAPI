using ApnaAahar.Exceptions;
using ApnaAahar.Repository;
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
    public class ApnaAaharFarmerRepositoryTest
    {
        DbContextOptions<Orchard1Context> options = new DbContextOptionsBuilder<Orchard1Context>().UseInMemoryDatabase("ApnaAahar").Options;
        Orchard1Context _orchard1Context;
        IFarmerData farmerRepository;
        IFarmerData _mockedFarmerData;
       Mock<Orchard1Context> _mockedOrchardContext;
        public ApnaAaharFarmerRepositoryTest()
        {
            _orchard1Context = new Orchard1Context(options);
            farmerRepository = new FarmerData(_orchard1Context);
            _mockedOrchardContext = new Mock<Orchard1Context>(options);
            _mockedFarmerData = new FarmerData(_mockedOrchardContext.Object);
        }
        [TestMethod]
        public async Task AddFarmerAsIndividual_UniqueDetails_HappyFlow()
        {
            FarmerDetails farmer = new FarmerDetails
            {
                FarmerId = 6789,
                User = new Users()
                {
                    UserFullName = "clary",
                    PhoneNumber = "8967569878",
                    Email = "clary123@gmail.com",
                    Location = "Hyderabad",
                    Password = "3460a0fa36ba463ea5383717841e045a",
                    UserRole = 1
                }
            };
            _orchard1Context.Users.Add(FarmerTestData());
            _orchard1Context.SaveChanges();
            var result = await farmerRepository.AddFarmerRegistrationDetails(farmer);
            Assert.AreEqual("Successfull", result);
        }
        [TestMethod]
        public async Task AddFarmerAsCommunity_UniqueDetails_HappyFlow()
        {
            FarmerDetails farmer = new FarmerDetails
            {
                FarmerId = 5678,
                User = new Users()
                {
                    UserFullName = "smith",
                    PhoneNumber = "7987569878",
                    Email = "smith123@gmail.com",
                    Location = "Hyderabad",
                    Password = "3460a0fa36ba463ea5383717841e045a",
                    UserRole = 1
                },
                Community = new CommunityDetails()
                {
                    CommunityName = "Remotecommunity"
                }
            };

            _orchard1Context.Users.Add(FarmerTestData());
            _orchard1Context.SaveChanges();
            var result = await farmerRepository.AddFarmerRegistrationDetails(farmer);
            Assert.AreEqual("Successfull", result);
        }
        [TestMethod]
        public async Task AddFarmerAsIndividual_InvalidUserDetails_BadFlow()
        {
            FarmerDetails farmer = new FarmerDetails
            {
                FarmerId = 1234,
                User = FarmerTestData()
            };
            _orchard1Context.Users.Add(FarmerTestData());
            _orchard1Context.SaveChanges();

            await Assert.ThrowsExceptionAsync<DataNotSavedException>(async () => await farmerRepository.AddFarmerRegistrationDetails(farmer));
        }
        [TestMethod]
        public async Task AddFarmerAsIndividual_InvalidFarmerId_BadFlow()
        {
            _orchard1Context.Database.EnsureDeleted();

            FarmerDetails farmer = new FarmerDetails
            {
                FarmerId = 1234,
                User = FarmerTestData()
            };
            FarmerDetails farmerDetails = new FarmerDetails()
            {
                FarmerId = 1234,
                UserId = 5,
                CommunityId = 6
            };
            _orchard1Context.Users.Add(FarmerTestData2());
            _orchard1Context.FarmerDetails.Add(farmerDetails);
            _orchard1Context.SaveChanges();

            await Assert.ThrowsExceptionAsync<DataNotSavedException>(async () => await farmerRepository.AddFarmerRegistrationDetails(farmer));
        }
        [TestMethod]
        public async Task AddFarmerAsCommunity_InvalidUserDetails_BadFlow()
        {
            FarmerDetails farmer = new FarmerDetails
            {
                FarmerId = 1234,
                User = FarmerTestData(),
                Community = new CommunityDetails()
                {
                    CommunityName = "BJPcommunity"
                }
            };
            _orchard1Context.Users.Add(FarmerTestData());
            _orchard1Context.SaveChanges();
            await Assert.ThrowsExceptionAsync<DataNotSavedException>(async () => await farmerRepository.AddFarmerRegistrationDetails(farmer));
        }

        [TestMethod]
        public async Task AddFarmerAsCommunity_InvalidCommunityName_BadFlow()
        {
            FarmerDetails farmer = new FarmerDetails
            {
                FarmerId = 1234,
                User = FarmerTestData(),
                Community = new CommunityDetails()
                {
                    CommunityName = "localcommunity"
                }
            };
            CommunityDetails communityDetails = new CommunityDetails
            {
                CommunityName = "localcommunity"
            };
            _orchard1Context.Users.Add(FarmerTestData2());
            _orchard1Context.CommunityDetails.Add(communityDetails);
            _orchard1Context.SaveChanges();

            await Assert.ThrowsExceptionAsync<DataNotSavedException>(async () => await farmerRepository.AddFarmerRegistrationDetails(farmer));
        }
        [TestMethod]
        public void DeleteContactRequest_HappyFlow()
        {
            _orchard1Context.Database.EnsureDeleted();
            ContactRequest contact = ContactRequestsData();
            _orchard1Context.ContactRequest.Add(contact);
            _orchard1Context.SaveChanges();

            var result = farmerRepository.DeleteContactRequests(1);
            _orchard1Context.Database.EnsureDeleted();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteContactRequest_BadFlow()
        {

            ContactRequest contact = ContactRequestsData();
            _orchard1Context.ContactRequest.Add(contact);
            _orchard1Context.SaveChanges();

            Assert.ThrowsException<AnySqlException>(() => farmerRepository.DeleteContactRequests(2));


        }
        [TestMethod]
        public async Task GetFarmersDetailsByUserId_HappyFlow()
        {
            _orchard1Context.Database.EnsureDeleted();
            Users farmer = FarmerTestDataForBuyers();
            _orchard1Context.Users.Add(farmer);
            _orchard1Context.SaveChanges();
            var farmerDetails = await farmerRepository.GetFarmerByUserId(1);
            _orchard1Context.Database.EnsureDeleted();
            Assert.AreEqual(farmerDetails, farmer);

        }

        [TestMethod]

        public void GetFarmersDetailsByUserId_BadFlow()
        {
            Users farmer = FarmerTestDataForBuyers();

            _orchard1Context.Users.Add(farmer);
            _orchard1Context.SaveChanges();


            _mockedOrchardContext.Setup(u => u.Users).Throws(new AnySqlException());

            Assert.ThrowsExceptionAsync<AnySqlException>(() => farmerRepository.GetFarmerByUserId(1));


        }

        [TestMethod]
        public async Task GetBuyers_HappyFlow()
        {
            _orchard1Context.Database.EnsureDeleted();
            ContactRequest contactRequest = new ContactRequest
            {
                ContactRequestId = 1,
                BuyerId = 2,
                ProductListingId = 3,
                Buyer = BuyersData(),
                ProductListing = ProductListings(),
            };
        
            List<ContactRequest> contactRequestsList = new List<ContactRequest>()
              {
                contactRequest
            };
            _orchard1Context.ContactRequest.AddRange(contactRequestsList);
            _orchard1Context.SaveChanges();
            int userId = 1;
            var listOfBuyers = await farmerRepository.GetBuyers(userId);
            _orchard1Context.Database.EnsureDeleted();

            Assert.IsNotNull(listOfBuyers);
            CollectionAssert.AllItemsAreInstancesOfType(listOfBuyers, typeof(ContactRequest));
        }

        [TestMethod]
        public async Task GetBuyers_BadFlow()
        {
            
          _mockedOrchardContext.Setup(u => u.ContactRequest).Throws(new Exception());
          await Assert.ThrowsExceptionAsync<Exception>(async () =>await _mockedFarmerData.GetBuyers(1));
       
        }

        public static Users FarmerTestData()
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
            return user;
        }

        public static Users FarmerTestDataForBuyers()
        {
            Users user = new Users
            {
                UserId = 1,
                UserFullName = "John",
                PhoneNumber = "9879856787",
                Email = "john123@gmail.com",
                Location = "Hyderabad",
                Password = "3460a0fa36ba463ea5383717841e045a",
                UserRole = 1
            };
            return user;
        }


        public static Users FarmerTestData2()
        {
            Users user = new Users
            {
                UserFullName = "John",
                PhoneNumber = "9869856780",
                Email = "j123@gmail.com",
                Location = "Hyderabad",
                Password = "3460a0fa36ba463ea5383717841e045a",
                UserRole = 1
            };
            return user;
        }
        public ContactRequest ContactRequestsData()
        {
            ContactRequest contactRequest = new ContactRequest
            {
                ContactRequestId = 1,
                BuyerId = 1,
                ProductListingId = 1,


            };
            return contactRequest;
        }
        public ProductListingData ProductListings()
        {
            ProductListingData productListingData = new ProductListingData
            {
                Price = 100,
                FarmerId = 1,
                Quantity = 100,
                ProductTypeId = 2,

                Farmer = FarmersData(),
                ProductType = ProductData(),
            };
            return productListingData;

        }

        private ProductType ProductData()
        {
            ProductType productType = new ProductType
            {
                ProductTypeId = 1,
                ProductType1 = "Potato",
                Msp = 100
            };
            return productType;
        }

        private FarmerDetails FarmersData()
        {
            CommunityDetails community = new CommunityDetails { CommunityName = "LocalCommunity" };
            FarmerDetails farmerDetails = new FarmerDetails
            {
                FarmerId = 1,
                UserId = 1,
                IsApproved = true,
                IsAccountDisabled = false,
                CommunityId = 1,

                Community = community,
                User = BuyersData()

            };
            return farmerDetails;
        }

        public Users BuyersData()
        {
            Users user = new Users
            {
                UserId = 1,
                UserFullName = "John",
                PhoneNumber = "9879856787",
                Email = "john123@gmail.com",
                Location = "Hyderabad",
                Password = "3460a0fa36ba463ea5383717841e045a",
                UserRole = 1
            };
            return user;
        }
    }
}
