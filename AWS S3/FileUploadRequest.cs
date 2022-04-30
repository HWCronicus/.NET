using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Files
{
    public class FileUploadRequest
    {
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Url { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string FileType { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int CreatedBy { get; set; }
    }
}
