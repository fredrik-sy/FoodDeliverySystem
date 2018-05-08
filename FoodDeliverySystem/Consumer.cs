using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodDeliverySystem
{
    class Consumer : INotifyPropertyChanged
    {
        private const int MaxRandomTimeout = 3000;
        private const int MinRandomTimeout = 1000;
        private const int UnloadTime = 5000;

        private Buffer storage;
        private Random random;
        private Thread thread;
        private bool running;

        private int totalItems;
        private double totalVolume;
        private double totalWeight;
        private bool unloadEnabled;

        public Consumer(Buffer buffer, Random random)
        {
            storage = buffer;
            this.random = random;
            Items = new BindingList<FoodItem>();
            MaxItems = random.Next(10, 20);
            MaxVolume = random.Next(5, 50);
            MaxWeight = random.Next(10, 50);
        }

        /// <summary>
        /// Event raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Removes all <see cref="FoodItem"/> from the <see cref="Consumer"/>.
        /// </summary>
        public void Clear()
        {
            totalItems = 0;
            totalVolume = 0;
            totalWeight = 0;
            Items.Clear();
        }

        /// <summary>
        /// Starts the thread to consume food.
        /// </summary>
        public void Start()
        {
            if (!Running)
            {
                Clear();
                Running = true;
                thread = new Thread(ConsumeFood)
                {
                    IsBackground = true
                };
                thread.Start();
            }
        }

        /// <summary>
        /// Gets the status of the <see cref="Consumer"/>.
        /// </summary>
        public string Status
        {
            get { return Running ? "Status: Running" : "Status: Idle"; }
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
        /// Gets or sets the unload option of the <see cref="Consumer"/>. 
        /// </summary>
        public bool UnloadEnabled
        {
            get { return unloadEnabled; }
            set { unloadEnabled = value; OnPropertyChanged(new PropertyChangedEventArgs("UnloadEnabled")); }
        }
        
        /// <summary>
        /// Gets or sets the <see cref="FoodItem"/> collection.
        /// </summary>
        public BindingList<FoodItem> Items { get; set; }

        /// <summary>
        /// Gets the maximum items the <see cref="Consumer"/> supports.
        /// </summary>
        public int MaxItems { get; }

        /// <summary>
        /// Gets the maximum volume the <see cref="Consumer"/> supports.
        /// </summary>
        public double MaxVolume { get; }

        /// <summary>
        /// Gets the maximum weight the <see cref="Consumer"/> supports.
        /// </summary>
        public double MaxWeight { get; }

        /// <summary>
        /// Stops the thread that consumes foods.
        /// </summary>
        public void Stop()
        {
            Running = false;
        }

        /// <summary>
        /// Consumes <see cref="FoodItem"/> from <see cref="Buffer"/>.
        /// </summary>
        private void ConsumeFood()
        {
            while (Running)
            {
                FoodItem foodItem = storage.Dequeue();

                if (foodItem != null)
                {
                    // Checks capacity limitations.
                    if (totalItems >= MaxItems
                        || foodItem.Volume + totalVolume > MaxVolume
                        || foodItem.Weight + totalWeight > MaxWeight)
                    {
                        if (UnloadEnabled)
                        {
                            Clear();
                            Thread.Sleep(UnloadTime);
                        }
                        else
                        {
                            Running = false;
                        }

                        continue;
                    }

                    Invoke(new Action(() => Items.Add(foodItem)));
                    totalItems++;
                    totalVolume += foodItem.Volume;
                    totalWeight += foodItem.Weight;

                    int timeoutValue = random.Next(MinRandomTimeout, MaxRandomTimeout);
                    Thread.Sleep(timeoutValue);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            Invoke(new Action(() => PropertyChanged?.Invoke(this, e)));
        }

        /// <summary>
        /// Invokes the delegate using the form thread.
        /// </summary>
        protected void Invoke(Delegate method)
        {
            Application.OpenForms[0].Invoke(method);
        }
    }
}
