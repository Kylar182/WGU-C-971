using System.Collections.Generic;
using System.Threading.Tasks;
using C971.Models.DatabaseModels;

namespace C971.Services
{
  /// <summary>
  /// Academic Course Database Service
  /// </summary>
  public interface ICourseService : ICRUDService<Course>
  {
    /// <summary>
    /// Get a List of Courses by their TermId
    /// </summary>
    /// <param name="termId">
    /// Term Id to use for Query
    /// </param>
    Task<List<Course>> GetByTerm(int termId);
  }
}
