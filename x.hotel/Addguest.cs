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
    public partial class Addguest : Form
    {
        public IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "x0WQCfAAdZVlyYXDbbIh9W5fG51aNI8uNj6zYmMn",
            BasePath = "https://x-hotel-451cb-default-rtdb.asia-southeast1.firebasedatabase.app"
        };
        IFirebaseClient Client;

        public Addguest()
        {
            InitializeComponent();
        }

        private void Addguest_Load(object sender, EventArgs e)
        {
            Client = new FireSharp.FirebaseClient(Config);

            Rooms1.AutoGenerateColumns = false;
            Rooms1.Columns.Add("roomClassification", "Room Type");
            Rooms1.Columns.Add("roomNumber", "Room No.");
            Rooms1.Columns.Add("roomCapacity", "Room Capacity");
            Rooms1.Columns.Add("bedCount", "Room Bed");
            Rooms1.Columns.Add("roomDailyRate", "Room Rate");
            Rooms1.Columns.Add("roomDailyRate", "SubTotal");

            // Load data into DataGridView when the form is loaded
            Rooms1.CellClick += Rooms1_CellClick;
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
                Rooms1.Rows.Clear();
            }
        }

        private void PopulateDataGrid(Dictionary<string, Room> rooms)
        {
            Rooms1.Rows.Clear();

            foreach (var room in rooms)
            {
                Rooms1.Rows.Add(
                    room.Value.roomClassification,
                    room.Value.roomNumber,
                    room.Value.roomCapacity,
                    room.Value.bedCount,
                    room.Value.roomDailyRate,
                    room.Value.roomDailyRate
                );
            }
        }

        private void Rooms1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell click event if needed
        }
    }
}