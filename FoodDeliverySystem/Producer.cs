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

        /// <summary>
        /// Event raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the status of the <see cref="Producer"/>.
        /// </summary>
        public string Status
        {
            get { return Running ? "Status: Running" : "Status: Stopped"; }
        }

        /// <summary>
        /// Gets the running status of the thread.
        /// </summary>
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

        /// <summary>
        /// Starts the thread to produce food.
        /// </summary>
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

        /// <summary>
        /// Stops the thread that produces foods.
        /// </summary>
        public void Stop()
        {
            if (Running)
            {
                Running = false;
            }
        }

        /// <summary>
        /// Produces the <see cref="FoodItem"/> to <see cref="Buffer"/>.
        /// </summary>
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

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="e"></param>
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
