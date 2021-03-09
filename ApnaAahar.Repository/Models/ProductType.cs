using System;
using System.Collections.Generic;

namespace ApnaAahar.Repository.Models
{
    public partial class ProductType
    {
        public ProductType()
        {
            ProductListingData = new HashSet<ProductListingData>();
        }

        public int ProductTypeId { get; set; }
        public string ProductType1 { get; set; }
        public double Msp { get; set; }

        public ICollection<ProductListingData> ProductListingData { get; set; }
    }
}
