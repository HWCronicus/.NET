using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Surveys
{
    public class SurveyAnswerAddRequest
    {
        [Required]
        public int InstanceId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        public int AnswerOptionId { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public string Answer { get; set; }

        public int AnswerNumber { get; set; }
    }
}
