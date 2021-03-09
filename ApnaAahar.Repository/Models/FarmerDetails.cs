using System;
using System.Collections.Generic;

namespace ApnaAahar.Repository.Models
{
    public partial class FarmerDetails
    {
        public FarmerDetails()
        {
            ProductListingData = new HashSet<ProductListingData>();
        }

        public int FarmerId { get; set; }
        public int? UserId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsAccountDisabled { get; set; }
        public int? CommunityId { get; set; }

        public CommunityDetails Community { get; set; }
        public Users User { get; set; }
        public ICollection<ProductListingData> ProductListingData { get; set; }
    }
}
