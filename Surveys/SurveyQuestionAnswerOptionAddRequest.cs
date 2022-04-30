using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Surveys
{
    public class SurveyQuestionAnswerOptionAddRequest
    {
        [Required]
        public int QuestionId { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public string Text { get; set; }

        [StringLength(100, MinimumLength = 1)]
        public string Value { get; set; }

        [StringLength(200, MinimumLength = 2)]
        public string AdditionalInfo { get; set; }

        [Required]
        public int CreatedBy { get; set; }
    }
}
