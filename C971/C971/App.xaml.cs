using C971.Models.DatabaseModels;
using C971.Services;
using C971.Services.Implementations;
using Xamarin.Forms;

namespace C971
{
  public partial class App : Application
  {
    public App(string dbPath)
    {
      InitializeComponent();

      DBConn conn = new()
      {
        ConnectionString = dbPath
      };

      DependencyService.Register<MockDataStore>();
      DependencyService.RegisterSingleton(conn);
      DependencyService.Register<IAcademicTermService, AcademicTermService>();
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
