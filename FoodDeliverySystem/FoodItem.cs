using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliverySystem
{
    internal struct FoodItem
    {
        public readonly string Name;

        public readonly double Volume;

        public readonly double Weight;

        public FoodItem(string name, double volume, double weight)
        {
            Name = name;
            Volume = volume;
            Weight = weight;
        }
    }
}
