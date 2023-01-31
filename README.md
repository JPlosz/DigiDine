
# How to Run 
1. Open the "cpsc481-foodmenu-f22-main" folder (assuming that the file has been unzipped)
2. Find a file called "run" (run.cmd) and double click it
3. A window should popup and you can start navigating (check out the *Navigation to Every Page + What You Can Do* section)

# Requirements Implemented 
**MUST INCLUDE REQUIREMENTS**
* Modifying an item’s options that’s in the cart - the "Edit" button in the Unordered Section of the Review Order Page
* Placing a partial order indicated in the Unordered vs. Ordered sections
* Details on ingredients
* Ability to search for specific items - implemented in the Filter Page
* Search the menu for a specific category - implemented by the different category buttons in the Menu Page
* Search the menu for discounted items - implemented in the "Specials" category
*  Search the menu for items under a specific price - implemented by the Filter Page through "price range" filter
* Price provided for each item - listed beside the item name in the Menu Page
* Summary of items selected to order - the grand total is indicated in the Review Order Page
* Total price of item selected to order - indicated on the bottom of the Item Details View/Page on the right-hand side of the split screen

**SHOULD INCLUDE**
* Calling a server - implemented with the "Call a Server" button 
* Request common, no-cost items
* Ability to tell if an item conforms to a specific set of dietary restrictions or allergies - though it was not completely indicated on the Item Details view alone, a user can apply dietary restrctions in the Filter's page which would filter out items that contain that restriction
* Changing the quantity or deleting an item that was added to cart 
* Ability to filter out items

# Navigation to Every Page + What You Can Do

## Menu Page:

* This is the “landing” page of the app so there is no need to navigate from it
* You can browse through the list of items on the left side of the split screen and click on an item to view it’s details (Item Details Page/View) 
* If there are options to customize the current dish, the CUSTOMIZE section will contain categorized by types of options (i.e Sauces, Toppings) and the available options
* Other customization requests should be noted in the “Add Note” text box



## Review Page:

* From Menu Page, click the Cart icon
* Click “Edit” to edit an item’s options and change quantity
* Click “X” to delete an item from your cart
* To view the selected options for an item, click the “v” expander
* Click “Return to Menu” to go back to the Menu Page
* Click “Call Server” to get a server to come to your table. This will most likely be clicked when there are problems that can’t be solved within the App (i.e: need some clarification with this item here) 
* When a user is ready to pay, they can leave the tablet where it is and go to the front desk. 
* Note: if items are currently in the “unordered” section then those won’t be added to your total payment, whatever the user has submitted in the Ordered section is what they will be paying for (see subtotal)

## Filter Page
* From the Menu Page, click “Filter” 
* Note: The Filter Page replaces the Item Details View 
* In the search bar, type in a keyword like: “Chicken” and hit the “Find” button (was all the item’s tagged? i should mention this)
* To apply a restriction under “Dietary Restrictions” , check on the box. This is will be dynamically
* To apply a price range, check the box beside it and move the slider to the price you want

## Edit Item Popup “Page”/window
* Ensure an item is added to the cart 
* Click the “Cart” Icon to visit the Review Page
* Click “Edit” to prompt the Edit Item window 
* Adjust the options if applicable or increase quantity if needed
* To discard changes, leave the selected options (if applicable) as is and hit "Apply Changes"

## Sample executions:
**Example #1 - “I want to order something that has chicken in the dish”**
1. From Menu Page, click “Filter”
2. In the search bar, type in “Chicken”
3. Click “Chicken Wings” in the left-hand side of the split screen 
4. Select a sauce option
5. Click “Add to order”

**Example #2 - “I need a straw”**
1. Browse categories in the Menu Page
2. Find “Complimentary” in one of the categories and click it
3. Click on “Plastic Drinking Straw” in the left-hand side of the split screen 
4. Click “Add to order”
