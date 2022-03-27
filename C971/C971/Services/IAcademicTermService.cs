using System.Collections.Generic;
using System.Threading.Tasks;
using C971.Models.DatabaseModels;

namespace C971.Services
{
  /// <summary>
  /// Academic Term Database Service
  /// </summary>
  public interface IAcademicTermService : IDBService<AcademicTerm>
  {
    Task<List<AcademicTerm>> GetAll();
  }
}
