using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Files
{
    public class FileAddRequest
    {
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Url { get; set; }
        [Required]
        public int FileTypeId { get; set; }
        [Required]
        public int CreatedBy { get; set; }
    }
}
