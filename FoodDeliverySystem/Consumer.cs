﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoodDeliverySystem
{
    class Consumer
    {
        private const int UnloadTime = 5000;

        private Buffer storage;
        private Thread thread;
        private bool running;

        public Consumer(Buffer buffer, Random random)
        {
            storage = buffer;
            MaxItems = random.Next(10, 20);
            MaxVolume = random.Next(5, 50);
            MaxWeight = random.Next(10, 50);
            Items = new FoodItem[MaxItems];
        }

        public void Start()
        {
            if (!running)
            {
                running = true;
                thread = new Thread(ConsumeFood)
                {
                    IsBackground = true
                };
                thread.Start();
            }
        }

        public bool UnloadEnabled { get; set; }

        public FoodItem[] Items { get; private set; }

        public int TotalItems { get; private set; }

        public double TotalVolume { get; private set; }

        public double TotalWeight { get; private set; }

        public int MaxItems { get; }

        public double MaxVolume { get; }

        public double MaxWeight { get; }

        public void Stop()
        {
            running = false;
        }

        private void ConsumeFood()
        {
            while (running)
            {
                FoodItem foodItem = storage.Dequeue();

                if (TotalItems >= MaxItems
                    || foodItem.Volume + TotalVolume > MaxVolume
                    || foodItem.Weight + TotalWeight > MaxWeight)
                {
                    if (UnloadEnabled)
                    {
                        TotalItems = 0;
                        TotalVolume = 0;
                        TotalWeight = 0;
                        Array.Clear(Items, 0, MaxItems);
                        Thread.Sleep(UnloadTime);
                    }
                    else
                    {
                        running = false;
                    }

                    continue;
                }

                Items[TotalItems] = foodItem;
                TotalItems++;
            }
        }
    }
}