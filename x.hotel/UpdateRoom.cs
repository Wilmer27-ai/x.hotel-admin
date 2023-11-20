using System;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace x.hotel
{
    public partial class UpdateRoom : Form
    {
        private IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "x0WQCfAAdZVlyYXDbbIh9W5fG51aNI8uNj6zYmMn",
            BasePath = "https://x-hotel-451cb-default-rtdb.asia-southeast1.firebasedatabase.app"
        };

        private IFirebaseClient Client;
        private string roomKey;

        public UpdateRoom(string roomKey)
        {
            InitializeComponent();
            this.roomKey = roomKey;
            LoadRoomDetails();
        }

        private void LoadRoomDetails()
        {
            // Initialize the Firebase client
            Client = new FireSharp.FirebaseClient(Config);

            // Load room details based on the room key
            FirebaseResponse res = Client.Get($"Rooms/{roomKey}");

            if (res.Body != "null")
            {
                Room room = res.ResultAs<Room>();

                // Load room details into textboxes and comboboxes
                textBox1.Text = roomKey;
                textBox2.Text = room.bedCount.ToString();
                textBox3.Text = room.roomCapacity.ToString();
                comboBox5.Text = room.roomClassification;
                textBox5.Text = room.roomDailyRate.ToString();
                textBox6.Text = room.roomDescription;
                comboBox1.Text = room.roomFeatures.hasCityView ? "Yes" : "No";
                comboBox2.Text = room.roomFeatures.hasPrivatePool ? "Yes" : "No";
                comboBox3.Text = room.roomFeatures.hasShower ? "Yes" : "No";
                comboBox4.Text = room.roomFeatures.isPetFriendly ? "Yes" : "No";
                textBox7.Text = room.roomImages.img1;
                textBox8.Text = room.roomImages.img2;
                textBox9.Text = room.roomImages.img3;
                textBox10.Text = room.roomName;
                textBox11.Text = room.roomNumber.ToString();
                // Load occupancy details
                textBox12.Text = room.occupancyDetails.endDate;
                comboBox6.Text = room.occupancyDetails.isOccupied ? "Yes" : "No";
                textBox13.Text = room.occupancyDetails.startDate;
                textBox4.Text = room.occupancyDetails.transId;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Update room details
            Room updatedRoom = new Room
            {
                bedCount = Convert.ToInt32(textBox2.Text),
                occupancyDetails = new occupancyDetails
                {
                    endDate = textBox12.Text,
                    isOccupied = comboBox6.Text.ToLower() == "yes",
                    startDate = textBox13.Text,
                    transId = textBox4.Text
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
                roomImages = new RoomImages
                {
                    img1 = textBox7.Text,
                    img2 = textBox8.Text,
                    img3 = textBox9.Text
                },
                roomName = textBox10.Text,
                roomNumber = Convert.ToInt32(textBox11.Text)
            };

            FirebaseResponse response = Client.Update($"Rooms/{roomKey}", updatedRoom);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("Room updated successfully!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Error updating room. Please try again.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
