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

        public event PropertyChangedEventHandler PropertyChanged;

        public void Clear()
        {
            totalItems = 0;
            totalVolume = 0;
            totalWeight = 0;
            Items.Clear();
        }

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

        public string Status
        {
            get { return Running ? "Status: Running" : "Status: Idle"; }
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

        public bool UnloadEnabled
        {
            get { return unloadEnabled; }
            set { unloadEnabled = value; OnPropertyChanged(new PropertyChangedEventArgs("UnloadEnabled")); }
        }

        public BindingList<FoodItem> Items { get; set; }

        public int MaxItems { get; }

        public double MaxVolume { get; }

        public double MaxWeight { get; }

        public void Stop()
        {
            Running = false;
        }

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

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            Invoke(new Action(() => PropertyChanged?.Invoke(this, e)));
        }

        protected void Invoke(Delegate method)
        {
            Application.OpenForms[0].Invoke(method);
        }
    }
}
