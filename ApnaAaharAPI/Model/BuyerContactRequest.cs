using ApnaAahar.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApnaAaharAPI.Model
{
    public class BuyerContactRequest
    {
        public ProductListingData ProductListing { get; set; }
        public Users User { get; set; }
    }
}
