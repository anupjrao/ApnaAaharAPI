using System;
using System.Collections.Generic;

namespace ApnaAahar.Repository.Models
{
    public partial class ProductListingData
    {
        public ProductListingData()
        {
            ContactRequest = new HashSet<ContactRequest>();
        }

        public int ProductListingId { get; set; }
        public int FarmerId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int ProductTypeId { get; set; }

        public FarmerDetails Farmer { get; set; }
        public ProductType ProductType { get; set; }
        public ICollection<ContactRequest> ContactRequest { get; set; }
    }
}
