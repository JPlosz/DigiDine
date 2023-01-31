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
using static CPSC481_DigiDine.Database;

namespace CPSC481_DigiDine
{
    /// <summary>
    /// Interaction logic for ReviewPage.xaml
    /// </summary>
    public partial class ReviewPage : Window
    {

        public ReviewPage()
        {
            DataContext = this;
            InitializeComponent();
            DataContext = this;
        }

        public void refresh()
        {
            populateList(App.db.unorderedList, "unordered");
            updateDisplayedPrices();
        }

        // populates the scroll viewer for unordered and ordered list 
        private void populateList(Dictionary<int, Item> list, string list_type)
        {
            UnorderedListView.Children.Clear();
            // FIX ME: buttons are affected by expander, 
            foreach (var item in list)
            {
                StackPanel wrapper = new StackPanel();
                wrapper.Orientation = Orientation.Horizontal;
                // have expander bind to a seperate stack panel
                //StackPanel buttonWrapper = new StackPanel();
                //buttonWrapper.Orientation = Orientation.Horizontal;

                // - DELETE ITEM BINDING ------------------------------------------------------------------------
                if (list_type == "unordered")
                {
                    Button deleteButton = new Button();
                    deleteButton.Width = 75;
                    deleteButton.Click += new RoutedEventHandler(deleteItem);
                    deleteButton.Content = "X";
                    deleteButton.Tag = item.Key;        // attach item name to delete button

                    wrapper.Children.Add(deleteButton);

                    getSubtotal();
                }

                // - ITEM NAME + SELECTED OPTIONS BINDING -------------------------------------------------------
                Expander itemName = new Expander();
                //itemName.TextWrapping = TextWrapping.Wrap;
                itemName.Width = 387;
                //itemName.TextAlignment = TextAlignment.Left;
                itemName.Padding = new Thickness(20, 10, 15, 10);
                itemName.Header = item.Value.getName();
                // itemName.FlowDirection = "RightToLeft";                   // TO-DO: move expander to the right of the text
                // Database.Option [] arrOfOptions = item.Value.getOptions();  // TO-DO : list all SELECTED options in content
                //string listOfOptions = "";

                Database.Option[] options = item.Value.getOptions();

                string content = "";

                foreach (var option in options)
                {
                    content += option.getName() + "\n";
                    foreach (var choice in option.getChoices())
                    {
                        if (choice.isSelected())
                        {
                            content += "\t" + choice.getName();
                            if (Double.Parse(choice.getPrice()) > 0) content += " $" + choice.getPrice();
                            content += "\n";
                        }
                    }
                }

                itemName.Content = content;
                itemName.FontSize = 16;

                // FIX-ME: iterate through list of options from in App.db.selectedOptions
                //foreach(var option in arrOfOptions)
                //{
                //    //itemName.Content = option.getName();
                //    itemName.Content = option;
                //}
                // itemName.Content = listOfOptions;

                wrapper.Children.Add(itemName);


                // - QUANTITY BINDING -----------------------------------------------------------------------
                TextBlock quantity = new TextBlock();
                quantity.Width = 150;
                quantity.Text = "x " + App.db.getQuantity(item.Key);       // get the quantity of the item based on item id
                quantity.Padding = new Thickness(20, 10, 15, 10);
                wrapper.Children.Add(quantity);


                // - EDIT BUTTON BINDING -------------------------------------------------------------------
                if (list_type == "unordered")
                {
                    Button editButton = new Button();
                    editButton.Width = 75;
                    editButton.Click += new RoutedEventHandler(editItem);
                    editButton.Content = "EDIT";
                    editButton.CommandParameter = item.Value.getIdName();

                    // attach list of options for each item
                    //List<Database.Choice> choices = new List<Database.Choice>();
                    //foreach (var options in item.Value.getOptions())
                    //{
                    //    choices.Add(options.getChoice());
                    //}


                    editButton.Tag = item.Key;
                    //editButton.Style = ;
                    wrapper.Children.Add(editButton);
                    //buttonWrapper.Children.Add(editButton);

                }

                // - PRICE BINDING ------------------------------------------------------------------------
                TextBlock price = new TextBlock();
                price.Width = 149;
                price.Text = "$" + getItemTotalPrice(item.Value, App.db.itemQuantity[item.Key]).ToString("0.00");
                price.TextAlignment = TextAlignment.Left;
                price.Padding = new Thickness(20, 10, 15, 10);
                wrapper.Children.Add(price);

                // ======================================================================================//
                if (list_type == "unordered")
                {
                    UnorderedListView.Children.Add(wrapper);
                    //UnorderedListView.Children.Add(buttonWrapper);
                }
                else if (list_type == "ordered")
                {
                    OrderedListView.Children.Add(wrapper);
                }
            }
        }


        // ================ buttons ===============================================================================//
        private void submitOrder(object sender, RoutedEventArgs e)
        {
            // open thank you window
            ThankYouWindow tyWindow = new ThankYouWindow();
            tyWindow.Owner = this;
            tyWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            tyWindow.Show();

            // Copy whatever was in the unordered list to the ordered list in database
            foreach (var item in App.db.unorderedList)
            {
                App.db.orderedList.Add(item.Key, item.Value);
            }

            // delete everything that was in the unordered list 
            App.db.unorderedList.Clear();
            UnorderedListView.Children.Clear();

            // Add everything to orderedlist section
            OrderedListView.Children.Clear();
            populateList(App.db.orderedList, "ordered");

            // update the subtotal for unordered list 
            updateDisplayedPrices();
        }

        private void returnToMenu(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        // confirmation window
        private void callServer(object sender, RoutedEventArgs e)
        {
            // open thank you window
            CalledServerPopup calledServer = new CalledServerPopup();
            calledServer.Owner = this;
            calledServer.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            calledServer.Show();
        }


        private void howToPay(object sender, RoutedEventArgs e)
        {
            // open thank you window
            howToPay payment = new howToPay();
            payment.Owner = this;
            payment.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            payment.Show();
        }

        // open a window to edit the options of the item selected
        // TO-DO: edit the correct item, need ID of the item and its current options
        private void editItem(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int id = (int)btn.Tag;
            EditItemMenu editItem = new EditItemMenu(this, id);
            editItem.Owner = this;
            editItem.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            editItem.ShowDialog();
        }

        public void closeItemEdit(EditItemMenu editWindow)
        {
            if (editWindow.IsActive) editWindow.Close();
            this.WindowState = WindowState.Normal;
        }

        // remove item from unordered list by id
        private void deleteItem(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int key = Convert.ToInt32(btn.Tag);

            foreach (var item in App.db.unorderedList)
            {
                App.db.unorderedList.Remove(key);
            }

            // update the scroll view
            UnorderedListView.Children.Clear();     // clear it first

            // then re-populate the list
            populateList(App.db.unorderedList, "unordered");

            // update the subtotal for unordered list and whole order totals
            updateDisplayedPrices();
        }

        // =============================================== compute total =============================================== // 
        private static double getOrderedTotal()
        {
            double total = 0;
            // for each item in the dictionary, get its price and add it to the total
            foreach (var item in App.db.orderedList)
            {
                total += getItemTotalPrice(item.Value, App.db.itemQuantity[item.Key]);
            }

            return total;
        }

        private static double getUnorderedTotal()
        {
            double total = 0;

            // for each item in the dictionary, get its price and add it to the total
            foreach (var item in App.db.unorderedList)
            {
                total += getItemTotalPrice(item.Value, App.db.itemQuantity[item.Key]);
            }

            return total;
        }

        public static double getSubtotal()
        {
            return getUnorderedTotal() + getOrderedTotal();
        }

        private static Double getItemTotalPrice(Database.Item item, int quantity)
        {
            double total = Convert.ToDouble(item.getPrice());

            foreach (var option in item.getOptions())
            {
                foreach (var choice in option.getChoices())
                {
                    if (choice.isSelected())
                    {
                        total += Double.Parse(choice.getPrice());
                    }
                }
            }

            return total * quantity;
        }

        // =================================== binding functions - will be called in XAML file =================================== //


        // current subtotal of items in the cart 
        public string getTax
        {
            get { return "$ " + (getSubtotal() * 0.05).ToString("0.00"); }
        }

        public string unorderedTotal
        {
            get { return "$ " + getUnorderedTotal().ToString("0.00"); }
        }

        public string orderedTotal
        {
            get { return "$ " + getOrderedTotal().ToString("0.00"); }
        }

        public string total
        {
            get { return "$ " + (getSubtotal() * 1.05).ToString("0.00"); }

        }

        public string subtotal
        {
            get { return "$ " + getSubtotal().ToString("0.00"); }
        }

        private void updateDisplayedPrices()
        {
            unordered_subtotal_amount.Content = unorderedTotal;
            ordered_subtotal_amount.Content = orderedTotal;
            subtotalLabel.Content = subtotal;
            taxLabel.Content = getTax;
            totalLabel.Content = total;
        }

    }
}
