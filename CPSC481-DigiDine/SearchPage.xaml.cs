using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CPSC481_DigiDine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SearchPage : Window
    {
        public SearchPage()
        {
            // Set this window as the current DataContext for binding
            DataContext = this;
        }

        // current subtotal of items in the cart
        private string _subtotal = "0.00";
        public string Subtotal
        {
            get { return "$" + _subtotal; }
            set { _subtotal = value; }
        }

        private void LeftViewButton_Click(object sender, RoutedEventArgs e)
        {
            Specials_Category.ScrollToRightEnd();
        }
    }
}
