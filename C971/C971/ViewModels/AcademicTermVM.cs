using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using C971.Models.DatabaseModels;
using C971.Services;
using Xamarin.Forms;

namespace C971.ViewModels
{
  public class AcademicTermVM : BaseViewModel
  {
    private readonly IAcademicTermService Service;

    public ObservableCollection<AcademicTerm> Terms { get; }

    public Command LoadTermsCommand { get; }

    private AcademicTerm _selectedTerm;
    public AcademicTerm SelectedTerm
    {
      get => _selectedTerm;
      set
      {
        SetProperty(ref _selectedTerm, value);
        OnTermSelected(value);
      }
    }

    public AcademicTermVM()
    {
      Title = nameof(Terms);
      Service = DependencyService.Get<IAcademicTermService>();
      Terms = new ObservableCollection<AcademicTerm>();
      LoadTermsCommand = new Command(async () => await ExecuteLoadTerms());
    }

    public void OnAppearing()
    {
      IsBusy = true;
      SelectedTerm = null;
    }

    private async Task ExecuteLoadTerms()
    {
      IsBusy = true;

      try
      {
        Terms.Clear();

        List<AcademicTerm> terms = await Service.GetAll();

        foreach (AcademicTerm term in terms)
          Terms.Add(term);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);
      }
      finally
      {
        IsBusy = false;
      }
    }


    async void OnTermSelected(AcademicTerm term)
    {
      if (term == null)
        return;

      // This will push the ItemDetailPage onto the navigation stack
      await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={term.Id}");
    }
  }
}
