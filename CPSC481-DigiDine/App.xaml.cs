using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CPSC481_DigiDine
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Database db = Database.Instance;
        public static MenuItemList menuItemList = new MenuItemList();
        private void App_Startup(object sender, StartupEventArgs e)
        {
            MenuPage window = new MenuPage();
            window.InitializeComponent();
            window.Show();
        }


        public void NewFilter(object sender, RoutedEventArgs e)
        {

        }
    }


}
