using C971.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class LoginPage : ContentPage
  {
    public LoginPage()
    {
      InitializeComponent();
      this.BindingContext = new LoginViewModel();
    }
  }
}