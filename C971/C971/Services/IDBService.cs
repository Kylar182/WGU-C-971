using System.Threading.Tasks;
using C971.Models.DatabaseModels;

namespace C971.Services
{
  /// <summary>
  /// Root Data Service for all Data Services
  /// </summary>
  public interface IDBService<T> where T : BaseModel
  {
    /// <summary>
    /// Add an Item to it's Table in the Database
    /// </summary>
    Task<T> Add(T item);

    /// <summary>
    /// Update this Item in the Database
    /// </summary>
    Task<int> Update(T item);

    /// <summary>
    /// Delete this Item in the Database
    /// </summary>
    Task<int> Delete(T item);
  }
}
