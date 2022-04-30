using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain
{
    public class Address
    {
        public int Id { get; set; }
        public string LineOne { get; set; }
        public int SuiteNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public bool IsActive { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }


    }
}
