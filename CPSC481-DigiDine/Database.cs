using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CPSC481_DigiDine
{
    /// <summary>
    /// Class creating a database containing items for the app Digidine.
    /// The class consists of a Dictionary which stores an object of type 'Item' with a corresponding key. Please note that when choosing a key for an item, naming controls (buttons, labels, etc...) does not like taking spaces or symbols except for underscores.
    /// 
    /// Items contain their respective variables as well as a nested list of type Option. Options represent the categories of customization available.
    /// 
    /// The other list that is nested within Options is the Choices list. Choices represent the individual choices that can be selected for each customiztion category.
    /// </summary>
    public class Database
    {
        private static Database instance = new Database(); // singleton instance

        // Lists for populating the different categories in the app
        public Dictionary<String, Item> appetizerList = new Dictionary<string, Item>();
        public Dictionary<String, Item> mainsList = new Dictionary<string, Item>();
        public Dictionary<String, Item> beverageList = new Dictionary<string, Item>();
        public Dictionary<String, Item> complimentaryList = new Dictionary<string, Item>();
        public Dictionary<String, Item> specialsList = new Dictionary<string, Item>();
        public Dictionary<String, Item> dessertList = new Dictionary<string, Item>();

        // selected options for an item 
        //public Dictionary<int, List<Database.Option>> selectedOptions;

        // will contain the quantity of the "unique" item order 
        public Dictionary<int, int> itemQuantity;

        // the item will contain a quantity of that current item id
        public Dictionary<int, Item> orderedList;
        public Dictionary<int, Item> unorderedList;

        // The complete database containing every item
        Dictionary<String, Item> dataSet;

        /// <summary>
        /// Constructor method, initializes the database
        /// </summary>
        private Database()
        {
            dataSet = new Dictionary<string, Item>();

            orderedList = new Dictionary<int, Item>();
            unorderedList = new Dictionary<int, Item>();
            itemQuantity = new Dictionary<int, int>();
            //selectedOptions = new Dictionary<int, List<Database.Option>>();

            createDB();
            categorizeItems();
            //addToUnOrderedList();
        }

        public Dictionary<String, Item> getDataSet()
        {
            return dataSet;
        }

        public static Database Instance
        {
            get { return instance; }
        }
        // ===================================
        // **** ReviewPage Access Methods ****
        // ===================================

        // FIX-ME: using whatever is in the database for now
        public void addToUnOrderedList()
        {
            var id = 0;
            foreach (var entry in dataSet)
            {
                unorderedList.Add(id, entry.Value);
                itemQuantity.Add(id, 2);
                id++;
            }
        }

        public int getQuantity(int id)
        {
            return itemQuantity[id];
        }

        //public List<Database.Option> options(int id)
        //{
        //    return selectedOptions[id];
        //}

        // =======================================
        // END **** ReviewPage Access Methods **** 
        // =======================================

        // =====================================
        // **** Database Object Definitions ****
        // =====================================

        /// <summary>
        /// The item and all of its associated info
        /// </summary>
        public struct Item
        {
            String idName;
            String name;
            String price;
            String description;

            Option[] options;
            String[] ingredients;
            String image;
            String category;
            Boolean isVegan = false;
            Boolean isVegetarian = false;

            /// <summary>
            /// Constructor method, initializes an item
            /// </summary>
            /// <param name="_idName"></param>
            /// <param name="newName"></param>
            /// <param name="newPrice"></param>
            /// <param name="newDescription"></param>
            /// <param name="newOptions"></param>
            /// <param name="newImage"></param>
            /// <param name="newIngredients"></param>
            /// <param name="newCategory"></param>
            public Item(String _idName, String newName, String newPrice, String newDescription, Option[] newOptions, String newImage, String[] newIngredients, String newCategory)
            {
                idName = _idName;
                name = newName;
                price = newPrice;
                description = newDescription;
                options = newOptions;
                image = newImage;
                ingredients = newIngredients;
                category = newCategory;
            }

            public Item(Item item)
            {
                idName = item.idName;
                name = item.name;
                price = item.price;
                description = item.description;
                options = item.options;
                image = item.image;
                ingredients = item.ingredients;
                category = item.category;
            }

            /// <summary>
            /// Returns the name used to identify this unique item.
            /// </summary>
            /// <returns></returns>
            public String getIdName()
            {
                return idName;
            }

            /// <summary>
            /// Returns the item name
            /// </summary>
            /// <returns></returns>
            public String getName()
            {
                return name;
            }

            /// <summary>
            /// Returns the item price
            /// </summary>
            /// <returns></returns>
            public String getPrice()
            {
                return price;
            }

            /// <summary>
            /// Returns the item description
            /// </summary>
            /// <returns></returns>
            public String getDescription()
            {
                return description;
            }

            /// <summary>
            /// Returns the list of option categories available for customizing
            /// </summary>
            /// <returns></returns>
            public Option[] getOptions()
            {
                Option[] copyOptions = { };
                foreach (Option option in options)
                {
                    copyOptions.Append(new Option(option));
                }
                return options;
            }

            /// <summary>
            /// Returns an option from the list
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public Option getOption(int index)
            {
                return new Option(options[index]);
            }

            /// <summary>
            /// Sets the image source for an image. The code would be imageBox.Source = getImage();
            /// </summary>
            /// <returns></returns>
            public BitmapImage getImage()
            {
                return new BitmapImage(new Uri(image, UriKind.Relative));
            }

            /// <summary>
            /// Returns a list of ingredients
            /// </summary>
            /// <returns></returns>
            public String[] getIngredients()
            {
                return ingredients;
            }

            /// <summary>
            /// Returns the items category type
            /// </summary>
            /// <returns></returns>
            public String getCategory()
            {
                return category;
            }


            public Boolean isItemVegan()
            {
                return isVegan;
            }

            public void setVegan()
            {
                isVegan = true;
            }

            public Boolean isItemVegetarian()
            {
                return isVegetarian;
            }

            public void setVegetarian()
            {
                isVegetarian = true;
            }

            public void setOption(Option copy)
            {
                for (int i = 0; i < options.Length; i++)
                {
                    if (options[i].getName().Equals(copy.getName()))
                    {
                        options[i] = copy;
                        return;
                    }
                }
            }
        }



        /// <summary>
        /// The option categories available for each item
        /// </summary>
        public struct Option
        {
            String name;
            Choice[] choices;
            // Determines if only one option can be chosen or many
            Boolean selectOne;
            // Determines if a choice should start highlighted.
            Boolean hasDefault = false;
            // Determines if this option is required to be selected
            Boolean required;

            public Option(String newName, Choice[] newChoices, Boolean _selectOne, Boolean _required = false)
            {
                name = newName;
                choices = newChoices;
                selectOne = _selectOne;
                required = _required;

                hasDefault = hasDefaultChoice();
            }

            public Option(Option option)
            {
                name = option.name;
                choices = option.choices;
                selectOne = option.selectOne;
                required = option.required;

                hasDefault = option.hasDefault;
            }

            public Boolean hasDefaultChoice()
            {
                foreach (Choice ch in choices)
                {
                    if (ch.isDefaultChoice())
                    {
                        return true;
                    }
                }
                return false;
            }

            public Boolean isRequired()
            {
                return required;
            }

            /// <summary>
            /// Returns the option name
            /// </summary>
            /// <returns></returns>
            public String getName()
            {
                return name;
            }

            /// <summary>
            /// Returns a list of available choices for the option
            /// </summary>
            /// <returns></returns>
            public Choice[] getChoices()
            {
                Choice[] copyChoices = { };
                foreach (Choice ch in choices)
                {
                    copyChoices.Append(new Choice(ch));
                }

                return choices;
            }

            /// <summary>
            /// Returns a choice from the list of choices for an option.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public Choice getChoice(int index)
            {
                return new Choice(choices[index]);
            }

            public void setChoice(Choice newChoice)
            {
                for (int i = 0; i < choices.Length; i++)
                {
                    if (choices[i].getName().Equals(newChoice.getName()))
                    {
                        choices[i] = newChoice;
                        return;
                    }
                }
            }

            public Boolean isSelectOne()
            {
                return selectOne;
            }

            /// <summary>
            /// Determines which text should be displayed for the option
            /// </summary>
            /// <returns></returns>
            public String selectLimit()
            {
                if (selectOne == false) return "(Select Any)";
                else return "(Select one)";
            }

            /// <summary>
            /// Returns the customization option title text label
            /// </summary>
            /// <returns></returns>
            public String printOption()
            {
                return name + " " + selectLimit();
            }

            public void setDefault(Boolean value)
            {
                hasDefault = value;
            }
        }



        /// <summary>
        /// The customization choices available for each customization category
        /// </summary>
        public struct Choice
        {
            String name;
            String price;
            Boolean selected = false;
            Boolean isDefault;

            public Choice(String newName, String newPrice, Boolean _isDefault = false)
            {
                name = newName;
                price = newPrice;
                isDefault = _isDefault;

                if (isDefault)
                {
                    selected = true;
                }
            }

            public Choice(Choice choice)
            {
                name = choice.name;
                price = choice.price;
                isDefault = choice.isDefault;

                if (isDefault)
                {
                    selected = true;
                }
            }

            /// <summary>
            /// Returns the name of the choice
            /// </summary>
            /// <returns></returns>
            public String getName()
            {
                return name;
            }

            /// <summary>
            /// Returns the price of the choice
            /// </summary>
            /// <returns></returns>
            public String getPrice()
            {
                return price;
            }

            public void selectChoice()
            {
                selected = !selected;
            }

            public bool isSelected() { return selected; }

            public void setSelected() { selected = !selected; }

            public bool isDefaultChoice() { return isDefault; }

            /// <summary>
            /// Returns a choice button text label
            /// </summary>
            /// <returns></returns>
            public String printChoice()
            {
                if (price == "0") return name;
                else return name + " + $ " + price;
            }
        }

        public struct OptionChoice
        {
            public Option option;
            public Choice choice;

            public OptionChoice(Option opt, Choice ch)
            {
                option = opt;
                choice = ch;
            }
        }

        // =========================================
        // END **** Database Object Definitions ****
        // =========================================

        // =================================
        // **** Database Access Methods ****
        // =================================

        /// <summary>
        /// Getter method for items within the list.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Item getItem(String key)
        {
            if (dataSet.ContainsKey(key)) return new Item(dataSet[key]);
            else throw new Exception("Item isn't in the list, check your spelling");
        }

        // =====================================
        // END **** Database Access Methods ****
        // =====================================

        // ===========================
        // **** Database Creation ****
        // ===========================

        /// <summary>
        /// Initialize the database, add items
        /// </summary>
        public void createDB()
        {
            Choice[] choices0;
            Choice[] choices1;
            Choice[] choices2;
            Option option0;
            Option option1;
            Option option2;
            String[] ingredients;
            Item item;

            // Apple Pie
            choices0 = new Choice[] { new Choice("Vanilla", "2.00"), new Choice("Chocolate", "2.00"), new Choice("Strawberry", "2.00"), new Choice("Butterscotch", "2.00") };
            option0 = new Option("Add Ice Cream", choices0, true);
            choices1 = new Choice[] { new Choice("1 Slice", "1.25"), new Choice("2 slices", "2.00"), new Choice("3 Slices", "3.00") };
            option1 = new Option("Add Slice of Cheddar Cheese", choices1, true);
            ingredients = new String[] { "Flour", "Sugar", "Salt", "Butter", "Eggs", "Lemon", "Apples", "Cinnamon", "Nutmeg", "Gluten" };
            item = new Item("ApplePie", "Apple Pie", "11.50",
                "Fresh hand-made apple pie crafted and baked in-house every morning.",
                new Option[] { option0, option1 }, "/img/Apple Pie.jpg", ingredients, "Desserts");
            dataSet.Add(item.getIdName(), item);
            specialsList.Add(item.getIdName(), item);


            // Basque Cheesecake
            ingredients = new String[] { "Almond florentine", "Berries", "Honey", "Chantilly cream", "Butter", "Graham wafer crumbs", "Cream cheese", "Sugar", "Gluten", "Dairy" };
            item = new Item("BasqueCheesecake", "Basque Cheesecake", "12.50",
                "A delicious almond florentine burnt top cheesecake served with berries and a jammy glaze of honey.",
                new Option[] { }, "/img/Basque Cheesecake.jpg", ingredients, "Desserts");
            dataSet.Add(item.getIdName(), item);


            // Chicken Alfredo
            choices0 = new Choice[] { new Choice("Cajun", "0"), new Choice("Grilled", "0") };
            option0 = new Option("Select Chicken Style", choices0, true);
            ingredients = new String[] { "Chicken", "Cheese", "Wheat", "Eggs", "Eggs", "Parsley", "Olive Oil", "Garlic", "Cream", "Gluten", "Dairy" };
            item = new Item("ChickenFettuccineAlfredo", "Chicken Fettuccine Alfredo", "24.75",
                "A true in-house classic: Fettuccine with our signature creamy alfredo sauce served over your choice of grilled or cajun chicken and Grana Padano cheese.",
                new Option[] { option0 }, "/img/Chicken Alfredo.jpg", ingredients, "Mains");
            dataSet.Add(item.getIdName(), item);


            // Chicken Nuggets
            choices0 = new Choice[] { new Choice("None", "0"), new Choice("Plum Sauce", "0"), new Choice("Honey Mustard", "0"), new Choice("Ketchup", "0"),
                new Choice("Siracha", "0"), new Choice("Ranch", "0"), new Choice("Hot Mustard", "0"), new Choice("Buffalo Sauce", "0") };

            option0 = new Option("Choose a Dipping Sauce", choices0, true);
            ingredients = new String[] { "Chicken", "Bread", "Gluten" };
            item = new Item("ChickenNuggies", "Chicken Nuggies", "18.75",
                "Soft yet crunchy chicken nuggets served in batches of 24 with a choice of dipping sauce.",
                new Option[] { option0 }, "/img/Chicken Nuggies.jpg", ingredients, "Mains");
            dataSet.Add(item.getIdName(), item);


            // Chicken Mini Tacos
            ingredients = new String[] { "Chicken", "Taco Shell", "Lettuce", "Cheese", "Tomato", "Gluten", "Dairy" };
            item = new Item("ChickenMiniTacos", "Chicken Mini Tacos", "9.49",
                "Hard shelled tacos beautifully seasoned to enrich the taco experience. All in a fun, shareable size!",
                new Option[] { }, "/img/Chicken Tacos.png", ingredients, "Appetizers");
            dataSet.Add(item.getIdName(), item);


            // Chicken Tendies
            choices0 = new Choice[] { new Choice("Plum Sauce", "0"), new Choice("Honey Mustard", "0"), new Choice("Ketchup", "0"),
                new Choice("Siracha", "0"), new Choice("Ranch", "0"), new Choice("Hot Mustard", "0"), new Choice("Buffalo Sauce", "0") };
            option0 = new Option("Choose a Dipping Sauce", choices0, true);
            ingredients = new String[] { "Chicken", "Bread", "Gluten", "Dairy" };
            item = new Item("ChickenTendies", "Chicken Tendies", "18.69",
                "Chicken strips done the way Earls mom used to make them: A taste that is sure to bring on a wave of nostalgia.",
                new Option[] { option0 }, "/img/Chicken Tendies.jpg", ingredients, "Mains");
            dataSet.Add(item.getIdName(), item);


            // Chicken Wings
            choices0 = new Choice[] { new Choice("Salt & Pepper", "0"), new Choice("Honey Garlic", "0"), new Choice("Lemon Pepper", "0"),
                new Choice("Chipotle Lime", "0"), new Choice("Garlic Parmesan", "0"), new Choice("Honey Butter Parmesan", "0"), new Choice("Jack Daniels Smoky BBQ", "0"),
                new Choice("Cilantro Lime", "0"), new Choice("Sweet thai chili", "0"), new Choice("Siracha", "0"), new Choice("Buffalo", "0"), new Choice("Hot Wings", "0"), new Choice("Suicide Wings", "0") };
            option0 = new Option("Choose a flavor", choices0, true);
            ingredients = new String[] { "Chicken" };
            item = new Item("ChickenWings", "Chicken Wings", "11.00",
                "1 lb of a mix of chicken wings and chicken drumsticks glazed with a sauce or seasoned with a dryrub.",
                new Option[] { option0 }, "/img/Chicken Wings.jpg", ingredients, "Mains");
            dataSet.Add(item.getIdName(), item);



            // Fork
            ingredients = new string[] { "Fork" };
            item = new Item("Fork", "Fork", "0.00",
                "A dining utensil commonly used for picking up items via impaling. ",
                new Option[] { }, "/img/Fork.jpg", ingredients, "Complimentary");
            dataSet.Add(item.getIdName(), item);



            // Fresh Lettuce Leaf Salad
            choices0 = new Choice[] { new Choice("No Dressing", "0"), new Choice("Caesar", "0"), new Choice("Ranch", "0"), new Choice("Italian", "0"), new Choice("Bleu Cheese", "0") };
            option0 = new Option("Select Dressing (Free)", choices0, true);
            choices1 = new Choice[] { new Choice("Greek Chicken", "3.45"), new Choice("Cajun Chicken", "4.95"), new Choice("Diced Ham", "3.45"), new Choice("Blackened Steak", "5.95") };
            option1 = new Option("Add Meats", choices1, false);
            choices2 = new Choice[] { new Choice("Sliced Avocado", "2.00"), new Choice("Feta", "1.50"), new Choice("Croutons", "1.00"), new Choice("Cherry Tomatoes", "1.50"), new Choice("Corn Kernals", "1.50"), new Choice("Black Beans", "1.50") };
            option2 = new Option("Add Vegetables", choices2, false);
            ingredients = new string[] { "Lettuce", "Cabbage", "Carrots" };
            item = new Item("FreshLettuceLeafSalad", "Fresh Lettuce Leaf Salad", "12.75",
                "Crunchy romaine lettuce tossed with shredded fresh cabbaged and carrots.",
                new Option[] { option0, option1, option2 }, "/img/salad.png", ingredients, "Appetizers");
            item.setVegan();
            item.setVegetarian();
            dataSet.Add(item.getIdName(), item);


            // Golden Fried Chicken
            ingredients = new String[] { "Chicken", "Eggs", "Gluten", "Wheat", "Milk", "Dairy" };
            item = new Item("GoldenFriedChicken", "Golden Fried Chicken", "19.75",
                "Half a chicken marinated in Earls secret chicken brine then deliciously fried in Earls secret oil formula for a crispy golden crunch.",
                new Option[] { }, "/img/Golden Fried Chicken.png", ingredients, "Mains");
            dataSet.Add(item.getIdName(), item);


            // Hearty Chicken Noodle Soups
            ingredients = new String[] { "Chicken", "Carrots", "Wheat", "Gluten", "Peas", "Butter", "Onion", "Parsley", "Cream", "Potatoes" };
            item = new Item("HeartyChickenNoodleSoup", "Hearty Chicken Noodle Soup", "9.59",
                "Earls take on the American Classic Chicken Noodle soup. Brewed from locally sourced chickens and produce.",
                new Option[] { }, "/img/Hearty Chicken Noodle Soup.jpg", ingredients, "Appetizers");
            dataSet.Add(item.getIdName(), item);


            // Jasper Brewing - Crisp Pils
            choices0 = new Choice[] { new Choice("12 OZ", "0"), new Choice("16 OZ", "3.00") };
            option0 = new Option("Size", choices0, true);
            //option0.setDefault(true);
            ingredients = new string[] { "Wheat", "Alcohol", "Gluten" };
            item = new Item("JasperBrewingCrispPils", "Jasper Brewing - Crisp Pils", "8.95", "A true Pilsner - clean Alberta grain flavour followed by a dry crisp hop finish. Brighter than your common domestic lager. Delicious 'til the last sip",
                new Option[] { option0 }, "/img/Jasper Crisp Pils.jpg", ingredients, "Beverages");
            dataSet.Add(item.getIdName(), item);
            specialsList.Add(item.getIdName(), item);


            // Merlot +  Syrah - Cherries & Rainbows
            choices0 = new Choice[] { new Choice("5 OZ", "13.25"), new Choice("7 OZ", "18.25"), new Choice("9 OZ", "22.25"), new Choice("750 ML", "60.00") };
            option0 = new Option("Size", choices0, true);
            //option0.setDefault(false);
            ingredients = new string[] { "Alcohol", "Grapes" };
            item = new Item("MerlotSyrahCherriesRainbows", "Merlot + Syrah - Cherries & Rainbows", "13.25", "Unconventional and lighthearted, Cherries & Rainbows hails from Minervois, France. " +
                "It’s made with zero added sulfur (sans soufre in French) in an eco-friendly winery. Cherries & Rainbows is ideal for the modern wine drinker in search of sustainably farmed, small lot options.",
                new Option[] { option0 }, "/img/Merlot + Syrah - Cherries &  Rainbows.png", ingredients, "Beverages");
            item.setVegan();
            item.setVegetarian();
            dataSet.Add(item.getIdName(), item);


            // Not-a-Big Mac
            choices0 = new Choice[] { new Choice("Single", "0"), new Choice("Double", "3.00"), new Choice("Triple", "6.00") };
            option0 = new Option("Number of Patties", choices0, true);
            choices1 = new Choice[] { new Choice("Pickles", "0.75"), new Choice("Bacon", "1.25"), new Choice("Not-a-Big Mac Sauce", "1.00") };
            option1 = new Option("Additional Addons", choices1, false);
            option1.setDefault(false);
            ingredients = new String[] { "Wheat", "Beef", "Cheese", "Pickles", "Lettuce", "Ketchup", "Gluten", "Dairy" };
            item = new Item("NotaBigMac", "Not-a-Big Mac", "18.25",
                "We wanted something that would make you squeel in delight, and so many iterations later, here is our signature burger with no equal in sight",
                new Option[] { option0, option1 }, "/img/Not-a-Big Mac.png", ingredients, "Mains");
            dataSet.Add(item.getIdName(), item);
            specialsList.Add(item.getIdName(), item);


            // Patron Cadillac Margarita
            ingredients = new string[] { "Tequila", "Orange liqueur", "Lime", "Alcohol" };
            item = new Item("PatronCadillacMargarita", "Patron Cadillac Margarita", "18.00",
                "A truly exquisite mix of only the finest lightly aged reposado tequila with brandy-based orange liqueur and lime juice for a light amber hue with oaky vanilla notes.",
                new Option[] { }, "/img/Patron Cadillac Margarita.png", ingredients, "Beverages");
            item.setVegan();
            item.setVegetarian();
            dataSet.Add(item.getIdName(), item);


            // Ribeye + Truffle Butter
            choices0 = new Choice[] { new Choice("9 OZ", "0"), new Choice("12 OZ", "4.00"), new Choice("16 OZ", "10.00"), new Choice("20 OZ", "16.00"), new Choice("32 OZ", "24.00") };
            option0 = new Option("Size", choices0, true);
            choices1 = new Choice[] { new Choice("Rare", "0"), new Choice("Medium Rare", "0"), new Choice("Medium", "0"), new Choice("Medium well", "0"), new Choice("Well Done", "0") };
            option1 = new Option("Rarity of the meat", choices1, true);
            option1.setDefault(false);
            ingredients = new string[] { "Beef", "Butter", "Truffle" };
            item = new Item("RibeyeTruffleButter", "Ribeye + Truffle Butter", "57.00",
                "A premium cut of the highest quality from grass fed Alberta raised calves and braised in our special truffle infused butter. ",
                new Option[] { option0, option1 }, "/img/Ribeye + Truffle Butter.png", ingredients, "Mains");
            dataSet.Add(item.getIdName(), item);


            // Southwest Caesar Chicken Wrap
            ingredients = new string[] { "Chicken", "Wheat", "Gluten", "Lettuce", "Caesar", "Eggs" };
            item = new Item("SouthwestCaesarChickenWrap", "Southwest Caesar Chicken Wrap", "24.00",
                "The Earls take on the classic Southwest Caesar Chicken Wrap. ",
                new Option[] { }, "/img/Southwest Caesar Chicken Wrap.jpg", ingredients, "Mains");
            dataSet.Add(item.getIdName(), item);


            // Spinach Stuffed Chicken Breast
            ingredients = new string[] { "Chicken", "Gluten", "Spinach" };
            item = new Item("SpinachStuffedChickenBreast", "Spinach Stuffed Chicken Breast", "24.00",
                "The Earls classic, made with locally sourced chickens and produce. ",
                new Option[] { }, "/img/Spinach Stuffed Chicken Breast.jpg", ingredients, "Mains");
            dataSet.Add(item.getIdName(), item);



            // Straw
            ingredients = new string[] { "Straw" };
            item = new Item("Straw", "Plastic Drinking Straw", "0.00",
                "A straw used for drinking. ",
                new Option[] { }, "/img/Straw.jpg", ingredients, "Complimentary");
            dataSet.Add(item.getIdName(), item);




        }


        private void categorizeItems()
        {
            foreach (KeyValuePair<String, Item> entry in dataSet)
            {
                String toSort = entry.Value.getCategory();
                if (toSort.Equals("Beverages")) beverageList.Add(entry.Key, entry.Value);
                else if (toSort.Equals("Appetizers")) appetizerList.Add(entry.Key, entry.Value);
                else if (toSort.Equals("Mains")) mainsList.Add(entry.Key, entry.Value);
                else if (toSort.Equals("Desserts")) dessertList.Add(entry.Key, entry.Value);
                else if (toSort.Equals("Complimentary")) complimentaryList.Add(entry.Key, entry.Value);
            }
        }

        // ===============================
        // END **** Database Creation ****
        // ===============================
    }
} 

