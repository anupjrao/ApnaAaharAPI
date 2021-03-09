using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApnaAahar.Repository.Models
{
    public partial class ContactRequest
    {
        public int ContactRequestId { get; set; }
        public int BuyerId { get; set; }
        public int ProductListingId { get; set; }

        public Users Buyer { get; set; }
        public ProductListingData ProductListing { get; set; }

        public static explicit operator ContactRequest(Task<ContactRequest> v)
        {
            throw new NotImplementedException();
        }
    }
}
