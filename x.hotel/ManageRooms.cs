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
    public partial class ManageRooms : Form
    {
        private IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "x0WQCfAAdZVlyYXDbbIh9W5fG51aNI8uNj6zYmMn",
            BasePath = "https://x-hotel-451cb-default-rtdb.asia-southeast1.firebasedatabase.app"
        };

        private IFirebaseClient Client;
        public ManageRooms()
        {
            InitializeComponent();
        }

        private void ManageRooms_Load(object sender, EventArgs e)
        {
            // Initialize the Firebase client and load data
            Client = new FireSharp.FirebaseClient(Config);
            managerooms1.AutoGenerateColumns = false;
            managerooms1.Columns.Add("Key", "Room Key");
            managerooms1.Columns.Add("roomName", "Room Name");
            managerooms1.Columns.Add("roomClassification", "Room Type");
            managerooms1.Columns.Add("roomNumber", "Room No.");
            managerooms1.Columns.Add("roomCapacity", "Room Capacity");
            managerooms1.Columns.Add("bedCount", "Room Bed");
            managerooms1.Columns.Add("roomDailyRate", "Room Rate");

            // Load and display data into the DataGridView when the form is loaded
            LoadData();
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
                managerooms1.Rows.Clear();
            }
        }
        private void PopulateDataGrid(Dictionary<string, Room> rooms)
        {
            managerooms1.Rows.Clear();

            foreach (var room in rooms)
            {
                    managerooms1.Rows.Add(
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

        private void button1_Click(object sender, EventArgs e)
        {
            AddRoom newForm = new AddRoom();
            // Show the new form
            newForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
    }

