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
    public interface ISurveyQuestionsService
    {
        SurveyQuestion Get(int id);
        Paged<SurveyQuestion> GetAll(int pageIndex, int pageSize);
        Paged<SurveyQuestion> GetCreatedBy(int pageIndex, int pageSize, int userId);
        int Add(SurveyQuestionAddRequest model);
        void Update(SurveyQuestionUpdateRequest model);
        void Delete(int id);
    }
}
