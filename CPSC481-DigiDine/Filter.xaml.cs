using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Filter.xaml
    /// </summary>
    public partial class Filter : UserControl
    {
        private List<string> customList = new List<string>();
        private bool PriceRangeIsChecked;
        Database.Item currentItem;
        String Input;

        public Filter()
        {
            InitializeComponent();
            PriceRangeIsChecked= false;
        }

        private void SearchBar(Object sender, RoutedEventArgs e)
        {
            App.menuItemList.addFilterTag(textBox);
        }

        private void PriceFilter(object sender)
        {
            if (PriceRangeIsChecked)
            {
                App.menuItemList.addFilterTag(slider);
            }
            else
            {
                App.menuItemList.removeFilterTag(slider);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).Name == "PriceRange")
            {
                PriceRangeIsChecked = false;
                PriceFilter(slider);
            }
            else
            {
                App.menuItemList.removeFilterTag(sender);
            }

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).Name == "PriceRange")
            {
                PriceRangeIsChecked= true;
                PriceFilter(slider);
            }
            else
            {
                App.menuItemList.addFilterTag(sender);
            }
            
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           PriceFilter(slider);
            /*var slider = sender as Slider;
            double value = slider.Value;

            String name = "";
            currentItem = App.db.getItem(name);
            String currentItemPrice = currentItem.getPrice();
            String currentItemName = currentItem.getName();
            if (PriceRangeIsChecked == true)
            {
                if (Convert.ToDouble(currentItemPrice) <= value)
                {
                    customList.Add(currentItemName);
                }
            }*/

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        /*
       private void Button_Click(object sender, RoutedEventArgs e)
       {
           Input = textBox.Text;

           String name = Input;
           currentItem = App.db.getItem(name);
           String currentItemDescription = currentItem.getDescription();
           String[] currentItemIngredients = currentItem.getIngredients();
           String currentItemName = currentItem.getName();

           if (currentItemName.Contains("Input"))
           {
               customList.Add(currentItemName);
           }
           else if (currentItemDescription.Contains("Input"))
           {
               customList.Add(currentItemName);
           }
           else if (currentItemIngredients.Contains("Input"))
           {
               customList.Add(currentItemName);
           }
       }*/

        /*
if ((Peanuts.IsChecked == true)) 
{
    String name = "";
    currentItem = App.db.getItem(name);
    String[] currentItemIngredients = currentItem.getIngredients();
    String currentItemName = currentItem.getName();
    if (!currentItemIngredients.Contains("peanuts"))
    {
        customList.Add(currentItemName);
    }
};
if ((Vegan.IsChecked == true))
{
    String name = "";
    currentItem = App.db.getItem(name);
    String currentItemDescription = currentItem.getDescription();
    String currentItemName = currentItem.getName();
    if (currentItemDescription.Contains("vegan"))
    {
        customList.Add(currentItemName);
    }
};
if ((Chicken.IsChecked == true))
{
    String name = "";
    currentItem = App.db.getItem(name);
    String[] currentItemIngredients = currentItem.getIngredients();
    String currentItemName = currentItem.getName();
    if (!currentItemIngredients.Contains("chicken"))
    {
        customList.Add(currentItemName);
    }
};
if ((Pork.IsChecked == true))
{
    String name = "";
    currentItem = App.db.getItem(name);
    String[] currentItemIngredients = currentItem.getIngredients();
    String currentItemName = currentItem.getName();
    if (!currentItemIngredients.Contains("pork"))
    {
        customList.Add(currentItemName);
    }
};
if ((Beef.IsChecked == true))
{
    String name = "";
    currentItem = App.db.getItem(name);
    String[] currentItemIngredients = currentItem.getIngredients();
    String currentItemName = currentItem.getName();
    if (!currentItemIngredients.Contains("beef"))
    {
        customList.Add(currentItemName);
    }
};
if ((Vegetarian.IsChecked == true))
{
    String name = "";
    currentItem = App.db.getItem(name);
    String currentItemDescription = currentItem.getDescription();
    String currentItemName = currentItem.getName();
    if (currentItemDescription.Contains("vegetarian"))
    {
        customList.Add(currentItemName);
    }
};
if ((Gluten.IsChecked == true))
{
    String name = "";
    currentItem = App.db.getItem(name);
    String currentItemDescription = currentItem.getDescription();
    String currentItemName = currentItem.getName();
    if (currentItemDescription.Contains("gluten"))
    {
        customList.Add(currentItemName);
    }
};
if ((Dairy.IsChecked == true))
{
    String name = "";
    currentItem = App.db.getItem(name);
    String currentItemDescription = currentItem.getDescription();
    String currentItemName = currentItem.getName();
    if (currentItemDescription.Contains("dairy"))
    {
        customList.Add(currentItemName);
    }
};
if ((PriceRange.IsChecked == true))
{
    PriceRangeIsChecked = true;
};
*/
    }
}
