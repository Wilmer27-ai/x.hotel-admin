using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace x.hotel
{
    public partial class Reports : Form
    {
        private IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "x0WQCfAAdZVlyYXDbbIh9W5fG51aNI8uNj6zYmMn",
            BasePath = "https://x-hotel-451cb-default-rtdb.asia-southeast1.firebasedatabase.app"
        };

        private IFirebaseClient Client;

        public Reports()
        {
            InitializeComponent();
        }

        private void Reports_Load(object sender, EventArgs e)
        {
            // Initialize the Firebase client
            Client = new FireSharp.FirebaseClient(Config);

            // Fetch transaction data from the "Transactions" node
            FirebaseResponse response = Client.Get("Transactions");
            Dictionary<string, Transaction> transactions = response.ResultAs<Dictionary<string, Transaction>>();

            // Populate the DataGridView with transaction data
            PopulateDataGridView(transactions);
            textBox1.TextChanged += TxtSearchName_TextChanged;
        }

        private void PopulateDataGridView(Dictionary<string, Transaction> transactions)
        {
            // Clear existing rows in the DataGridView
            dataGridView1.Rows.Clear();

            // Add columns to the DataGridView if not added already
            if (dataGridView1.ColumnCount == 0)
            {
                dataGridView1.Columns.Add("TransId", "Transaction ID");
                dataGridView1.Columns.Add("CustomerName", "Customer Name");
                dataGridView1.Columns.Add("PhoneNumber", "Phone Number");
                dataGridView1.Columns.Add("GuestCount", "Guest Count");
                dataGridView1.Columns.Add("PaymentMethod", "Payment Method");
                dataGridView1.Columns.Add("StartDate", "Start Date");
                dataGridView1.Columns.Add("EndDate", "End Date");
                dataGridView1.Columns.Add("RoomNumber", "Room Number");
                dataGridView1.Columns.Add("TransAmount", "Transaction Amount");
                dataGridView1.Columns.Add("TransDate", "Transaction Date");
            }

            // Populate DataGridView with transaction details
            foreach (var transaction in transactions.Values)
            {
                dataGridView1.Rows.Add(
                    transaction.transId,
                    transaction.customerName,
                    transaction.customerPhoneNumber,
                    transaction.guestCount,
                    transaction.paymentMethod,
                    transaction.roomDetails.startDate,
                    transaction.roomDetails.endDate,
                    transaction.roomDetails.roomNumber,
                    transaction.transAmount,
                    transaction.transDate
                );
            }
        }
        private void TxtSearchName_TextChanged(object sender, EventArgs e)
        {
            // Get the search name from the TextBox
            string searchName = textBox1.Text.Trim().ToLower();

            // Filter DataGridView based on the search name
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Check if the cell is not null and the column exists
                if (row.Cells["CustomerName"] != null && row.Cells["CustomerName"].Value != null)
                {
                    // Check if the value contains the search name
                    if (row.Cells["CustomerName"].Value.ToString().ToLower().Contains(searchName))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
