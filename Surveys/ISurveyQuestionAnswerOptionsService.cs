using Sabio.Models;
using Sabio.Models.Domain.Survey;
using Sabio.Models.Requests.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface ISurveyQuestionAnswerOptionsService
    {
        SurveyQuestionAnswerOption Get(int id);
        Paged<SurveyQuestionAnswerOption> GetAll(int pageIndex, int pageSize);
        Paged<SurveyQuestionAnswerOption> GetCreatedBy(int pageIndex, int pageSize, int userId);
        int Add(SurveyQuestionAnswerOptionAddRequest model);
        void Update(SurveyQuestionAnswerOptionUpdateRequest model);
        void Delete(int id);
    }
}
