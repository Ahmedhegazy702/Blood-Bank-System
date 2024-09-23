using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Core.Entites
{
    public class RequestModel:BaseEntity
    {
        public string City { get; set; }
        public string Hospital { get; set; }
        public string BloodType { get; set; }
        public string Mobile { get; set; }
        public string Note { get; set; }

    }
}
