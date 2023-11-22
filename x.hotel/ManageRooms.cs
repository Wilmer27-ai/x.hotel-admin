using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;

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
            // Get the selected room classification from the combo box
            string selectedClassification = comboBox1.SelectedItem?.ToString();
            

            FirebaseResponse res = Client.Get("Rooms");

            if (res.Body != "null")
            {
                Console.WriteLine("Response body:");
                Console.WriteLine(res.Body);

                Dictionary<string, Room> rooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(res.Body.ToString());

                // Filter rooms based on selected classification
                Dictionary<string, Room> filteredRooms = FilterRoomsByClassification(rooms, selectedClassification);

                PopulateDataGrid(filteredRooms);
                Console.WriteLine("Rooms:");
                Console.WriteLine(filteredRooms.ToString());
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (managerooms1.SelectedCells.Count > 0)
            {
                int selectedRowIndex = managerooms1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = managerooms1.Rows[selectedRowIndex];
                string roomKey = selectedRow.Cells["Key"].Value.ToString();

                UpdateRoom updateRoomForm = new UpdateRoom(roomKey);
                updateRoomForm.ShowDialog();

                // Optionally, reload data after update
                LoadData();
            }
            else
            {
                MessageBox.Show("Please select a room to update.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Dictionary<string, Room> FilterRoomsByClassification(Dictionary<string, Room> rooms, string classification)
        {
            if (classification == null || classification == "All")
            {
                // If no classification selected or "All" selected, return all rooms
                return rooms;
            }

            // Filter rooms based on the selected classification
            Dictionary<string, Room> filteredRooms = new Dictionary<string, Room>();

            foreach (var room in rooms)
            {
                if (room.Value.roomClassification == classification)
                {
                    filteredRooms.Add(room.Key, room.Value);
                }
            }

            return filteredRooms;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            // Check if a room is selected
            if (managerooms1.SelectedCells.Count > 0)
            {
                int selectedRowIndex = managerooms1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = managerooms1.Rows[selectedRowIndex];
                string roomKey = selectedRow.Cells["Key"].Value.ToString();

                // Show a confirmation dialog before deleting
                DialogResult result = MessageBox.Show($"Are you sure you want to delete the room with key {roomKey}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Delete the room
                    FirebaseResponse deleteResponse = Client.Delete($"Rooms/{roomKey}");

                    if (deleteResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        MessageBox.Show("Room deleted successfully!");
                        // Optionally, reload data after delete
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Error deleting room. Please try again.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a room to delete.");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}