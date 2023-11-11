using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using x.hotel; // Add the namespace for your class

namespace x.hotel
{
    public partial class Addguestfinal : Form
    {
        private IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "x0WQCfAAdZVlyYXDbbIh9W5fG51aNI8uNj6zYmMn",
            BasePath = "https://x-hotel-451cb-default-rtdb.asia-southeast1.firebasedatabase.app"
        };

        private IFirebaseClient Client;

        public Addguestfinal()
        {
            InitializeComponent();
        }

        private void Addguestfinal_Load(object sender, EventArgs e)
        {
            // Initialize the Firebase client and load data
            Client = new FireSharp.FirebaseClient(Config);
            RoomsdataGrid.AutoGenerateColumns = false;
            RoomsdataGrid.Columns.Add("roomName", "Room Name");
            RoomsdataGrid.Columns.Add("roomClassification", "Room Type");
            RoomsdataGrid.Columns.Add("roomNumber", "Room No.");
            RoomsdataGrid.Columns.Add("roomCapacity", "Room Capacity");
            RoomsdataGrid.Columns.Add("bedCount", "Room Bed");
            RoomsdataGrid.Columns.Add("roomDailyRate", "Room Rate");

            // Load and display data into the DataGridView when the form is loaded
            LoadData();

            // Generate and display transaction ID during form load
            GenerateAndDisplayTransactionId();

            // Attach the cell click event handler
            RoomsdataGrid.CellClick += RoomsdataGrid_CellContentClick;
        }

        private void LoadData()
        {
            FirebaseResponse res = Client.Get("Rooms");

            if (res.Body != "null")
            {
                Dictionary<string, Room> rooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(res.Body.ToString());
                PopulateDataGrid(rooms);
            }
            else
            {
                RoomsdataGrid.Rows.Clear();
            }
        }

        private void PopulateDataGrid(Dictionary<string, Room> rooms)
        {
            RoomsdataGrid.Rows.Clear();

            foreach (var room in rooms)
            {
                RoomsdataGrid.Rows.Add(
                    room.Value.RoomName,
                    room.Value.RoomClassification,
                    room.Value.RoomNumber,
                    room.Value.RoomCapacity,
                    room.Value.BedCount,
                    room.Value.RoomDailyRate,
                    room.Value.RoomDailyRate,
                    string.Empty,
                    string.Empty
                );
            }
        }

        private string GenerateTransactionId()
        {
            return Guid.NewGuid().ToString();
        }

        private void GenerateAndDisplayTransactionId()
        {
            string transactionId = GenerateTransactionId();
            textBox1.Text = transactionId;
        }

        private void SaveTransaction(string customerName, string customerPhoneNumber, int guestCount, int roomNumber, DateTime startDate, DateTime endDate, int totalAmount)
        {
            string transactionId = GenerateTransactionId();

            var transaction = new
            {
                CustomerName = customerName,
                CustomerPhoneNumber = customerPhoneNumber,
                GuestCount = guestCount,
                PaymentMethod = "Walk In",
                RoomDetails = new
                {
                    StartDate = startDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    RoomNumber = roomNumber,
                    EndDate = endDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                },
                TransAmount = totalAmount,
                TransDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                TransId = transactionId
            };

            try
            {
                FirebaseResponse response = Client.Set($"Transactions/{transactionId}", transaction);
                // Check the response if needed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transaction: {ex.Message}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Add your logic to handle the button click event
            // For example, get values from textboxes, datepickers, etc.
            string customerName = "Sample Name";
            string customerPhoneNumber = "1234567890";
            int guestCount = 2;
            int roomNumber = 101;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddDays(2);
            int totalAmount = 200;

            // Call SaveTransaction with the obtained values
            SaveTransaction(customerName, customerPhoneNumber, guestCount, roomNumber, startDate, endDate, totalAmount);
        }

        private void RoomsdataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 2)
            {
                if (RoomsdataGrid.SelectedRows.Count > 0)
                {
                    int selectedRoomNumber = (int)RoomsdataGrid.SelectedRows[0].Cells["roomNumber"].Value;
                    // Use the selected room number as needed
                    // You can save it to the roomDetails or perform other actions
                }
            }
        }
    }
}
