using ApnaAahar.Exceptions;
using ApnaAahar.Repository;
using ApnaAahar.Repository.Models;
using ApnaAahar.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApnaAahar.Tests
{
    [TestClass]
    public class ApnaAaharFarmerServicesTest
    {
        Mock<IFarmerData> _farmerRepositoryMock = new Mock<IFarmerData>();
        IFarmerServices _farmerServices;
        public ApnaAaharFarmerServicesTest()
        {
            _farmerServices = new FarmerServices(_farmerRepositoryMock.Object);
        }

        [TestMethod]
        public async Task AddFarmerAsIndividual_UniqueDetails_HappyFlow()
        {
            FarmerDetails farmer = new FarmerDetails
            {
                FarmerId=1234,
                User = FarmerTestData(),
            };
            _farmerRepositoryMock.Setup(f => f.AddFarmerRegistrationDetails(It.IsAny<FarmerDetails>())).ReturnsAsync("Successfull");
            var testResult = await _farmerServices.AddFarmerRegistrationDetails(farmer);
            Assert.AreEqual("Successfull", testResult);
        }
        [TestMethod]
        public async Task AddFarmerAsCommunity_UniqueDetails_HappyFlow()
        {
            CommunityDetails community = new CommunityDetails { CommunityName = "LocalCommunity" };
            FarmerDetails farmer = new FarmerDetails
            {
                FarmerId = 1234,
                User = FarmerTestData(),
                Community = community
            };
            _farmerRepositoryMock.Setup(f => f.AddFarmerRegistrationDetails(It.IsAny<FarmerDetails>())).ReturnsAsync("Successfull");
            var testResult = await _farmerServices.AddFarmerRegistrationDetails(farmer);
            Assert.AreEqual("Successfull", testResult);
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
       
       [TestMethod]
        public async Task AddFarmerAsIndividual_InvalidUserDetails_BadFlow()
        {
            FarmerDetails farmer = new FarmerDetails
            {
                FarmerId = 1234,
                User = FarmerTestData(),
            };
            _farmerRepositoryMock.Setup(b => b.AddFarmerRegistrationDetails(It.IsAny<FarmerDetails>())).ThrowsAsync(new DataNotSavedException("Duplication"));
            await Assert.ThrowsExceptionAsync<DataNotSavedException>(async () => await _farmerServices.AddFarmerRegistrationDetails(farmer));
        }
        [TestMethod]
        public async Task AddFarmerAsCommunity_InvalidUserDetails_BadFlow()
        {
            CommunityDetails community = new CommunityDetails { CommunityName = "LocalCommunity" };
            FarmerDetails farmer = new FarmerDetails
            {
                FarmerId = 1234,
                User = FarmerTestData(),
                Community = community
            };
            _farmerRepositoryMock.Setup(b => b.AddFarmerRegistrationDetails(It.IsAny<FarmerDetails>())).ThrowsAsync(new DataNotSavedException("Duplication"));
            await Assert.ThrowsExceptionAsync<DataNotSavedException>(async () => await _farmerServices.AddFarmerRegistrationDetails(farmer));
        }
        [TestMethod]
        public async Task AddFarmerAsCommunity_InvalidCommunityName_BadFlow()
        {
            CommunityDetails community = new CommunityDetails { CommunityName = "LocalCommunity" };
            FarmerDetails farmer = new FarmerDetails
            {
                FarmerId = 1234,
                User = FarmerTestData(),
                Community = community
            };
            _farmerRepositoryMock.Setup(b => b.AddFarmerRegistrationDetails(It.IsAny<FarmerDetails>())).ThrowsAsync(new DataNotSavedException("Community Name Already Exists"));
            await Assert.ThrowsExceptionAsync<DataNotSavedException>(async () => await _farmerServices.AddFarmerRegistrationDetails(farmer));
        }
        [TestMethod]
        public async Task AddFarmerAsIndividual_InvalidFarmerID_BadFlow()
        {
            FarmerDetails farmer = new FarmerDetails
            {
                FarmerId = 1234,
                User = FarmerTestData(),
            };
            _farmerRepositoryMock.Setup(f => f.AddFarmerRegistrationDetails(It.IsAny<FarmerDetails>())).ThrowsAsync(new DataNotSavedException("FarmerId already exists"));        
            await Assert.ThrowsExceptionAsync<DataNotSavedException>(async () => await _farmerServices.AddFarmerRegistrationDetails(farmer));
        }

        [TestMethod]

        public void DeleteContactRequests_HappyFlow()
        {
            int contactReqId = 2;
            _farmerRepositoryMock.Setup(b => b.DeleteContactRequests(It.IsAny<int>())).Returns(true);
            var isDeleted = _farmerServices.DeleteBuyerRequest(contactReqId);
            Assert.IsTrue(isDeleted);

        }

        [TestMethod]

        public void DeleteContactRequests_SadFlow()
        {
            int contactReqId = 2;
            _farmerRepositoryMock.Setup(b => b.DeleteContactRequests(It.IsAny<int>())).Returns(false);
            var isDeleted = _farmerServices.DeleteBuyerRequest(contactReqId);
            Assert.IsFalse(isDeleted);

        }

        [TestMethod]

        public void DeleteContactRequests_BadFlow()
        {
            int contactReqId = 2;
            _farmerRepositoryMock.Setup(f => f.DeleteContactRequests(It.IsAny<int>())).Throws(new AnySqlException("Something Went Wrong"));        
             Assert.ThrowsException<AnySqlException>( () =>  _farmerServices.DeleteBuyerRequest(contactReqId));


        }
        [TestMethod]
        public async Task GetBuyers_HappyFlow()
        {
           
            ContactRequest contactRequest = new ContactRequest
            {
                ContactRequestId = 1,
                BuyerId = 2,
                ProductListingId = 3,
                Buyer = BuyersData(),
                ProductListing = ProductListings(),
                 };
            ContactRequest contactRequest2 = new ContactRequest
            {
                ContactRequestId = 2,
                BuyerId = 2,
                ProductListingId = 3,
                Buyer = BuyersData(),
                ProductListing = ProductListings(),
            };
            List<ContactRequest> contactRequestsList = new List<ContactRequest>()
              {
                contactRequest,
                contactRequest2
            };
            _farmerRepositoryMock.Setup(f => f.GetBuyers(It.IsAny<int>())).ReturnsAsync(contactRequestsList);
            var listOfBuyers = await _farmerServices.GetBuyers(1);
            Assert.AreEqual(listOfBuyers,contactRequestsList);
        }

        [TestMethod]
        public async Task GetBuyers_SadFlow()
        {


            List<ContactRequest> contactRequestsList = null;
            _farmerRepositoryMock.Setup(f => f.GetBuyers(It.IsAny<int>())).ReturnsAsync(contactRequestsList);
            var listOfBuyers = await _farmerServices.GetBuyers(1);
            Assert.IsNull(listOfBuyers);
        }

        [TestMethod]
        public async Task GetBuyers_BadFlow()
        {

            _farmerRepositoryMock.Setup(f => f.GetBuyers(It.IsAny<int>())).Throws(new AnySqlException("Something Went Wrong"));
            await Assert.ThrowsExceptionAsync<AnySqlException>(async () => await _farmerServices.GetBuyers(1));
        
        }

        [TestMethod]

        public async Task GetFarmersDetailsByUserId_HappyFlow()
        {
            Users farmer = FarmerTestData();
             _farmerRepositoryMock.Setup(f => f.GetFarmerByUserId(It.IsAny<int>())).ReturnsAsync(farmer);
            var farmerDetails = await _farmerServices.GetFarmersDetailsByUserId(1);
            Assert.AreEqual(farmerDetails, farmer);

        }
        [TestMethod]

        public async Task GetFarmersDetailsByUserId_SadFlow()
        {
            Users farmer = null;
            _farmerRepositoryMock.Setup(f => f.GetFarmerByUserId(It.IsAny<int>())).ReturnsAsync(farmer);
            var farmerDetails = await _farmerServices.GetFarmersDetailsByUserId(1);
            Assert.IsNull(farmerDetails);

        }
        [TestMethod]

        public async Task GetFarmersDetailsByUserId_BadFlow()
        {

            _farmerRepositoryMock.Setup(f => f.GetFarmerByUserId(It.IsAny<int>())).Throws(new AnySqlException("Something Went Wrong"));
            await Assert.ThrowsExceptionAsync<AnySqlException>(async () => await _farmerServices.GetFarmersDetailsByUserId(1));

        }
        public ProductListingData ProductListings()
        {
            ProductListingData productListingData = new ProductListingData
            {
                Price = 100,
                FarmerId = 1,
                Quantity = 100,
                ProductTypeId = 2,

                Farmer = FarmerData(),
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

        private FarmerDetails FarmerData()
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
