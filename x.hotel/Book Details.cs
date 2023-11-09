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



        // Constructor with two arguments
        public Book_Details(Room selectedRoom)
        {
            InitializeComponent();
            this.selectedRoom = selectedRoom;
            InitializeFirebase();
            PopulateNextFormRooms();
        }

        private string GenerateGuestId()
        {
            // Implement your logic to generate a unique guest ID here
            // For example, you can concatenate the current date and time or use a GUID
            return Guid.NewGuid().ToString();
        }
        private string GenerateTransactionId()
        {
            // Implement your logic to generate a unique guest ID here
            // For example, you can concatenate the current date and time or use a GUID
            return Guid.NewGuid().ToString();
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

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            // Get guest details from the user input
            string guestId = GenerateGuestId(); // You need to implement GenerateGuestId() method
            string firstName = textBox2.Text;
            string lastName = textBox3.Text;
            int.TryParse(textBox4.Text, out int age);

            string contactNumber = textBox5.Text;
            string address = textBox6.Text;
            string sex = checkBox1.Checked ? "Male" : (checkBox2.Checked ? "Female" : "");
            // Get the start and end dates from DateTimePickers
            string startDate = dateTimePicker1.Value.ToString("MM/dd/yyyy");
            string endDate = dateTimePicker2.Value.ToString("MM/dd/yyyy");
            // Create a Guest object
            Guest guest = new Guest
            {
                GuestId = guestId,
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                ContactNumber = contactNumber,
                Address = address,
                Sex = sex
            };
            OccupancyDetails occupancyDetails = new OccupancyDetails
            {
                startDate = startDate,
                endDate = endDate,
                isOccupied = true, // Set to true since the room is booked
                transId = GenerateTransactionId() // You need to implement GenerateTransactionId() method
            };

            // Use UpdateAsync to update specific fields in the occupancyDetails
            FirebaseResponse updateResponse = await Client.UpdateAsync($"Rooms/{selectedRoom.roomNumber}", new { occupancyDetails });

            Confirm_Payment confirmPaymentForm = new Confirm_Payment(selectedRoom, guest);
            confirmPaymentForm.Show();
        }
    }
}
