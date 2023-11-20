using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;

namespace x.hotel
{
    public partial class List_of_Guests : Form
    {
        private IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "x0WQCfAAdZVlyYXDbbIh9W5fG51aNI8uNj6zYmMn",
            BasePath = "https://x-hotel-451cb-default-rtdb.asia-southeast1.firebasedatabase.app"
        };

        private IFirebaseClient Client;

        public List_of_Guests()
        {
            InitializeComponent();
        }

        private void List_of_Guests_Load(object sender, EventArgs e)
        {
            // Initialize the Firebase client and load data
            Client = new FireSharp.FirebaseClient(Config);

            // Set up columns in the DataGridView
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Add("transId", "Transaction ID");
            dataGridView1.Columns.Add("customerName", "Customer Name");
            dataGridView1.Columns.Add("roomKey", "Room Key");
            dataGridView1.Columns.Add("roomClassification", "Room Classification");
            dataGridView1.Columns.Add("startDate", "Start Date");
            dataGridView1.Columns.Add("endDate", "End Date");
            dataGridView1.Columns.Add("transAmount", "Transaction Amount");

            // Load and display data into the DataGridView when the form is loaded
            LoadData();
        }

        private void LoadData()
        {
            // Fetch data from "Transactions" node
            FirebaseResponse transactionsResponse = Client.Get("Transactions");
            Dictionary<string, Transaction> transactions = JsonConvert.DeserializeObject<Dictionary<string, Transaction>>(transactionsResponse.Body);

            // Fetch data from "Rooms" node
            FirebaseResponse roomsResponse = Client.Get("Rooms");
            Dictionary<string, Room> rooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(roomsResponse.Body);

            // Display relevant information in the DataGridView
            foreach (var transaction in transactions)
            {
                if (transaction.Value.roomDetails != null && rooms.ContainsKey(transaction.Value.roomDetails.roomNumber.ToString()))
                {
                    Room room = rooms[transaction.Value.roomDetails.roomNumber.ToString()];

                    dataGridView1.Rows.Add(
                        transaction.Key,
                        transaction.Value.customerName,
                        transaction.Value.roomDetails.roomNumber,
                        room.roomClassification,
                        transaction.Value.roomDetails.startDate,
                        transaction.Value.roomDetails.endDate,
                        transaction.Value.transAmount
                    );
                }
            }
        }
            private void button1_Click(object sender, EventArgs e)
            {

            }
        }
    }
