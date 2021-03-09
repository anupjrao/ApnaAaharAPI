using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApnaAaharAPI.Model
{

    /// <summary>
    /// Response class for sending email to buyers on acceptance/declination of requests
    /// </summary>
    public class BuyerResponse
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public bool isAccepted { get; set; }

        public int ContactRequestId { get; set; }

        public string PhoneNumber { get; set; }


    }
}
