using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodDeliverySystem
{
    class Buffer : INotifyPropertyChanged
    {
        private const int Timeout = 1000;

        private FoodItem[] buffer;
        private int count;

        private Mutex mutex;
        private Semaphore semReaders;
        private Semaphore semWriters;

        public Buffer(int size)
        {
            buffer = new FoodItem[size];
            mutex = new Mutex();
            semReaders = new Semaphore(0, size);
            semWriters = new Semaphore(size, size);
        }

        /// <summary>
        /// Event raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the number of <see cref="FoodItem"/> contained in the <see cref="Buffer"/>;
        /// </summary>
        public int Count
        {
            get { return count; }
            private set { count = value; OnPropertyChanged(new PropertyChangedEventArgs("Count")); }
        }

        /// <summary>
        /// Enqueue <see cref="FoodItem"/> to the stack.
        /// </summary>
        public void Enqueue(FoodItem foodItem)
        {
            if (semWriters.WaitOne(Timeout))
            {
                mutex.WaitOne();

                buffer[Count++] = foodItem;

                mutex.ReleaseMutex();
                semReaders.Release();
            }
        }

        /// <summary>
        /// Dequeue <see cref="FoodItem"/> from the stack.
        /// </summary>
        public FoodItem Dequeue()
        {
            FoodItem foodItem = null;

            if (semReaders.WaitOne(Timeout))
            {
                mutex.WaitOne();

                foodItem = buffer[--Count];

                mutex.ReleaseMutex();
                semWriters.Release();
            }

            return foodItem;
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
        private void Invoke(Delegate method)
        {
            Application.OpenForms[0].Invoke(method);
        }
    }
}
