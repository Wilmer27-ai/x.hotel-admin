using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System.Linq;

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
            dataGridView1.Columns.Add("roomNumber", "Room Number");
            dataGridView1.Columns.Add("startDate", "Start Date");
            dataGridView1.Columns.Add("endDate", "End Date");
            dataGridView1.Columns.Add("transAmount", "Transaction Amount");

            // Initialize the Firebase client and load data
            Client = new FireSharp.FirebaseClient(Config);
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.Columns.Add("Key", "Room Key");
            dataGridView2.Columns.Add("roomName", "Room Name");
            dataGridView2.Columns.Add("roomNumber", "Room No.");
            dataGridView2.Columns.Add("startDate", "Start Date");
            dataGridView2.Columns.Add("endDate", "End Date");
            dataGridView2.Columns.Add("roomDailyRate", "Room Rate");

            // Load and display data into the DataGridView when the form is loaded
            loadData();

            // Load and display data into the DataGridView when the form is loaded
            LoadData();
            dataGridView2.CellClick += dataGridView2_CellClick;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
        }
        private void LoadData()
        {
            // Fetch data from "Transactions" node
            FirebaseResponse transactionsResponse = Client.Get("Transactions");

            Dictionary<string, Transaction> transactions = JsonConvert.DeserializeObject<Dictionary<string, Transaction>>(transactionsResponse.Body);

            foreach (var transaction in transactions)
            {
                if (transaction.Value.roomDetails != null)
                {
                    dataGridView1.Rows.Add(
                        transaction.Key,
                        transaction.Value.customerName,
                        transaction.Value.roomDetails.roomNumber,
                        transaction.Value.roomDetails.startDate,
                        transaction.Value.roomDetails.endDate,
                        transaction.Value.transAmount
                    );
                }
            }

            // Fetch data from "Rooms" node
            FirebaseResponse roomsResponse = Client.Get("Rooms");

            if (roomsResponse.Body != "null")
            {
                Dictionary<string, Room> rooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(roomsResponse.Body.ToString());

                // Call PopulateDataGrid to populate dataGridView2 with room data
                PopulateDataGrid(rooms);
            }

            // Refresh the DataGridView
            dataGridView1.Refresh();
            dataGridView2.Refresh();
        }
        private void loadData()
        {
            FirebaseResponse res = Client.Get("Rooms");

            if (res.Body != "null")
            {
                Console.WriteLine("Response body:");
                Console.WriteLine(res.Body);

                Dictionary<string, Room> rooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(res.Body.ToString());

            }
        }


        private void PopulateDataGrid(Dictionary<string, Room> rooms)
        {
            foreach (var room in rooms)
            {
                if (room.Value.occupancyDetails != null && room.Value.occupancyDetails.isOccupied)
                {
                    dataGridView2.Rows.Add(
                        room.Key, // Add the unique key column
                        room.Value.roomName,
                        room.Value.roomNumber,
                        room.Value.occupancyDetails.startDate,
                        room.Value.occupancyDetails.endDate,
                        room.Value.roomDailyRate,
                        string.Empty,
                        string.Empty
                    );
                }
            }
        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is not the header
            if (e.RowIndex >= 0)
            {
                // Get the roomNumber from the clicked row in dataGridView2
                string clickedRoomNumber = dataGridView2.Rows[e.RowIndex].Cells["roomNumber"].Value.ToString();

                // Find the corresponding row in dataGridView1
                DataGridViewRow matchingRow = dataGridView1.Rows
                    .Cast<DataGridViewRow>()
                    .Where(row => row.Cells["roomNumber"].Value.ToString() == clickedRoomNumber)
                    .FirstOrDefault();

                // Highlight the row in dataGridView1 if found
                if (matchingRow != null)
                {
                    matchingRow.Selected = true;
                    dataGridView1.FirstDisplayedScrollingRowIndex = matchingRow.Index;
                }
            }
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Clear the selection in dataGridView2 when a row is selected in dataGridView1
            dataGridView2.ClearSelection();
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            // Clear the selection in dataGridView1 when a row is selected in dataGridView2
            dataGridView1.ClearSelection();
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}