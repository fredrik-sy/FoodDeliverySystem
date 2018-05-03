using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoodDeliverySystem
{
    class Producer : INotifyPropertyChanged
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
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Status
        {
            get { return Running ? "Status: Running" : "Status: Stopped"; }
        }

        public bool Running
        {
            get { return running; }
            private set
            {
                running = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Running"));
                OnPropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }
        
        public void Start()
        {
            if (!Running)
            {
                lock (produceFoodLock)
                {
                    Running = true;
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
            if (Running)
            {
                Running = false;
            }
        }

        private void ProduceFood()
        {
            lock (produceFoodLock)
            {
                while (Running)
                {
                    int foodIndex = random.Next(foodItems.Length);
                    storage.Enqueue(foodItems[foodIndex]);

                    int timeoutValue = random.Next(MinRandomTimeout, MaxRandomTimeout);
                    Thread.Sleep(timeoutValue);
                }
            }
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
