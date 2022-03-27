using System.Collections.Generic;
using System.Threading.Tasks;
using C971.Models.DatabaseModels;

namespace C971.Services.Implementations
{
  /// <inheritdoc cref="IAcademicTermService"/>
  public class AcademicTermService : DBService<AcademicTerm>, IAcademicTermService
  {
    public async Task<List<AcademicTerm>> GetAll()
    {
      return await _conn.Table<AcademicTerm>().ToListAsync();
    }
  }
}
