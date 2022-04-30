using Sabio.Models;
using Sabio.Models.Domain.CodeingChallenge;
using Sabio.Models.Requests.CodeingChallenge;
using Sabio.Models.Requests.CodingChallenge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface ICodingChallengeService
    {
        int Add(CourseAddRequest model);
        Course GetById(int id);
        void Update(CourseUpdateRequest model);
        void Delete(int id);
        Paged<Course>CoursesDetails(int pageIndex, int pageSize);
    }
}
