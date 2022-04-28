using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using C971.Extensions;
using C971.Models.DatabaseModels;
using C971.Services;
using Xamarin.Forms;

namespace C971.ViewModels
{
  /// <summary>
  /// Base View Model for DB Model Add / Update / Delete Pages
  /// </summary>
  /// <typeparam name="T">
  /// Database Model for CRUD
  /// </typeparam>
  public abstract class BaseRUDPageVM<T> : BaseViewModel where T : BaseModel, new()
  {
    protected new bool isBusy = true;
    /// <summary>
    /// DB CRUD Service for this Database Model
    /// </summary>
    protected ICRUDService<T> Service;

    /// <summary>
    /// Database Item to Add or Update
    /// </summary>
    public T Item { get; set; }

    private int? id;
    /// <inheritdoc cref="BaseModel.Id"/>
    public int? Id
    {
      get { return id; }
      set
      {
        SetOrError(new() { new Tuple<bool, string>(true, $"{typeof(T).Name} Id") }, value);

        SetProperty(ref id, value);
        CanDelete = Id != null && Id > 0;
      }
    }

    /// <summary>
    /// Command to attempt to Save the State of the Current Model if Valid
    /// </summary>
    public ICommand Save { get; protected set; }

    /// <summary>
    /// Command to attempt to Delete the Current Model
    /// </summary>
    public ICommand Delete { get; protected set; }

    /// <summary>
    /// Errors of each Property
    /// </summary>
    /// <remarks>
    /// Property's Name is the Key, Value is a List of the Errors that property may have, if any
    /// </remarks>
    public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();

    /// <summary>
    /// Determines if the Bound Model Properties are Valid
    /// </summary>
    public bool Valid => Errors.Count == 0;

    private bool canDelete;
    /// <summary>
    /// Determines if the Bound Model Properties are Valid
    /// </summary>
    public bool CanDelete
    {
      get { return canDelete; }
      set
      {
        SetProperty(ref canDelete, value);
      }
    }

    public BaseRUDPageVM()
    {
      Item = new();
    }

    /// <inheritdoc cref="BaseAddPageVM{}" />
    public BaseRUDPageVM(Func<Task> save, Func<Task> delete)
    {
      Item = new();
      Save = new Command(async () => await save?.Invoke());
      Delete = new Command(async () => await delete?.Invoke());
    }

    /// <summary>
    /// Initial Page Load Visibility Method
    /// </summary>
    public void OnAppearing()
    {
      IsBusy = true;
    }

    /// <summary>
    /// Updates the Item Property and Errors related to the Value based on Validity
    /// </summary>
    /// <typeparam name="P">
    /// Property Type Calling the Set Method
    /// </typeparam>
    /// <param name="validations">
    /// Key Value Pair Validations of the Value 
    /// </param>
    /// <param name="value">
    /// Property's Value
    /// </param>
    /// <param name="propertyName">
    /// Name of the Property or Caller if not Explicit
    /// </param>
    /// <remarks>
    ///  Validations => Key Boolean false = invalid, string Error = Error Message if Invalid
    /// </remarks>
    protected void SetOrError<P>(List<Tuple<bool, string>> validations, P value,
                                          [CallerMemberName] string propertyName = "")
    {
      if (propertyName.NotEmpty())
      {
        foreach (Tuple<bool, string> validation in validations)
        {
          if (validation.Item1)
            RemoveError(propertyName, validation.Item2);
          else
            AddError(propertyName, validation.Item2);
        }

        if (PropValid(propertyName))
          Item.SetByName(propertyName, value);

        OnPropertyChanged($"{propertyName}Error");

        OnPropertyChanged(nameof(Valid));
      }
    }

    /// <summary>
    /// Adds an Error to the Errors Dictionary using the given Property Name as a Key
    /// </summary>
    /// <param name="propertyName">
    /// Name of the Propery Containing the Error
    /// </param>
    /// <param name="error">
    /// Error's Value
    /// </param>
    protected void AddError(string propertyName, string error)
    {
      if (propertyName.NotEmpty())
      {
        Errors.TryGetValue(propertyName, out List<string> errors);

        if (errors != null)
        {
          if (!errors.Where(e => e == error).Any())
            errors.Add(error);
        }
        else
        {
          errors = new()
          {
            error
          };
        }

        Errors[propertyName] = errors;
      }
    }

    /// <summary>
    /// Removes an Error from the Errors Dictionary using the given Property Name as a Key <para />
    /// If the Property no longer contains any Errors, it removes the Property from the Error Dictionary and sets the Value
    /// </summary>
    /// <param name="propertyName">
    /// Name of the Propery Containing the Error
    /// </param>
    /// <param name="error">
    /// Error's Value
    /// </param>
    protected void RemoveError(string propertyName, string error)
    {
      if (propertyName.NotEmpty())
      {
        Errors.TryGetValue(propertyName, out List<string> errors);

        if (errors != null && errors.Where(e => e == error).Any())
          errors.Remove(errors.Where(e => e == error).First());

        if (errors != null && errors.Any())
          Errors[propertyName] = errors;
        else
          Errors.Remove(propertyName);
      }
    }

    /// <summary>
    /// Checks if a Property is Valid
    /// </summary>
    /// <param name="prop">
    /// Property's Name
    /// </param>
    protected bool PropValid(string prop)
    {
      return !Errors.ContainsKey(prop);
    }

    /// <summary>
    /// Trys to Save the Item to the Database if it's Valid
    /// </summary>
    public virtual async Task SaveItem()
    {
      if (Valid)
      {
        if (Item.Id == 0)
        {
          await Service.Add(Item);
          Id = Item.Id;
        }
        else
          await Service.Update(Item);
      }
      else
      {
        return;
      }
    }

    /// <summary>
    /// Trys to Delete the Item from the Database if possible
    /// </summary>
    public virtual async Task DeleteItem()
    {
      if (CanDelete)
        await Service.Delete(Item);
      else
        return;
    }
  }
}
