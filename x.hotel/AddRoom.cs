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
    public partial class AddRoom : Form
    {
        private IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "x0WQCfAAdZVlyYXDbbIh9W5fG51aNI8uNj6zYmMn",
            BasePath = "https://x-hotel-451cb-default-rtdb.asia-southeast1.firebasedatabase.app"
        };

       // private IFirebaseClient Client;
        public AddRoom()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Create a new Room object and populate its properties from the textboxes
            Room newRoom = new Room
            {
                bedCount = Convert.ToInt32(textBox2.Text),
                occupancyDetails = new occupancyDetails
                {
                    endDate = "",
                    isOccupied = false,
                    startDate = "",
                    transId = ""
                },
                roomCapacity = Convert.ToInt32(textBox3.Text),
                roomClassification = comboBox5.Text,
                roomDailyRate = Convert.ToInt32(textBox5.Text),
                roomDescription = textBox6.Text,
                roomFeatures = new RoomFeatures
                {
                    hasCityView = comboBox1.Text.ToLower() == "yes",
                    hasPrivatePool = comboBox2.Text.ToLower() == "yes",
                    hasShower = comboBox3.Text.ToLower() == "yes",
                    isPetFriendly = comboBox4.Text.ToLower() == "yes",
                },
                roomHourlyRate = string.IsNullOrEmpty(textBox5.Text) ? (int?)null : Convert.ToInt32(textBox5.Text),
                roomImages = new RoomImages
                {
                    img1 = textBox7.Text,
                    img2 = textBox8.Text,
                    img3 = textBox9.Text
                },
                roomName = textBox10.Text,
                roomNumber = Convert.ToInt32(textBox11.Text)
            };

            // Save the new room to the Firebase database
            FirebaseResponse response = await new FireSharp.FirebaseClient(Config).Set($"Rooms/{Guid.NewGuid()}", newRoom);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("Room added successfully!");
                ClearTextboxes();
            }
            else
            {
                MessageBox.Show("Error adding room. Please try again.");
            }
        }
        private void ClearTextboxes()
        {
            // Iterate through all controls in the form
            foreach (Control control in Controls)
            {
                // Check if the control is a TextBox
                if (control is TextBox textBox)
                {
                    // Clear the Text property of the TextBox
                    textBox.Clear();
                }
            }
        }
    }
    }

