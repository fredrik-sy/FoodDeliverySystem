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
    class Buffer
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

        public event EventHandler CountChanged;

        public int Count
        {
            get { return count; }
            private set { count = value; OnCountChanged(new EventArgs()); }
        }

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

        protected void OnCountChanged(EventArgs e)
        {
            Application.OpenForms[0].Invoke(new Action(() => { CountChanged?.Invoke(this, e); }));
        }
    }
}
