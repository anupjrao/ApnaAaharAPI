using System;
using System.Collections.Generic;

namespace ApnaAahar.Repository.Models
{
    public partial class Otp
    {
        public int OtpId { get; set; }
        public int UserId { get; set; }
        public int Otp1 { get; set; }
        public DateTime? Timestamp { get; set; }

        public Users User { get; set; }
    }
}
