using System;
using C971.Models.DatabaseModels;
using C971.Services;
using C971.Services.Implementations;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace C971
{
  public partial class App : Application
  {
    public App(string dbPath)
    {
      InitializeComponent();
      InitializeDB(dbPath);

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

    private void InitializeDB(string dbPath)
    {
      SQLiteConnection _syncConn = new(dbPath);
      _syncConn.CreateTable<AcademicTerm>();
      _syncConn.CreateTable<Course>();
      _syncConn.CreateTable<Assessment>();
      _syncConn.CreateTable<Instructor>();

      bool Initial = Preferences.Get(nameof(Initial), true);
      if (Initial)
      {
        Initial = false;

        _syncConn.Insert(new AcademicTerm()
        {
          Title = "Mid 2021",
          Start = new DateTime(2021, 4, 01, 6, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2021, 10, 01, 5, 59, 59, DateTimeKind.Utc),
        });

        _syncConn.Insert(new AcademicTerm()
        {
          Title = "Spring 2022",
          Start = new DateTime(2022, 1, 01, 6, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 7, 01, 5, 59, 59, DateTimeKind.Utc),
        });




        Preferences.Set(nameof(Initial), Initial);
      }
    }
  }
}
