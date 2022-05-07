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

      if (item is INotify notify)
      {
        Notification startNotify = notify.StartNotification();
        _ = await _conn.InsertAsync(startNotify);
        notify.StartId = startNotify.Id;

        Notification endNotify = notify.EndNotification();
        _ = await _conn.InsertAsync(endNotify);
        notify.EndId = endNotify.Id;

        _ = await _conn.UpdateAsync(notify);

        startNotify.StartInsert();
        startNotify.EndInsert();
      }

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
      if (item is INotify notify)
      {
        Notification startNotify = notify.StartNotification();
        _ = await _conn.UpdateAsync(startNotify);

        startNotify.StartUpdate();

        Notification endNotify = notify.EndNotification();
        _ = await _conn.UpdateAsync(endNotify);

        endNotify.EndUpdate();

        return await _conn.UpdateAsync(notify);
      }

      return await _conn.UpdateAsync(item);
    }

    /// <inheritdoc />
    public virtual async Task<int> Delete(T item)
    {
      if (item is INotify notify)
      {
        Notification startNotify = notify.StartNotification();
        startNotify.Cancel();
        _ = await _conn.DeleteAsync(startNotify);

        Notification endNotify = notify.EndNotification();
        endNotify.Cancel();
        _ = await _conn.DeleteAsync(endNotify);
      }

      return await _conn.DeleteAsync(item);
    }
  }
}
