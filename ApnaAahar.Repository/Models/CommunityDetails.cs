using System;
using System.Collections.Generic;

namespace ApnaAahar.Repository.Models
{
    public partial class CommunityDetails
    {
        public CommunityDetails()
        {
            FarmerDetails = new HashSet<FarmerDetails>();
        }

        public int CommunityId { get; set; }
        public string CommunityName { get; set; }

        public ICollection<FarmerDetails> FarmerDetails { get; set; }
    }
}
