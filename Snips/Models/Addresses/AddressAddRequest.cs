using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Addresses
{
    public class AddressAddRequest
    {
        [Required]
        [StringLength(100,MinimumLength = 3)]
        public string LineOne { get; set; }
        [Required]
        [Range(1,10)]
        public int SuiteNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        [Range(-90, 90)]
        public double Lat { get; set; }
        [Required]
        [Range(-180, 180)]
        public double Long { get; set; }



    }
}
