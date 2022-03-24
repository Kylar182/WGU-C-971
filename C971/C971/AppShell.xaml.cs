using C971.ViewModels;
using C971.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace C971
{
  public partial class AppShell : Xamarin.Forms.Shell
  {
    public AppShell()
    {
      InitializeComponent();
      Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
      Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
    }

  }
}
