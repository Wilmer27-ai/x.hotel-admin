using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

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
            RoomsdataGrid.Columns.Add("Key", "Room Key");
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
                Console.WriteLine("Response body:");
                Console.WriteLine(res.Body);

                Dictionary<string, Room> rooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(res.Body.ToString());
                PopulateDataGrid(rooms);
                Console.WriteLine("Rooms:");
                Console.WriteLine(rooms.ToString());
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
                // Check if the room is not occupied before adding it to the DataGridView
                if (!room.Value.occupancyDetails.isOccupied)
                {
                    RoomsdataGrid.Rows.Add(
                        room.Key, // Add the unique key column
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

        private async void SaveTransaction(string customerName, string customerPhoneNumber, int guestCount, string roomKey, DateTime startDate, DateTime endDate, int totalAmount, string Key)
        {
            string transactionId = GenerateTransactionId();

            try
            {
                // Calculate the number of days between start date and end date
                int numberOfDays = (int)(endDate - startDate).TotalDays;

                // Calculate the total amount based on the daily rate and number of days
                int calculatedAmount = numberOfDays * (int)RoomsdataGrid.SelectedRows[0].Cells["roomDailyRate"].Value;

                // Save the transaction
                FirebaseResponse response = Client.Set($"Transactions/{transactionId}", new
                {
                    customerName = customerName,
                    customerPhoneNumber = customerPhoneNumber,
                    guestCount = guestCount,
                    paymentMethod = "WalkIn",
                    roomDetails = new
                    {
                        startDate = startDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        roomNumber = roomKey, // Use roomKey instead of roomNumber
                        endDate = endDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                    },
                    transAmount = calculatedAmount, // Use the calculated amount
                    transDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    transId = transactionId
                });

                // Update the room occupancy details using the unique key (roomNumber)
                string updateOccupancyUrl = $"{Config.BasePath}/Rooms/{Key}/occupancyDetails.json?auth={Config.AuthSecret}";
                var occupancyPatchData = new
                {
                    startDate = startDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    endDate = endDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    isOccupied = true,
                    transId = transactionId
                };

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage occupancyResponse = await client.PatchAsJsonAsync(updateOccupancyUrl, occupancyPatchData);

                    if (occupancyResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Room occupancy for {roomKey} updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Error updating room occupancy: {occupancyResponse.StatusCode} - {occupancyResponse.ReasonPhrase}");
                    }
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
        // Assuming guestCount and roomKey are columns in the DataGridView
        int guestCount = (int)RoomsdataGrid.SelectedRows[0].Cells["roomCapacity"].Value;
        string roomKey = RoomsdataGrid.SelectedRows[0].Cells["roomNumber"].Value.ToString(); // Use roomKey instead of roomNumber
        int totalAmount = (int)RoomsdataGrid.SelectedRows[0].Cells["roomDailyRate"].Value;
        string Key = RoomsdataGrid.SelectedRows[0].Cells["Key"].Value.ToString();
        // You can add additional logic to get other values if needed
        DateTime startDate = dateTimePicker1.Value;
        DateTime endDate = dateTimePicker2.Value;

        // Get other values from textboxes, datepickers, etc.
        string customerName = textBox2.Text;
        string customerPhoneNumber = textBox5.Text;

        // Calculate the number of days between start date and end date
        int numberOfDays = (int)(endDate - startDate).TotalDays;

        // Calculate the total amount based on the daily rate and number of days
        int calculatedAmount = numberOfDays * (int)RoomsdataGrid.SelectedRows[0].Cells["roomDailyRate"].Value;

                // Show the confirmation form with transaction details
              confirmationfinal confirmationForm = new confirmationfinal(customerName, customerPhoneNumber, calculatedAmount, GenerateTransactionId());
              DialogResult result = confirmationForm.ShowDialog();

                // Check if the user confirmed the booking in the confirmation form
                if (result == DialogResult.OK)
        {
            // Call SaveTransaction with the obtained values
            SaveTransaction(customerName, customerPhoneNumber, guestCount, roomKey, startDate, endDate, calculatedAmount, Key);
        }
        else
        {
            // User canceled the confirmation, you can handle it accordingly
            MessageBox.Show("Booking not confirmed.");
        }
    }
    else
    {
        // Display a message to inform the user to select a room from the DataGridView
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
                    string selectedRoomKey = RoomsdataGrid.SelectedRows[0].Cells["Key"].Value.ToString();
                    // Use the selected room number as needed
                    // You can save it to the roomDetails or perform other actions
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
