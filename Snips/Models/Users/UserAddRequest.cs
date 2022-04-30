using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Users
{
    public class UserAddRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string firstName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string lastName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string password { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string passwordConfirm { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string avatarUrl { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string tenantId { get; set; }
    }
}
