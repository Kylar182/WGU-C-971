using C971.Services;
using Xamarin.Forms;

namespace C971
{
  public partial class App : Application
  {
    public App()
    {
      InitializeComponent();

      DependencyService.Register<MockDataStore>();
      MainPage = new AppShell();
    }

    protected override void OnStart()
    {
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }
  }
}
