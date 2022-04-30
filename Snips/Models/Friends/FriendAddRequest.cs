using Sabio.Models.Domain;
using Sabio.Models.Requests.Skills;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Friends
{
    public class FriendAddRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Bio { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Summary { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Headline { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Slug { get; set; }

        [Required]
        [Range(1, 10)]
        public int StatusId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Url { get; set; }

        [Required]
        [Range(1, 5)]
        public int TypeId { get; set; }

        [Required]
        public List<string> Skills { get; set;}

	}
}
