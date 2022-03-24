using C971.Views;
using Xamarin.Forms;

namespace C971
{
  public partial class AppShell : Shell
  {
    public AppShell()
    {
      InitializeComponent();
      Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
      Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
    }
  }
}
