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
        private Random random = new Random();

        private Producer producerScan;
        private Producer producerArla;
        private Producer producerAxFood;

        private Consumer consumerIca;
        private Consumer consumerCoop;
        private Consumer consumerCityGross;

        public Form1()
        {
            InitializeComponent();
            InitializeFoodItem();
            InitializeBuffer();
            InitializeProducer();
            InitializeConsumer();
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

        private void InitializeBuffer()
        {
            buffer = new Buffer(BufferSize);
            progressBar.Maximum = BufferSize;
            progressBar.DataBindings.Add("Value", buffer, "Count");
            lblMaxStorageCapacity.Text = "Max capacity: " + BufferSize;
        }

        private void InitializeProducer()
        {
            producerScan = new Producer(buffer, random, foodItems);
            btnStartProducerScan.DataBindings.Add(new InverseBinding("Enabled", producerScan, "Running"));
            btnStopProducerScan.DataBindings.Add(new Binding("Enabled", producerScan, "Running"));
            lblProducerScanStatus.DataBindings.Add(new Binding("Text", producerScan, "Status"));

            producerArla = new Producer(buffer, random, foodItems);
            btnStartProducerArla.DataBindings.Add(new InverseBinding("Enabled", producerArla, "Running"));
            btnStopProducerArla.DataBindings.Add(new Binding("Enabled", producerArla, "Running"));
            lblProducerArlaStatus.DataBindings.Add(new Binding("Text", producerArla, "Status"));

            producerAxFood = new Producer(buffer, random, foodItems);
            btnStartProducerAxFood.DataBindings.Add(new InverseBinding("Enabled", producerAxFood, "Running"));
            btnStopProducerAxFood.DataBindings.Add(new Binding("Enabled", producerAxFood, "Running"));
            lblProducerAxFoodStatus.DataBindings.Add(new Binding("Text", producerAxFood, "Status"));
        }

        private void InitializeConsumer()
        {
            consumerIca = new Consumer(buffer, random);
            btnStartConsumerIca.DataBindings.Add(new InverseBinding("Enabled", consumerIca, "Running"));
            btnStopConsumerIca.DataBindings.Add(new Binding("Enabled", consumerIca, "Running"));
            lstConsumerIca.DataSource = consumerIca.Items;
            lstConsumerIca.DisplayMember = "Name";
            lblConsumerIcaStatus.DataBindings.Add(new Binding("Text", consumerIca, "Status"));
            lblConsumerIcaItemsLimit.Text = "Items: " + consumerIca.MaxItems;
            lblConsumerIcaWeightLimit.Text = "Weight: " + consumerIca.MaxWeight;
            lblConsumerIcaVolumeLimit.Text = "Volume: " + consumerIca.MaxVolume;

            consumerCoop = new Consumer(buffer, random);
            btnStartConsumerCoop.DataBindings.Add(new InverseBinding("Enabled", consumerCoop, "Running"));
            btnStopConsumerCoop.DataBindings.Add(new Binding("Enabled", consumerCoop, "Running"));
            lstConsumerCoop.DataSource = consumerCoop.Items;
            lstConsumerCoop.DisplayMember = "Name";
            lblConsumerCoopStatus.DataBindings.Add(new Binding("Text", consumerCoop, "Status"));
            lblConsumerCoopItemsLimit.Text = "Items: " + consumerCoop.MaxItems;
            lblConsumerCoopWeightLimit.Text = "Weight: " + consumerCoop.MaxWeight;
            lblConsumerCoopVolumeLimit.Text = "Volume: " + consumerCoop.MaxVolume;

            consumerCityGross = new Consumer(buffer, random);
            btnStartConsumerCityGross.DataBindings.Add(new InverseBinding("Enabled", consumerCityGross, "Running"));
            btnStopConsumerCityGross.DataBindings.Add(new Binding("Enabled", consumerCityGross, "Running"));
            lstConsumerCityGross.DataSource = consumerCityGross.Items;
            lstConsumerCityGross.DisplayMember = "Name";
            lblConsumerCityGrossStatus.DataBindings.Add(new Binding("Text", consumerCityGross, "Status"));
            lblConsumerCityGrossItemsLimit.Text = "Items: " + consumerCityGross.MaxItems;
            lblConsumerCityGrossWeightLimit.Text = "Weight: " + consumerCityGross.MaxWeight;
            lblConsumerCityGrossVolumeLimit.Text = "Volume: " + consumerCityGross.MaxVolume;
        }

        private void btnStartProducer_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button == btnStartProducerScan)
                producerScan.Start();
            else if (button == btnStartProducerArla)
                producerArla.Start();
            else
                producerAxFood.Start();
        }

        private void btnStopProducer_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button == btnStopProducerScan)
                producerScan.Stop();
            else if (button == btnStopProducerArla)
                producerArla.Stop();
            else
                producerAxFood.Stop();
        }

        private void btnStartConsumer_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button == btnStartConsumerIca)
                consumerIca.Start();
            else if (button == btnStartConsumerCoop)
                consumerCoop.Start();
            else
                consumerCityGross.Start();
        }

        private void btnStopConsumer_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button == btnStartConsumerIca)
                consumerIca.Stop();
            else if (button == btnStartConsumerCoop)
                consumerCoop.Stop();
            else
                consumerCityGross.Stop();
        }

        private void chkConsumerContinueLoad_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            if (checkBox == chkConsumerIcaContinueLoad)
                consumerIca.UnloadEnabled = chkConsumerIcaContinueLoad.Checked;
            else if (checkBox == chkConsumerCoopContinueLoad)
                consumerCoop.UnloadEnabled = chkConsumerCoopContinueLoad.Checked;
            else
                consumerCityGross.UnloadEnabled = chkConsumerCityGrossContinueLoad.Checked;
        }
    }
}
