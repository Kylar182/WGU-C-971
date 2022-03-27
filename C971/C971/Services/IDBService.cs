using System;
using System.Linq.Expressions;
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
    /// Get an Item of this Type using the given expression
    /// </summary>
    /// <param name="predExpr">
    /// Expression for Query
    /// </param>
    Task<T> Get(Expression<Func<T, bool>> predExpr);

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
