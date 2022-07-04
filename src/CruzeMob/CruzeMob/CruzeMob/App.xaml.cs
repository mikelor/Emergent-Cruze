using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CruzeMob.Data;
using System.Dynamic;

namespace CruzeMob
{
    public partial class App : Application
    {
        public static CruzeServices CruzeApi { get; private set; }
        public App()
        {
            InitializeComponent();
            CruzeApi = new CruzeServices(new RestService());
            MainPage = new MainPage();
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
