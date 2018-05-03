using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliverySystem
{
    internal class FoodItem
    {
        public FoodItem(string name, double volume, double weight)
        {
            Name = name;
            Volume = volume;
            Weight = weight;
        }

        public string Name { get; }

        public double Volume { get; }

        public double Weight { get; }
    }
}
