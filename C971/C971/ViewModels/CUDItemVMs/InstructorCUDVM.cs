using System;
using System.Linq;
using System.Threading.Tasks;
using C971.Extensions;
using C971.Models.DatabaseModels;
using C971.Services;
using Xamarin.Forms;

namespace C971.ViewModels.ItemCUDVMs
{
  /// <summary>
  /// VM For a Course Instructor Create / Update / Delete
  /// </summary>
  public class InstructorCUDVM : BaseRUDPageVM<Instructor>
  {
    private string name;
    /// <inheritdoc cref="Instructor.Name"/>
    public string Name
    {
      get { return name; }
      set
      {
        SetOrError(new()
        {
          new Tuple<bool, string>(value.NotEmpty(), "A Name is required"),
          new Tuple<bool, string>(value.NotEmpty() && value.Length <= 150, "Name Max 150 Characters")
        }, value.TrimFix());

        SetProperty(ref name, value.TrimFix());
      }
    }

    /// <inheritdoc cref="Instructor.Name"/>
    public string NameError => Errors.ContainsKey(nameof(Name)) ? Errors[nameof(Name)].First() : "";

    private string phoneNumber;
    /// <inheritdoc cref="Instructor.PhoneNumber"/>
    public string PhoneNumber
    {
      get { return phoneNumber; }
      set
      {
        SetOrError(new()
        {
          new Tuple<bool, string>(value.NotEmpty(), "A Phone Number is required"),
          new Tuple<bool, string>(value.NotEmpty() && value.Length <= 150, "Phone Number Max 150 Characters")
        }, value.TrimFix());

        SetProperty(ref phoneNumber, value.TrimFix());
      }
    }

    /// <inheritdoc cref="Instructor.PhoneNumber"/>
    public string PhoneNumberError => Errors.ContainsKey(nameof(PhoneNumber)) ? Errors[nameof(PhoneNumber)].First() : "";

    private string email;
    /// <inheritdoc cref="Instructor.Email"/>
    public string Email
    {
      get { return email; }
      set
      {
        SetOrError(new()
        {
          new Tuple<bool, string>(value.NotEmpty(), "A Phone Number is required"),
          new Tuple<bool, string>(value.NotEmpty() && value.Length <= 150, "Phone Number Max 150 Characters")
        }, value.TrimFix());

        SetProperty(ref email, value.TrimFix());
      }
    }

    /// <inheritdoc cref="Instructor.Email"/>
    public string EmailError => Errors.ContainsKey(nameof(Email)) ? Errors[nameof(Email)].First() : "";

    /// <inheritdoc cref="InstructorCUDVM" />
    public InstructorCUDVM()
    {
      if (Id == null)
      {
        Title = "New Instructor";
        Name = null;
        PhoneNumber = null;
        Email = null;
      }
    }

    /// <inheritdoc cref="TermCUDVM" />
    public InstructorCUDVM(Func<Task> save, Func<Task> delete) : base(save, delete)
    {
      if (Id == null)
      {
        Title = "New Instructor";
        Name = null;
        PhoneNumber = null;
        Email = null;
      }

      Service = DependencyService.Get<IInstructorService>();

      IsBusy = false;
    }

    public async Task LoadInstructor(int id)
    {
      IsBusy = true;
      Id = id;
      Instructor instructor = null;

      await Service.Get(pr => pr.Id == id).ContinueWith(t =>
      {
        if (t.Exception == null)
        {
          Id = t.Result.Id;
          Name = t.Result.Name;
          PhoneNumber = t.Result.PhoneNumber;
          Email = t.Result.Email;
          Title = $"Instructor {id}";
          Item = t.Result;
          instructor = t.Result;
        }
      }).ConfigureAwait(true);

      if (instructor == null)
      {
        Title = "New Instructor";
        Name = null;
        PhoneNumber = null;
        Email = null;
      }

      IsBusy = false;
    }
  }
}
