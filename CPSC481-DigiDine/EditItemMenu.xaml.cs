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
using System.Windows.Shapes;

namespace CPSC481_DigiDine
{
    /// <summary>
    /// Interaction logic for EditItemMenu.xaml
    /// </summary>
    public partial class EditItemMenu : Window
    {
        private Database.Item selectedItem;
        private ReviewPage parentWindow;
        private int itemID;

        public EditItemMenu(ReviewPage _parentWindow, int _itemID )
        {
            parentWindow = _parentWindow;
            itemID = _itemID;
            InitializeComponent();

            DataContext = this;

            selectedItem = App.db.unorderedList[itemID];
            _itemQuantity = App.db.itemQuantity[itemID];
            updateItemDetail();
        }

        public void CloseEdit(object sender, EventArgs e)
        {
            parentWindow.refresh();
            parentWindow.closeItemEdit(this);
        }

        // current selected quantity of selected item
        private int _itemQuantity = 1;
        private string getItemQuantity()
        {
            return _itemQuantity.ToString();
        }

        private void resetItemQuantity()
        {
            _itemQuantity = 1;
            itemQuantity.Content = getItemQuantity();
        }

        public string ItemQuantity
        {
            get { return getItemQuantity(); }
        }

        // selected items total cost with option prices
        private double _itemTotalCost;
        public string ItemTotalCost
        {
            get { return "$" + _itemTotalCost.ToString("0.00"); }
            set { _itemTotalCost = Convert.ToDouble(value); }
        }

        public void callServer(object sender, RoutedEventArgs e)
        {
            // open thank you window
            CalledServerPopup calledServer = new CalledServerPopup();
            calledServer.Owner = parentWindow;
            calledServer.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            calledServer.Show();
        }

        public void subQuantity(object sender, RoutedEventArgs e)
        {
            if (_itemQuantity - 1 >= 1) _itemQuantity--;
            itemQuantity.Content = getItemQuantity();
            updatePrices();
        }

        public void addQuantity(object sender, RoutedEventArgs e)
        {
            _itemQuantity++;
            itemQuantity.Content = getItemQuantity();
            updatePrices();
        }

        public void addToOrder(object sender, RoutedEventArgs e)
        {
            App.db.unorderedList[itemID] = selectedItem;
            App.db.itemQuantity[itemID] = _itemQuantity;
            parentWindow.refresh();
            this.Close();
        }

        /// <summary>
        /// Updates the item detail overview on the right side of the menu page
        /// </summary>
        /// <param name="sender">menu item button</param>
        /// <param name="e"></param>
        public void updateItemDetail()
        {
            updatePrices();

            // Updating the page
            itemTitle.Content = selectedItem.getName();
            itemPhoto.Source = selectedItem.getImage();
            itemCost.Content = "$ " + selectedItem.getPrice();
            itemDesc.Text = selectedItem.getDescription();

            options.Children.Clear();

            Database.Option[] custOptions = selectedItem.getOptions();

            //foreach (Database.Option opt in custOptions)
            for (int i = 0; i < custOptions.Length; i++)
            {
                // all customization is wrapped in a stack panel
                StackPanel sp = new StackPanel();

                // border separation from each item
                Border border = new Border();
                border.BorderThickness = new Thickness(0, 1, 0, 0);
                border.BorderBrush = (SolidColorBrush)Application.Current.FindResource("buttonColor");

                sp.Children.Add(border);

                StackPanel optTitle = new StackPanel();
                optTitle.Orientation = Orientation.Horizontal;

                // title of the customizable option
                Label optName = new Label();
                optName.Content = custOptions[i].getName();
                optName.FontSize = 16;
                optName.FontWeight = FontWeights.Bold;
                optName.Foreground = (SolidColorBrush)Application.Current.FindResource("textColor");

                optTitle.Children.Add(optName);

                if (custOptions[i].isRequired())
                {
                    Label reqWarning = new Label();
                    reqWarning.Content = "  Selection Required";
                    reqWarning.FontSize = 16;
                    reqWarning.Foreground = Brushes.DarkRed;

                    optTitle.Children.Add(reqWarning);
                }

                sp.Children.Add(optTitle);

                // collection of choices for this option
                WrapPanel choices = new WrapPanel();
                choices.Orientation = Orientation.Horizontal;

                Database.Choice[] optChoices = custOptions[i].getChoices();

                //foreach (Database.Choice choice in optChoices)
                for (int j = 0; j < optChoices.Length; j++)
                {
                    Button button = new Button();

                    if (Convert.ToDouble(optChoices[j].getPrice()) > 0.0)
                    {
                        button.Content = optChoices[j].getName() + " $" + optChoices[j].getPrice();
                    }
                    else
                    {
                        button.Content = optChoices[j].getName();
                    }
                    button.Margin = new Thickness(10, 0, 0, 10);
                    if (optChoices[j].isSelected())
                    {
                        button.Style = (Style)Application.Current.FindResource("SelectedButton");
                    }

                    // attach to the button the option and choice as tag
                    button.Tag = new Database.OptionChoice(custOptions[i], optChoices[j]);
                    button.Click += new RoutedEventHandler(choiceSelected);

                    choices.Children.Add(button);
                }

                // add choices to each option
                sp.Children.Add(choices);
                // add option to customize section
                options.Children.Add(sp);
            }

            // add note textbox to end of customizable options
            Border noteBorder = new Border();
            noteBorder.BorderThickness = new Thickness(0, 1, 0, 0);
            noteBorder.BorderBrush = (SolidColorBrush)Application.Current.FindResource("buttonColor");
            options.Children.Add(noteBorder);

            Label noteLabel = new Label();
            noteLabel.Content = "Add a Note";
            noteLabel.FontSize = 16;
            noteLabel.FontWeight = FontWeights.Bold;
            noteLabel.Foreground = (SolidColorBrush)Application.Current.FindResource("textColor");
            options.Children.Add(noteLabel);

            TextBox note = new TextBox();
            note.Margin = new Thickness(10, 0, 10, 0);
            note.MinHeight = 60;
            options.Children.Add(note);
        }

        /// <summary>
        /// Logic behind selecting choices under each customizable option.
        /// </summary>
        /// <param name="sender">button of the choice selected</param>
        /// <param name="e"></param>
        private void choiceSelected(object sender, RoutedEventArgs e)
        {
            Button choiceButton = ((Button)sender);
            Database.OptionChoice optCh = (Database.OptionChoice)choiceButton.Tag;
            Database.Option option = optCh.option;
            Database.Choice choice = optCh.choice;

            // has this choice been selected or deselected?
            if (!choice.isSelected())
            {
                // check if only one choice can be selected and deselect the rest
                if (option.isSelectOne())
                {
                    // gets the panel that holds this options buttons
                    Panel panel = ((Button)sender).Parent as Panel;
                    foreach (UIElement elem in panel.Children)
                    {
                        Database.OptionChoice oc = (Database.OptionChoice)((elem as Button).Tag);
                        // deselect all the choices that are selected
                        if (oc.choice.isSelected() && !oc.choice.getName().Equals(choice.getName()))
                        {
                            oc.choice.setSelected();
                            (elem as Button).Style = (Style)Application.Current.FindResource("DefaultButton");
                            oc.option.setChoice(oc.choice);
                            (elem as Button).Tag = new Database.OptionChoice(oc.option, oc.choice);
                            //selectedItem.setOption(oc.option);
                        }
                    }
                }

                // update button and its style
                choice.setSelected();
                choiceButton.Style = (Style)Application.Current.FindResource("SelectedButton");
            }
            else
            {
                choice.setSelected();
                choiceButton.Style = (Style)Application.Current.FindResource("DefaultButton");
            }

            option.setChoice(choice);
            selectedItem.setOption(option);
            updatePrices();

            ((Button)sender).Tag = new Database.OptionChoice(option, choice);
        }

        private void updatePrices()
        {
            // Update Cart Price
            subtotalLabel.Content = "$ " + ReviewPage.getSubtotal().ToString("0.00");

            // Update Item Accumulative Price
            double total = Double.Parse(selectedItem.getPrice());

            foreach (var option in selectedItem.getOptions())
            {
                foreach (var choice in option.getChoices())
                {
                    if (choice.isSelected())
                    {
                        total += Double.Parse(choice.getPrice());
                    }
                }
            }

            total *= _itemQuantity;

            itemTotalCost.Content = "$" + total.ToString("0.00");
        }
    }
}