using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoodDeliverySystem
{
    class Producer
    {
        private const int MaxRandomTimeout = 2000;
        private const int MinRandomTimeout = 500;

        private Buffer storage;
        private FoodItem[] foodItems;
        private Random random;

        private Thread thread;
        private bool running;
        private object produceFoodLock = new object();

        public Producer(Buffer buffer, Random random, FoodItem[] foodItems)
        {
            storage = buffer;
            this.random = random;
            this.foodItems = foodItems;
            running = false;
        }

        public void Start()
        {
            if (!running)
            {
                lock (produceFoodLock)
                {
                    running = true;
                    thread = new Thread(new ThreadStart(ProduceFood))
                    {
                        IsBackground = true
                    };
                }

                thread.Start();
            }
        }

        public void Stop()
        {
            if (running)
            {
                running = false;
            }
        }

        private void ProduceFood()
        {
            lock (produceFoodLock)
            {
                while (running)
                {
                    int foodIndex = random.Next(foodItems.Length);
                    storage.Enqueue(foodItems[foodIndex]);

                    int timeoutValue = random.Next(MinRandomTimeout, MaxRandomTimeout);
                    Thread.Sleep(timeoutValue);
                }
            }
        }
    }
}
