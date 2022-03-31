using System.Collections.Generic;
using System.Threading.Tasks;
using C971.Models.DatabaseModels;

namespace C971.Services.Implementations
{
  /// <inheritdoc cref="ICRUDService{T}"/>
  public class CRUDDBService<T> : DBService<T>, ICRUDService<T> where T : BaseModel, new()
  {
    /// <inheritdoc />
    public async Task<List<T>> GetAll()
    {
      return await _conn.Table<T>().ToListAsync();
    }
  }
}
