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
        private Mutex bufferMutex;
        private Semaphore semaphoreReaders;
        private Semaphore semaphoreWriters;
        
        public Buffer(int size)
        {
            buffer = new FoodItem[size];
            bufferMutex = new Mutex();
            semaphoreReaders = new Semaphore(0, size);
            semaphoreWriters = new Semaphore(size, size);
        }

        public event EventHandler CountChanged;

        public int Count { get; private set; }

        public void Enqueue(FoodItem foodItem)
        {
            Trace.WriteLine("Enter Enqueue(FoodItem foodItem)");

            if (semaphoreWriters.WaitOne(Timeout))
            {
                Trace.WriteLine("Acquired semaphoreWriters");
                bufferMutex.WaitOne();
                Trace.WriteLine("Acquired bufferMutex");

                buffer[Count++] = foodItem;
                OnCountChanged(new EventArgs());

                bufferMutex.ReleaseMutex();
                Trace.WriteLine("Released bufferMutex");
                semaphoreReaders.Release();
                Trace.WriteLine("Released semaphoreReaders");
            }

            Trace.WriteLine("Exit Enqueue(FoodItem foodItem)");
        }

        public FoodItem Dequeue()
        {
            semaphoreReaders.WaitOne();
            bufferMutex.WaitOne();

            FoodItem foodItem = buffer[--Count];

            bufferMutex.ReleaseMutex();
            semaphoreWriters.Release();

            return foodItem;
        }

        protected void OnCountChanged(EventArgs e)
        {
            CountChanged?.Invoke(this, e);
        }
    }
}
