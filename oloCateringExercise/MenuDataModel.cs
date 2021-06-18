using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oloCateringExercise
{
    public enum FoodTypeValue
    {
        appetizer = 0,
        entree = 1,
        dessert = 2,
    }

    public class WebMenu
    {
        public string Restaurant { get; set; }
        public List<MenuItem> MenuItems { get; set; }
    }

    public class MenuItem
    {
        public string FoodType { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }

        public FoodTypeValue FoodTypeSorter
        {
            get
            {
                switch (FoodType.ToLower())
                {
                    case "dessert":
                        return FoodTypeValue.dessert;
                    case "entree":
                        return FoodTypeValue.entree;
                    default:
                        return FoodTypeValue.appetizer;
                }
            }
        }
        
        public float PriceSorter
        { 
            get
            {
                if (float.TryParse(Price, out float price))
                {
                    return price;
                }
                return 0;
            }
        }

    }

    public class MenuSort : IComparer<MenuItem>
    {
        // Sorting by FoodType(ascending) and then by Price(ascending).
        // Name should not matter since they should be all unique.
        public int Compare(MenuItem itemA, MenuItem itemB)
        {
            // First sort by food type ascending. 
            if (itemA.FoodTypeSorter > itemB.FoodTypeSorter)
            {
                return 1;
            }
            if (itemA.FoodTypeSorter < itemB.FoodTypeSorter)
            {
                return -1;
            }

            // Next sort by price ascending
            if (itemA.PriceSorter > itemB.PriceSorter)
            {
                return 1;
            }
            if (itemA.PriceSorter < itemB.PriceSorter)
            {
                return -1;
            }

            return 0; // Menu items likely match in FoodType and Price.
                      // If two items have the same price, we could sort by name, but no real need here I think.
        }
    }
}
