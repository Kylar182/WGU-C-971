using C971.Models.DatabaseModels;

namespace C971.Services.Implementations
{
  /// <inheritdoc cref="IAcademicTermService"/>
  public class AcademicTermService : DBService<AcademicTerm>, IAcademicTermService
  {
    /// <inheritdoc cref="IAcademicTermService"/>
    public AcademicTermService(DBConn dbPath) : base(dbPath) { }
  }
}
