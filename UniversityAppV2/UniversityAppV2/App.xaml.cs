﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniversityAppV2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            MainPage.BackgroundColor = Color.FromHex("#4c99fe");
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
