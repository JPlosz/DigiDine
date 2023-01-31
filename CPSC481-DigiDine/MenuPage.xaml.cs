using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CPSC481_DigiDine
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Window
    {
        //public Database.Item selectedItem;

        private ItemDetail itemDetail = new ItemDetail();
        private Filter filter = new Filter();
        private ReviewPage reviewPage = new ReviewPage();
        private MenuItemList menuItemList = App.menuItemList;

        string startingItem = "JasperBrewingCrispPils";

        public MenuPage()
        {
            // Set this window as the current DataContext for binding
            DataContext = this;

        }

        public void OnLoad( Object sender, RoutedEventArgs e)
        {
            this.KeyDown += new System.Windows.Input.KeyEventHandler(MainWindow_Quit);
            setSplitLeftContext();
            setSplitRightContext("item_detail");
            //selectedItem = App.db.getItem(startingItem);
            updateItemDetail(startingItem);
        }

        public void setSplitLeftContext()
        {
            SplitLeft.Children.Clear();
            DataContext = menuItemList;
            SplitLeft.Children.Add(menuItemList);
        }

        public void setSplitRightContext(string context)
        {
            SplitRight.Children.Clear();
            if (context == "item_detail")
            {
                // change context to item_detail
                DataContext = itemDetail;
                SplitRight.Children.Add(itemDetail);
            } else if (context == "filter")
            {
                // change context to filter
                DataContext = filter;
                SplitRight.Children.Add(filter);
            }
        }

        public void toReviewPage()
        {
            MenuPage menu = (MenuPage)Window.GetWindow(this);
            reviewPage.Owner = menu;
            reviewPage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            reviewPage.refresh();
            reviewPage.ShowDialog();
        }

        public void updateItemDetail(string item_name) 
        {
            // check if current split right-side context is 'item_detail' or 'filter'
            setSplitRightContext("item_detail");
            itemDetail.updateItemDetail(item_name);
        }

        // adding to order, will need to add the current item to the unordered list
        int id = 0;
        public void addItem(Database.Item item, int quantity)
        {
            App.db.unorderedList.Add(id, item);     // add to unordered list
            App.db.itemQuantity.Add(id, quantity);
            id++;
        }

        private void MainWindow_Quit(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
    }
}
