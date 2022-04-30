using Sabio.Models.Requests.CodeingChallenge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.CodingChallenge
{
    public class CourseUpdateRequest : CourseAddRequest
    {
        public int Id { get; set; }
    }
}
