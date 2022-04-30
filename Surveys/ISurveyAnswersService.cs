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
    public interface ISurveyAnswersService
    {
        SurveyAnswer Get(int id);
        Paged<SurveyAnswer> GetAll(int pageIndex, int pageSize);
        Paged<SurveyAnswer> GetCreatedBy(int pageIndex, int pageSize, int userId);
        int Add(SurveyAnswerAddRequest model);
        void Update(SurveyAnswerUpdateRequest model);
        void Delete(int id);
    }
}
