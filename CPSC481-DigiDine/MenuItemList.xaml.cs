using System;
using System.Collections;
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
using System.Xml.Linq;
using static CPSC481_DigiDine.Database;

namespace CPSC481_DigiDine
{
    /// <summary>
    /// Interaction logic for MenuItemList.xaml
    /// </summary>
    public partial class MenuItemList : UserControl
    {
        // Variables for storing stuff for ease of access
        Database.Item selectedItem;
        Database.Option currentOption;
        Database.Choice currentChoice;

        // Memory for which buttons should be highlighted on Left side
        Button currentCategory;
        Button currentItem;

        // Storing search values
        // This is the searchbar 
        String searchKey;
        // This stores the price filter
        public String priceFilter;
        // This stores any checked restrictions
        ArrayList restrictionList = new ArrayList();
        // This is the compiled list of items for the custom search
        ArrayList customDb = new ArrayList();
        Boolean Vegan = false;
        Boolean Vegetarian = false;

        StringComparison comp = StringComparison.OrdinalIgnoreCase;


        public MenuItemList()
        {
            searchKey = "";
            priceFilter = null;
            restrictionList = new ArrayList();
            customDb = new ArrayList();

            InitializeComponent();
            initialHighlight(Appetizers, new RoutedEventArgs());
        }

        // ==================================
        // **** Menu List Initialization ****
        // ==================================

        /// <summary>
        /// Initializes the first list and sets the initial highlighted values. Runs as soon as the app has loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void initialHighlight(object sender, RoutedEventArgs e)
        {
            currentCategory = Appetizers;
            SwitchCategory(Appetizers, e);
            CurrentCategory.Content = currentCategory.Name;
        }

        // ======================================
        // END **** Menu List Initialization ****
        // ======================================


        // ============================
        // **** Category Scrolling ****
        // ============================

        /// <summary>
        /// Moves the category tabs to view more. When reaching the end, the arrow on that side disappears      *******************************This needs to be readjusted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollRight(object sender, RoutedEventArgs e)
        {
            CategoryPanel.ScrollToRightEnd();
            if (CategoryPanel.HorizontalOffset == CategoryPanel.MaxWidth)
            {
                LeftViewButton.Visibility = Visibility.Hidden;
                RightViewButton.Visibility = Visibility.Visible;
            } 

            else 
            { 
                LeftViewButton.Visibility = Visibility.Visible; 
                RightViewButton.Visibility = Visibility.Hidden;
            }

            CategoryPanel.Margin = new Thickness(24, 35, 0, 651);
        }

        /// <summary>
        /// Moves the category tabs to view more. When reaching the end, the arrow on that side disappears      *******************************This needs to be readjusted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollLeft(object sender, RoutedEventArgs e)
        {
            CategoryPanel.ScrollToLeftEnd();
            if (CategoryPanel.HorizontalOffset == CategoryPanel.MaxWidth)
            {
                LeftViewButton.Visibility = Visibility.Visible;
                RightViewButton.Visibility = Visibility.Hidden;
            }

            else
            {
                LeftViewButton.Visibility = Visibility.Hidden;
                RightViewButton.Visibility = Visibility.Visible;
            }

            CategoryPanel.Margin = new Thickness(0, 35, 24, 651);
        }

        private void updateScrollButtons(object sender, ScrollChangedEventArgs e)
        {
            if (CategoryPanel.HorizontalOffset == CategoryPanel.ScrollableWidth)
            {
                LeftViewButton.Visibility = Visibility.Visible;
                RightViewButton.Visibility = Visibility.Hidden;
                CategoryPanel.Margin = new Thickness(24, 35, 0, 651);
            }
            else if (CategoryPanel.HorizontalOffset != CategoryPanel.MaxWidth && CategoryPanel.HorizontalOffset != 0)
            {
                LeftViewButton.Visibility = Visibility.Visible;
                RightViewButton.Visibility = Visibility.Visible;
                CategoryPanel.Margin = new Thickness(24, 35, 24, 651);
            }
            else
            {
                LeftViewButton.Visibility = Visibility.Hidden;
                RightViewButton.Visibility = Visibility.Visible;
                CategoryPanel.Margin = new Thickness(0, 35, 24, 651);
            }

            if (Custom.IsFocused)
            {
                CategoryPanel.ScrollToRightEnd();
                RightViewButton.Visibility = Visibility.Hidden;
                CategoryPanel.Margin = new Thickness(24, 35, 0, 651);
            }
        }

        // ================================
        // END **** Category Scrolling ****
        // ================================


        // ========================
        // **** Button Presses ****
        // ========================

        /// <summary>
        /// When an item button is pressed, passes the information and updates the right side of the screen.
        /// Also picks which button should be highlighted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateItemDetail(object sender, RoutedEventArgs e)
        {
            selectedItem = App.db.getItem(((Button)sender).Name);
            // Changes the buttons that should be highlighted
            if (currentItem != null)
            {
                currentItem.Style = this.FindResource("DefaultButton") as Style;
            }
            currentItem = (Button)sender;
            currentItem.Style = this.FindResource("SelectedButton") as Style;

            MenuPage menu = (MenuPage)Window.GetWindow(this);
            menu.updateItemDetail((sender as Button).Name);
        }

        /// <summary>
        /// Handles the event in which a category button is pressed.
        /// Repopulates the list of items being displayed
        /// Also changes which category button is highlighted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchCategory(object sender, RoutedEventArgs e)
        {
            // De-populates the list
            while (menuPanel.Children.Count > 0)
            {
                menuPanel.Children.RemoveAt(0);
            }

            // Changes which category button is highlighted
            currentCategory.Style = this.FindResource("DefaultButton") as Style;
            currentCategory = (Button)sender;
            currentCategory.Style = this.FindResource("SelectedButton") as Style;
            currentCategory.Focus();
            CurrentCategory.Content = currentCategory.Name;

            // Creates and populates the list depending on which category was selected
            String name = ((Button)sender).Name;
            if (name == "Beverages") foreach (KeyValuePair<String, Database.Item> entry in App.db.beverageList)
                {
                    createItemButton(entry.Value);
                }
            else if (name == "Appetizers") foreach (KeyValuePair<String, Database.Item> entry in App.db.appetizerList)
                {
                    createItemButton(entry.Value);
                }
            else if (name == "Mains") foreach (KeyValuePair<String, Database.Item> entry in App.db.mainsList)
                {
                    createItemButton(entry.Value);
                }
            else if (name == "Specials") foreach (KeyValuePair<String, Database.Item> entry in App.db.specialsList)
                {
                    createItemButton(entry.Value);
                }
            else if (name == "Complimentary") foreach (KeyValuePair<String, Database.Item> entry in App.db.complimentaryList)
                {
                    createItemButton(entry.Value);
                }
            else if (name == "Desserts") foreach (KeyValuePair<String, Database.Item> entry in App.db.dessertList)
                {
                    createItemButton(entry.Value);
                }
            else if (name == "Custom" && menuPanel.Children.Count == 0)
                {
                    createCustomMenu();
                }
            else if (name == "Specials") foreach (KeyValuePair<String, Database.Item> entry in App.db.specialsList)
                {
                    createItemButton(entry.Value);
                }

            if (menuPanel.Children.Count == 0) 
            {
                createEmptyMessage();
                currentCategory.Focus();
                CategoryPanel.ScrollToRightEnd();
            }
        }

        private void createEmptyMessage()
        {
            Label empty = new Label();
            empty.Name = "Empty";
            empty.Content = "NO AVAILABLE RESULTS";
           // empty.Background = (Brush)Application.Current.MainWindow.FindResource("altTextColor");
            empty.Foreground = (Brush)Application.Current.MainWindow.FindResource("textColor");
            empty.Height = 644;
            empty.Width = 477.5;
            empty.HorizontalContentAlignment= HorizontalAlignment.Center;
            empty.VerticalContentAlignment= VerticalAlignment.Center;
            empty.FontSize= 20;
            empty.FontWeight = FontWeights.UltraBold;
            menuPanel.Children.Add(empty);
        }
        // ============================
        // END **** Button Presses ****
        // ============================


        // ===================================
        // **** Search menu list handling ****
        // ===================================

        /// <summary>
        /// Handles the event in which a filter in the form of a typed word, price slider, or checkbox is applied
        /// Takes and stores the filter value and applies it to the custom list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void addFilterTag(object sender)
        {
            if (sender is TextBox)
            {
                searchKey = ((TextBox)sender).Text;
                if (searchKey.Contains("Vegan", comp))
                {
                    Vegan = true;
                }
                else if (!restrictionList.Contains("Vegan"))
                {
                    Vegan = false;
                }
                if (searchKey.Contains("Vegetarian", comp))
                {
                    Vegetarian = true;
                }
                else if (!restrictionList.Contains("Vegetarian"))
                {
                    Vegetarian = false;
                }
            }

            else if (sender is Slider)
            {
                priceFilter = ((Slider)sender).Value.ToString();
            }

            else
            {
                if (((CheckBox)sender).Name.Contains("Vegan", comp))
                {
                    Vegan = true;
                }
                else if (((CheckBox)sender).Name.Contains("Vegetarian", comp))
                {
                    Vegetarian = true;
                }

                restrictionList.Add(((CheckBox)sender).Name);
            }
            filterItems();
        }

        /// <summary>
        /// Handles the event in which a filter tag is removed via a checkbox or slider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void removeFilterTag(object sender)
        {
            if (sender is CheckBox)
            {
                if (((CheckBox)sender).Name == "Vegan" && !searchKey.Contains("Vegan", comp))
                {
                    Vegan = false;
                }
                else if (((CheckBox)sender).Name == "Vegetarian" && !searchKey.Contains("Vegetarian", comp))
                {
                    Vegetarian = false;
                }
                restrictionList.Remove(((CheckBox)sender).Name);
            }
            else
            {
                priceFilter = null;
            }
            filterItems();

        }

        /// <summary>
        /// Filters items from the database into an arraylist according to the filters applied
        /// </summary>
        public void filterItems()
        {
            ArrayList customList = new ArrayList();

            // No filter
            if (searchKey.Count() == 0 && restrictionList.Count == 0)
            {
                customDb = customList;
            }

            // Searchbar filter only. Checks if the item contains it then adds it to the list.
            else if (searchKey.Count() > 0 && restrictionList.Count == 0)
            {
                foreach (KeyValuePair<String, Database.Item> entry in App.db.getDataSet())
                {
                    Database.Item item = entry.Value;
                    if (Vegan)
                    {
                        if (item.isItemVegan())
                        {
                            customList.Add(item);
                            continue;
                        }
                    }
                    if (Vegetarian)
                    {
                        if (item.isItemVegetarian())
                        {
                            customList.Add(item);
                            continue;
                        }
                    }
                    if (item.getName().Contains(searchKey, comp) || item.getDescription().Contains(searchKey, comp))
                    {
                        customList.Add(item);
                    }
                    else foreach (String ingredient in item.getIngredients())
                    {
                        if (ingredient.Contains(searchKey, comp))
                        {
                            customList.Add(item);
                            break;
                        }
                    }
                }
            }

            // Checkbox filters only, checks that the restriction isn't in the item then adds it to the list
            else if (searchKey.Count() == 0 && restrictionList.Count > 0)
            {
                Boolean badItem = false;

                foreach (KeyValuePair<String, Database.Item> entry in App.db.getDataSet())
                {
                    Database.Item item = entry.Value;
                    if (Vegan)
                    {
                        if (!item.isItemVegan())
                        {
                            continue;
                        }
                    }
                    if (Vegetarian)
                    {
                        if (!item.isItemVegetarian())
                        {
                            continue;
                        }
                    }

                    foreach (String filter in restrictionList)
                    {
                        if (item.getName().Contains(filter, comp) && item.getDescription().Contains(filter, comp))
                        {
                            badItem = true;
                            break;
                        }
                        else
                        {
                            foreach (String ingredient in item.getIngredients())
                            {
                                if (ingredient.Contains(filter, comp))
                                {
                                    badItem = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (!badItem)
                    {
                        customList.Add(item);
                    }
                    else badItem = false;
                }
            }

            // Checks for restrictions first, then checks if that item also contains the searchword
            else
            {
                Boolean badItem = false;
                foreach (KeyValuePair<String, Database.Item> entry in App.db.getDataSet())
                {
                    Database.Item item = entry.Value;
                    if (Vegan)
                    {
                        if (!item.isItemVegan())
                        {
                            continue;
                        }
                    }
                    if (Vegetarian)
                    {
                        if (!item.isItemVegetarian())
                        {
                            continue;
                        }
                    }

                    foreach (String filter in restrictionList)
                    {
                        if (item.getName().Contains(filter, comp) && item.getDescription().Contains(filter, comp))
                        {
                            badItem = true;
                            break;
                        }
                        else
                        {
                            foreach (String ingredient in item.getIngredients())
                            {
                                if (ingredient.Contains(filter, comp))
                                {
                                    badItem = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (badItem)
                    {
                        badItem= false;
                        continue;
                    }

                    if (item.getName().Contains(searchKey, comp) || item.getDescription().Contains(searchKey, comp))
                    {
                        customList.Add(item);
                    }
                    else foreach (String ingredient in item.getIngredients())
                    {
                        if (ingredient.Contains(searchKey, comp))
                        {
                            customList.Add(item);
                            break;
                        }
                    }
                }
            }

            
            // Checks if a there is a price filter applied
            if (priceFilter != null)
            {
                // Checks if any other filters were applied or if no items are already in the list
                if (customList.Count == 0)
                {
                    // If no items already in the list, scans through the database
                    foreach (KeyValuePair<String, Database.Item> entry in App.db.getDataSet())
                    {
                        Database.Item item = entry.Value;
                        if (double.Parse(item.getPrice()) <= double.Parse(priceFilter))
                        {
                            customList.Add(item);
                        }
                    }
                }
                // Otherwise filter the items already in the list
                else
                {
                    for (int i = 0; i < customList.Count; i++)
                    {
                        if (double.Parse(((Database.Item)customList[i]).getPrice()) > double.Parse(priceFilter))
                        {
                            customList.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }

            customDb = customList;
            SwitchCategory(Custom, new RoutedEventArgs());
        }

        /// <summary>
        /// Creates the custom item menu list by first sorting each item into it's respective category then creating the buttons
        /// </summary>
        private void createCustomMenu()
        {
            ArrayList appetizers = new ArrayList();
            ArrayList mains = new ArrayList();
            ArrayList beverages = new ArrayList();
            ArrayList complimentary = new ArrayList();
            ArrayList desserts = new ArrayList();

            foreach (Database.Item item in customDb)
            {
                if (item.getCategory().Equals("Appetizers"))
                {
                    appetizers.Add(item);
                }
                else if (item.getCategory().Equals("Mains"))
                {
                    mains.Add(item);
                }
                else if (item.getCategory().Equals("Beverages"))
                {
                    beverages.Add(item);
                }
                else if (item.getCategory().Equals("Desserts"))
                {
                    desserts.Add(item);
                }
                else if (item.getCategory().Equals("Complimentary"))
                {
                    complimentary.Add(item);
                }
            }

            // Creates and adds buttons to the menu panel
            if (beverages.Count != 0) createButtons(beverages, "BEVERAGES");
            if (appetizers.Count != 0) createButtons(appetizers, "APPETIZERS");
            if (mains.Count != 0) createButtons(mains, "MAINS");
            if (desserts.Count != 0) createButtons(desserts, "DESSERTS");
            if (complimentary.Count != 0) createButtons(complimentary, "COMPLIMENTARY");
        }


        // Creates the Category labels and buttons for the Custom menu list
        private void createButtons(ArrayList list, String category)
        {
            if (list.Count > 0)
            {
                createCategoryLabel(category);
                foreach (Database.Item item in list)
                {
                    createItemButton(item);
                }
            }
        }

        // =======================================
        // END **** Search menu list handling ****
        // =======================================


        // ==================================
        // **** Button and list creation ****
        // ==================================


        // Method for creating in-line item list category labels
        private void createCategoryLabel(String name)
        {
            Label label = new Label();
            label.Content = name;
            label.Background = (Brush)Application.Current.MainWindow.FindResource("altTextColor");
            label.Foreground = (Brush)Application.Current.MainWindow.FindResource("textColor");
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.FontWeight = FontWeights.UltraBold;
            label.FontSize = 16;
            label.Height = 28;
            menuPanel.Children.Add(label);
        }

        /// <summary>
        /// Creates and adds a single button representing an item to the left side menu
        /// </summary>
        /// <param name="item"></param>
        public void createItemButton(Database.Item item)
        {
            // Initialize the button
            Button newButton = new Button();
            // Assign parameters to the button
            newButton.Name = item.getIdName();
            newButton.Height = 75;
            newButton.Width = 455;
            newButton.HorizontalAlignment = HorizontalAlignment.Left;

            newButton.Margin = new Thickness(2);
            newButton.Click += updateItemDetail;

            // Create a grid in which to put stuff inside
            Grid newGrid = new Grid();
            newGrid.Height = 75;
            newGrid.Width = 461;

            // Assign image
            Image newImage = new Image();
            newImage.HorizontalAlignment = HorizontalAlignment.Left;
            newImage.Height = 75;
            newImage.Width = 75;
            newImage.Source = item.getImage();
            newImage.VerticalAlignment = VerticalAlignment.Top;


            // Item name
            Label itemName = new Label();
            itemName.Content = item.getName();
            itemName.Margin = new Thickness(75, 18, 72, 17);
            itemName.FontSize = 14;
            itemName.FontWeight= FontWeights.Bold;
            itemName.HorizontalContentAlignment = HorizontalAlignment.Center;
            itemName.VerticalAlignment = VerticalAlignment.Center;

            // Item price
            Label price = new Label();
            price.Content = "$ " + item.getPrice();
            price.Margin = new Thickness(383, 18, 20, 17);
            price.FontSize = 14;
            price.FontWeight= FontWeights.Bold;
            price.HorizontalContentAlignment = HorizontalAlignment.Left;
            price.VerticalAlignment = VerticalAlignment.Center;

            // Add the the elements to the button
            newGrid.Children.Add(newImage);
            newGrid.Children.Add(price);
            newGrid.Children.Add(itemName);
            newButton.Content = newGrid;

            if (newButton.Name == selectedItem.getIdName())
            {
                newButton.Style = this.FindResource("SelectedButton") as Style;
                currentItem = newButton;
                newButton.Focus();                                                      // *********************This should make the list snap to it?
            }
            menuPanel.Children.Add(newButton);
        }


        // ======================================
        // END **** Button and list creation ****
        // ======================================
    }
}
