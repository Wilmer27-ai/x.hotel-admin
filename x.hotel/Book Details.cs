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

namespace x.hotel
{
    public partial class Book_Details : Form
    {
        private Room selectedRoom;
        private IFirebaseConfig Config;
        private IFirebaseClient Client;

        public Book_Details(Room selectedRoom)
        {
            InitializeComponent();
            this.selectedRoom = selectedRoom;
            InitializeFirebase();
            PopulateNextFormRooms();
        }

        private void InitializeFirebase()
        {
            Config = new FirebaseConfig
            {
                AuthSecret = "x0WQCfAAdZVlyYXDbbIh9W5fG51aNI8uNj6zYmMn",
                BasePath = "https://x-hotel-451cb-default-rtdb.asia-southeast1.firebasedatabase.app"
            };

            Client = new FireSharp.FirebaseClient(Config);
        }

        private async void PopulateNextFormRooms()
        {
            Rooms2.AutoGenerateColumns = false;
            Rooms2.Columns.Add("roomName", "Room Name");
            Rooms2.Columns.Add("roomClassification", "Room Type");
            Rooms2.Columns.Add("roomNumber", "Room No.");
            Rooms2.Columns.Add("roomCapacity", "Room Capacity");
            Rooms2.Columns.Add("bedCount", "Room Bed");
            Rooms2.Columns.Add("roomDailyRate", "Room Rate");
            Rooms2.Columns.Add("roomDailyRate", "Subtotal");

            // Populate the Rooms2 DataGridView with the selected room data
            Rooms2.Rows.Add(
                selectedRoom.roomName,
                selectedRoom.roomClassification,
                selectedRoom.roomNumber,
                selectedRoom.roomCapacity,
                selectedRoom.bedCount,
                selectedRoom.roomDailyRate,
                selectedRoom.roomDailyRate
            );

            // You can perform additional Firebase operations if needed
            // For example, you might want to store the selectedRoom data in Firebase
            // Uncomment the following lines to store the data

            // var response = await Client.SetAsync("BookedRooms/" + selectedRoom.roomNumber, selectedRoom);
            // MessageBox.Show("Room Booked!");
        }
    }
}