// Programma di Graziano Filippo
// ValtrighExchange 2.0

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace ValtrighExchange
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            balance_value.Text = "0 €";
            ListBox listBox1 = new ListBox();
            Thread receiveThread = new Thread(new ThreadStart(ReceiveValue));
            receiveThread.Start();
        }
        
        // Receive the value of ValtrigheCoin from the server
        public void ReceiveValue()
        {
            while (true)
            {
                try
                {
                    // Create a TcpClient socket and connect to the specified server
                    TcpClient client = new TcpClient("116.203.25.177", 5000);

                    // Get the stream to read and write data to the server
                    NetworkStream stream = client.GetStream();

                    // Send a message to the server
                    string message = "Give me the value of ValtrigheCoin";
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    // Receive a message from the server
                    data = new byte[256];
                    string response = string.Empty;
                    int bytes = stream.Read(data, 0, data.Length);
                    response = Encoding.ASCII.GetString(data, 0, bytes);
                    
                    current_value.Invoke((MethodInvoker)(() => current_value.Text = response.ToString()));

                    // Close the client socket
                    client.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Thread.Sleep(5000);
            }
        }

        // BUY AND SELL

        public void Buy(object sender, EventArgs e)
        {
            decimal value = numericUpDown1.Value;
            string text = "Buy: " + value.ToString() + " ValtrigheCoin at the price of " + current_value.Text + " €";

            if (numericUpDown1.Value > 0)
            {
                listBox1.Items.Add(text);

                try
                {
                    // Create a TcpClient socket and connect to the specified server
                    TcpClient client = new TcpClient("116.203.25.177", 5001);

                    // Get the stream to read and write data to the server
                    NetworkStream stream = client.GetStream();

                    // Send a message to the server
                    string message2 = "BUY " + value.ToString() + " " + current_value.Text;
                    byte[] data = Encoding.ASCII.GetBytes(message2);
                    stream.Write(data, 0, data.Length);

                    // Close the client socket
                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Please enter a value > 0");
            }
            numericUpDown1.Value = 0;
        }

        public void Sell(object sender, EventArgs e)
        {
            decimal value = numericUpDown2.Value;
            string text = "Sell: " + value.ToString() + " ValtrigheCoin at the price of " + current_value.Text + " €";

            if (numericUpDown2.Value > 0)
            {
                listBox1.Items.Add(text);

                try
                {
                    // Create a TcpClient socket and connect to the specified server
                    TcpClient client = new TcpClient("116.203.25.177", 5001);

                    // Get the stream to read and write data to the server
                    NetworkStream stream = client.GetStream();

                    // Send a message to the server
                    string message = "SELL " + value.ToString() + " " + current_value.Text;
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    // Close the client socket
                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Please enter a value > 0");
            }
            numericUpDown2.Value = 0;
        }

        // DEPOSIT AND WITHDRAW MONEY

        // Enable deposit money
        public void button_DepositMoney(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            button5.Visible = true;

            button1.Enabled = false;
            button2.Enabled = false;
        }

        // Deposit money
        public void DepositMoney(object sender, EventArgs e)
        {
            int value = Int32.Parse(textBox1.Text);
            int balance = Int32.Parse(balance_value.Text.Substring(0, balance_value.Text.Length - 2));
            if (value > 0)
            {
                balance += value;
                balance_value.Text = balance.ToString() + " €";
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Please enter a value > 0");
                textBox1.Text = "";
            }
            textBox1.Visible = false;
            button5.Visible = false;

            button1.Enabled = true;
            button2.Enabled = true;

            // Write movement to listBox
            listBox1.Items.Add("Deposit: " + value.ToString() + " €");
        }

        // Enable withdraw money
        public void button_WithdrawMoney(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            button6.Visible = true;

            button1.Enabled = false;
            button2.Enabled = false;
        }

        // Withdraw money
        public void WithdrawMoney(object sender, EventArgs e)
        {
            int value = Int32.Parse(textBox1.Text);
            int balance = Int32.Parse(balance_value.Text.Substring(0, balance_value.Text.Length - 2));
            if (balance >= value)
            {
                balance -= value;
                balance_value.Text = balance.ToString() + " €";
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Not enough money in your account!");
                textBox1.Text = "";
            }
            textBox1.Visible = false;
            button6.Visible = false;

            button1.Enabled = true;
            button2.Enabled = true;

            // Write movement to listBox
            listBox1.Items.Add("Withdraw: " + value.ToString() + " €");
        }
    }
}
