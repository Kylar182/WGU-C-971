using System.Collections.Generic;
using System.Threading.Tasks;
using C971.Models.DatabaseModels;

namespace C971.Services
{
  /// <summary>
  /// Base Data Service for CRUD Page Models
  /// </summary>
  /// <typeparam name="T">
  /// CRUD Page Model
  /// </typeparam>
  public interface ICRUDService<T> : IDBService<T> where T : BaseModel
  {
    /// <summary>
    /// Get all Displayable Items of this Type from the Database
    /// </summary>
    Task<List<T>> GetAll();
  }
}
