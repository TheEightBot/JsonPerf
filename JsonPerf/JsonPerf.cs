﻿using System;
using JsonPerf.Pages;
using Xamarin.Forms;

namespace JsonPerf
{
    public class App : Application
    {
        public App()
        {
            MainPage = new LocationTabContainer();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
