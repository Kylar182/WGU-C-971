using C971.Models;
using C971.ViewModels;
using Xamarin.Forms;

namespace C971.Views
{
  public partial class NewItemPage : ContentPage
  {
    public Item Item { get; set; }

    public NewItemPage()
    {
      InitializeComponent();
      BindingContext = new NewItemViewModel();
    }
  }
}