using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using C971.Models.DatabaseModels;
using SQLite;
using Xamarin.Forms;

namespace C971.Services.Implementations
{
  /// <inheritdoc cref="IDBService{T}"/>
  public abstract class DBService<T> : IDBService<T> where T : BaseModel, new()
  {
    /// <inheritdoc cref="SQLiteAsyncConnection"/>
    protected readonly SQLiteAsyncConnection _conn;

    /// <inheritdoc cref="IDBService{T}"/>
    public DBService()
    {
      _conn = new SQLiteAsyncConnection(DependencyService.Get<DBConn>().ConnectionString);
    }

    /// <inheritdoc />
    public virtual async Task<T> Add(T item)
    {
      _ = await _conn.InsertAsync(item);
      return item;
    }

    /// <inheritdoc />
    public virtual async Task<T> Get(Expression<Func<T, bool>> predExpr)
    {
      return await _conn.FindAsync(predExpr);
    }

    /// <inheritdoc />
    public virtual async Task<int> Update(T item)
    {
      return await _conn.UpdateAsync(item);
    }

    /// <inheritdoc />
    public virtual async Task<int> Delete(T item)
    {
      return await _conn.DeleteAsync(item);
    }
  }
}
