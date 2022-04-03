﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
  public abstract class BaseRUDPageVM<T> : BaseViewModel where T : BaseModel
  {
    /// <summary>
    /// DB CRUD Service for this Database Model
    /// </summary>
    protected ICRUDService<T> Service;

    /// <summary>
    /// Database Item to Add or Update
    /// </summary>
    public T Item { get; set; }

    /// <summary>
    /// Command to attempt to Save the State of the Current Model if Valid
    /// </summary>
    public Command Save { get; }

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
    protected void SetOrError<P>(ref P backingStore, List<Tuple<bool, string>> validations, P value,
                                          [CallerMemberName] string propertyName = "", Action onChanged = null)
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

        SetProperty(ref backingStore, value, propertyName, onChanged);
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

        if (!errors.Where(e => e == error).Any())
          errors.Add(error);

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

        if (errors.Where(e => e == error).Any())
          errors.Remove(errors.Where(e => e == error).First());

        if (errors.Any())
          Errors[propertyName] = errors;
        else
        {
          Errors.Remove(propertyName);
        }
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
    protected virtual Task SaveItem()
    {
      return Task.CompletedTask;
    }
  }
}
