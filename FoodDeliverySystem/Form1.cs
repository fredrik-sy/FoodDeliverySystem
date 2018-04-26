using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodDeliverySystem
{
    public partial class Form1 : Form
    {
        private const int BufferSize = 10;

        private FoodItem[] foodItems;
        private Buffer buffer;
        private Random random;
        private Producer producerScan;
        private Consumer consumerIca;

        public Form1()
        {
            InitializeComponent();
            InitializeFoodItem();
            InitializeProducerConsumer();
        }

        private void InitializeFoodItem()
        {
            foodItems = new FoodItem[10];
            foodItems[0] = new FoodItem("Milk", 2, 2.5);
            foodItems[1] = new FoodItem("Cream", 0.5, 0.2);
            foodItems[2] = new FoodItem("Youghurt", 1.5, 2);
            foodItems[3] = new FoodItem("Butter", 0.6, 0.4);
            foodItems[4] = new FoodItem("Flower", 0.2, 0.25);
            foodItems[5] = new FoodItem("Sugar", 4, 0.6);
            foodItems[6] = new FoodItem("Salt", 1.4, 1);
            foodItems[7] = new FoodItem("Bread", 2, 0.2);
            foodItems[8] = new FoodItem("Ham", 2.2, 3);
            foodItems[9] = new FoodItem("Soda", 2.5, 3);
        }

        private void InitializeProducerConsumer()
        {
            buffer = new Buffer(BufferSize);
            buffer.CountChanged += Buffer_CountChanged;
            progressBar.Maximum = BufferSize;
            lblMaxCapacity.Text = string.Format("Max capacity: {0}", BufferSize);

            random = new Random();
            producerScan = new Producer(buffer, random, foodItems);
            consumerIca = new Consumer(buffer, random);
        }

        private void Buffer_CountChanged(object sender, EventArgs e)
        {
            Invoke(new Action(() => { progressBar.Value = buffer.Count; }));
        }

        private void btnStartProducerScan_Click(object sender, EventArgs e)
        {
            btnStartProducerScan.Enabled = false;
            btnStopProducerScan.Enabled = true;
            producerScan.Start();
            lblProducerScanStatus.Text = "Status: Running";
        }

        private void btnStopProducerScan_Click(object sender, EventArgs e)
        {
            btnStartProducerScan.Enabled = true;
            btnStopProducerScan.Enabled = false;
            producerScan.Stop();
            lblProducerScanStatus.Text = "Status: Stopped";
        }

        private void btnStartConsumerIca_Click(object sender, EventArgs e)
        {
            btnStartConsumerIca.Enabled = false;
            btnStopConsumerIca.Enabled = true;
            consumerIca.Start();
        }
    }
}
