﻿using C971.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace C971.Views
{
  public partial class ItemDetailPage : ContentPage
  {
    public ItemDetailPage()
    {
      InitializeComponent();
      BindingContext = new ItemDetailViewModel();
    }
  }
}