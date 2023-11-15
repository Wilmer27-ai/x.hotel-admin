using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
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
                    room.Value.roomName,
                    room.Value.roomClassification,
                    room.Value.roomNumber,
                    room.Value.roomCapacity,
                    room.Value.bedCount,
                    room.Value.roomDailyRate,
                    room.Value.roomDailyRate,
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
                customerName = customerName,
                customerPhoneNumber = customerPhoneNumber,
                guestCount = guestCount,
                paymentMethod = "WalkIn",
                roomDetails = new
                {
                    startDate = startDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    roomNumber = roomNumber,
                    endDate = endDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                },
                transAmount = totalAmount,
                transDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                transId = transactionId
            };

            try
            {
                // Save the transaction
                FirebaseResponse response = Client.Set($"Transactions/{transactionId}", transaction);

                // Check if the room with the given roomNumber already exists
                FirebaseResponse roomResponse = Client.Get($"Rooms/{roomNumber}");
                if (roomResponse.Body != "null")
                {
                    // Room exists, update the occupancy details
                    var existingRoom = roomResponse.ResultAs<Room>();
                    existingRoom.occupancyDetails = new occupancyDetails
                    {
                        startDate = startDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        endDate = endDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        isOccupied = true,
                        transId = transactionId
                    };

                    // Update the room with the modified occupancy details
                    FirebaseResponse updateResponse = Client.Update($"Rooms/{roomNumber}", existingRoom);

                    // Check the response if needed
                }
                else
                {
                    Console.WriteLine($"Room with roomNumber {roomNumber} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transaction: {ex.Message}");
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            // Get the selected row from the DataGridView
            if (RoomsdataGrid.SelectedRows.Count > 0)
            {
                // Assuming guestCount and roomNumber are columns in the DataGridView
                int guestCount = (int)RoomsdataGrid.SelectedRows[0].Cells["roomCapacity"].Value;
                int roomNumber = (int)RoomsdataGrid.SelectedRows[0].Cells["roomNumber"].Value;
                int totalAmount = (int)RoomsdataGrid.SelectedRows[0].Cells["roomDailyRate"].Value;
                // You can add additional logic to get other values if needed
                DateTime startDate = dateTimePicker1.Value;
                DateTime endDate = dateTimePicker2.Value;

                // Get other values from textboxes, datepickers, etc.
                string customerName = textBox2.Text;
                string customerPhoneNumber = textBox5.Text;

                // Call SaveTransaction with the obtained values
                SaveTransaction(customerName, customerPhoneNumber, guestCount, roomNumber, startDate, endDate, totalAmount);
            }
            else
            {
                // Display a message to inform the user to select a row in the DataGridView
                MessageBox.Show("Please select a room from the DataGridView.");
            }
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
