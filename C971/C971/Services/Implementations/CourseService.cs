using System.Collections.Generic;
using System.Threading.Tasks;
using C971.Models.DatabaseModels;

namespace C971.Services.Implementations
{
  /// <inheritdoc cref="ICourseService"/>
  public class CourseService : CRUDDBService<Course>, ICourseService
  {
    public async Task<List<Course>> GetByTerm(int termId)
    {
      return await _conn.Table<Course>().Where(pr => pr.AcademicTermId == termId).ToListAsync();
    }
  }
}
