using Sabio.Models.Domain.CodeingChallenge;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.CodeingChallenge
{
    public class CourseAddRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int SeasonTermId { get; set; }
        [Required]
        public int TeacherId { get; set; }
    }
}
