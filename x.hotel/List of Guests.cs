using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
            textBoxSearch.TextChanged += textBoxSearch_TextChanged;
            button1.Click -= button1_Click; // Unsubscribe to prevent multiple subscriptions
            button1.Click += button1_Click; // Subscribe to the button click event

        }
        private void LoadData()
        {
            // Fetch data from "Transactions" node
            FirebaseResponse transactionsResponse = Client.Get("Transactions");

            if (transactionsResponse.Body != null)
            {
                Console.WriteLine("Response body:");
                Console.WriteLine(transactionsResponse.Body);

                Dictionary<string, Transaction> transactions = JsonConvert.DeserializeObject<Dictionary<string, Transaction>>(transactionsResponse.Body);

                // Process transactions if there are any
                if (transactions != null)
                {
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
                }
            }
            else
            {
                // Handle the case where there are no transactions
                Console.WriteLine("No transactions found.");
            }

            // Fetch data from "Rooms" node
            FirebaseResponse res = Client.Get("Rooms");

            if (res.Body != null)
            {
                Console.WriteLine("Response body:");
                Console.WriteLine(res.Body);

                Dictionary<string, Room> rooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(res.Body.ToString());

                // Call PopulateDataGrid to populate dataGridView2 with room data
                PopulateDataGrid(rooms);
            }

            // Refresh the DataGridView
            dataGridView1.Refresh();
            dataGridView2.Refresh();
            loadData();
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
                    dataGridView2.Refresh();
                }
            }
        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is not the header and there is a selected row
            if (e.RowIndex >= 0 && dataGridView2.SelectedRows.Count > 0)
            {
                // Get the roomNumber from the selected row in dataGridView2
                string clickedRoomNumber = dataGridView2.SelectedRows[0].Cells["roomNumber"].Value?.ToString();

                if (!string.IsNullOrEmpty(clickedRoomNumber))
                {
                    // Find the corresponding row in dataGridView1
                    DataGridViewRow matchingRow = dataGridView1.Rows
                        .Cast<DataGridViewRow>()
                        .FirstOrDefault(row => row.Cells["roomNumber"].Value?.ToString() == clickedRoomNumber);

                    // Highlight the row in dataGridView1 if found
                    if (matchingRow != null)
                    {
                        // Check if the matching row is visible before setting FirstDisplayedScrollingRowIndex
                        if (matchingRow.Visible)
                        {
                            matchingRow.Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = matchingRow.Index;
                        }
                        else
                        {
                            // Handle the case where the matching row is invisible
                            // You may want to handle this case differently based on your requirements
                        }
                    }
                }
            }
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Check if there is a selected row in dataGridView1
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the roomNumber from the selected row in dataGridView1
                string clickedRoomNumber = dataGridView1.SelectedRows[0].Cells["roomNumber"].Value?.ToString();

                if (!string.IsNullOrEmpty(clickedRoomNumber))
                {
                    // Clear the selection in dataGridView2
                    dataGridView2.ClearSelection();

                    // Find the corresponding row in dataGridView2
                    DataGridViewRow matchingRow = dataGridView2.Rows
                        .Cast<DataGridViewRow>()
                        .FirstOrDefault(row => row.Cells["roomNumber"].Value?.ToString() == clickedRoomNumber);

                    // Highlight the entire row in dataGridView2 if found
                    if (matchingRow != null)
                    {
                        matchingRow.Selected = true;
                        dataGridView2.FirstDisplayedScrollingRowIndex = matchingRow.Index;
                    }
                }
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            // Get the search term from the TextBox
            string searchTerm = textBoxSearch.Text.ToLower();

            // Loop through each row in dataGridView1
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Check if the row is the new row and in edit mode
                if (!row.IsNewRow)
                {
                    // Get the customerName from the current row
                    object customerNameCellValue = row.Cells["customerName"].Value;

                    // Check if the customerName cell value is not null
                    if (customerNameCellValue != null)
                    {
                        // Convert the cell value to string and make it lowercase
                        string customerName = customerNameCellValue.ToString().ToLower();

                        // Check if the customerName contains the search term
                        bool match = customerName.Contains(searchTerm);

                        // Show or hide the row based on the search result
                        row.Visible = match;
                    }
                    else
                    {
                        // If the cell value is null, hide the row
                        row.Visible = false;
                    }
                }
            }
        }
        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            // Clear the selection in dataGridView1 when a row is selected in dataGridView2
            dataGridView1.ClearSelection();
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            // Check if there is a selected row in dataGridView2
            if (dataGridView2.SelectedRows.Count > 0)
            {
                // Get the roomKey from the selected row in dataGridView2
                string roomKey = dataGridView2.SelectedRows[0].Cells["Key"].Value?.ToString();

                if (!string.IsNullOrEmpty(roomKey))
                {
                    // Update the occupancy details using the unique key (roomKey)
                    string updateOccupancyUrl = $"{Config.BasePath}/Rooms/{roomKey}/occupancyDetails.json?auth={Config.AuthSecret}";
                    var occupancyPatchData = new
                    {
                        startDate = "",
                        endDate = "",
                        isOccupied = false,
                        transId = ""
                    };

                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage occupancyResponse = await client.PatchAsJsonAsync(updateOccupancyUrl, occupancyPatchData);

                        if (occupancyResponse.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"Room {roomKey} Checked out");
                        }
                        else
                        {
                            Console.WriteLine($"Error updating room occupancy: {occupancyResponse.StatusCode} - {occupancyResponse.ReasonPhrase}");
                        }
                    }
                }
            }
        }

        private Room GetRoomByNumber(string roomNumber)
        {
            FirebaseResponse roomsResponse = Client.Get("Rooms");

            if (roomsResponse.Body != "null")
            {
                Dictionary<string, Room> rooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(roomsResponse.Body.ToString());

                foreach (var room in rooms)
                {
                    if (room.Value.roomNumber.ToString() == roomNumber)
                    {
                        return room.Value;
                    }
                }
            }
            return null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Clear existing rows in both DataGridViews
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            // Load and display data into the DataGridViews
            LoadData();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // Check if there is a selected row in dataGridView1
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the transaction ID from the selected row in dataGridView1
                string transactionId = dataGridView1.SelectedRows[0].Cells["transId"].Value?.ToString();

                if (!string.IsNullOrEmpty(transactionId))
                {
                    // Ask for confirmation before deleting
                    DialogResult result = MessageBox.Show($"Are you sure you want to delete transaction with ID {transactionId}?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // User confirmed, proceed with deletion
                        string deleteTransactionUrl = $"{Config.BasePath}/Transactions/{transactionId}.json?auth={Config.AuthSecret}";

                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage deleteResponse = client.DeleteAsync(deleteTransactionUrl).Result;

                            if (deleteResponse.IsSuccessStatusCode)
                            {
                                MessageBox.Show($"Transaction with ID {transactionId} deleted successfully.");
                            }
                            else
                            {
                                Console.WriteLine($"Error deleting transaction: {deleteResponse.StatusCode} - {deleteResponse.ReasonPhrase}");
                            }
                        }
                    }
                }
            }
        }
    }
}
