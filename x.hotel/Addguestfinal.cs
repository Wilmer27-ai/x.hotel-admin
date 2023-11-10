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
            // This is the method where you initialize the Firebase client and load data
            Client = new FireSharp.FirebaseClient(Config);
            RoomsdataGrid.AutoGenerateColumns = false;
            RoomsdataGrid.Columns.Add("roomName", "Room Name");
            RoomsdataGrid.Columns.Add("roomClassification", "Room Type");
            RoomsdataGrid.Columns.Add("roomNumber", "Room No.");
            RoomsdataGrid.Columns.Add("roomCapacity", "Room Capacity");
            RoomsdataGrid.Columns.Add("bedCount", "Room Bed");
            RoomsdataGrid.Columns.Add("roomDailyRate", "Room Rate");
            // Initialize the DataGridView columns only once
            // Load and display data into the DataGridView when the form is loaded
            LoadData();
        }

        private void LoadData()
        {
            FirebaseResponse res = Client.Get("Rooms"); // Assuming "Rooms" is the key for your rooms data

            // Check if the response body is not null
            if (res.Body != "null")
            {
                Dictionary<string, Room> rooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(res.Body.ToString());
                PopulateDataGrid(rooms);
            }
            else
            {
                // If there is no data, clear the DataGridView
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
                    string.Empty, // Start Date column (empty for now)
                    string.Empty  // End Date column (empty for now)
                );
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
