using System;
using System.Collections.Generic;
using System.Text;

namespace ApnaAahar.Repository.Models
{
    public class UpdateModel
    {
        public int UserId { get; set; }
        public string Location { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

    }
}
