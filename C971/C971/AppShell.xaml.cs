using C971.Views.NewItemPages;
using Xamarin.Forms;

namespace C971
{
  public partial class AppShell : Shell
  {
    public AppShell()
    {
      InitializeComponent();
      Routing.RegisterRoute(nameof(NewTermsPage), typeof(NewTermsPage));
      //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
    }
  }
}
