using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace x.hotel
{
    public partial class AddRoom : Form
    {
        private IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "x0WQCfAAdZVlyYXDbbIh9W5fG51aNI8uNj6zYmMn",
            BasePath = "https://x-hotel-451cb-default-rtdb.asia-southeast1.firebasedatabase.app"
        };

        private string randomId;
        private int roomNumber;

        public AddRoom()
        {
            InitializeComponent();
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {// Create a new Room object and populate its properties from the textboxes
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
                    roomNumber = roomNumber
                };

                // Save the new room to the Firebase database using the stored randomId
                FirebaseResponse response = new FireSharp.FirebaseClient(Config).Set($"Rooms/{randomId}", newRoom);

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
        }
        private void GenerateAndDisplayRandomId()
        {
            randomId = GenerateRandomId();
            textBox1.Text = randomId;
        }

        private void GenerateAndDisplayRoomNumber()
        {
            roomNumber = GenerateUniqueRoomNumber();
            textBox11.Text = roomNumber.ToString();
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

        private void AddRoom_Load(object sender, EventArgs e)
        {
            // Load existing room numbers for validation
            List<int> existingRoomNumbers = GetExistingRoomNumbers("");

            // Generate and display randomId and roomNumber when the form loads
            GenerateAndDisplayRandomId();
            GenerateAndDisplayRoomNumber();
        }

        private int GenerateUniqueRoomNumber()
        {
            // Retrieve existing room numbers from Firebase using the provided randomId
            List<int> existingRoomNumbers = GetExistingRoomNumbers(randomId);

            // Generate a random room number between 1 and 1000
            Random random = new Random();
            int number;
            do
            {
                number = random.Next(1, 1001);
            } while (existingRoomNumbers.Contains(number));

            return number;
        }

        private List<int> GetExistingRoomNumbers(string randomId)
        {
            // Fetch existing room numbers from Firebase using the provided randomId
            // Modify this logic based on how you retrieve room numbers from Firebase
            // For example, if your Room class has a property called roomNumber, you can query Firebase to get a list of room numbers using the randomId.

            // Placeholder logic (modify based on your actual implementation)
            List<int> existingRoomNumbers = new List<int>();

            // Replace this with your logic to retrieve existing room numbers from Firebase using the randomId
            // For example, if you have a list of Room objects, you can extract room numbers like this:
            // existingRoomNumbers = listOfRooms.Where(room => room.RandomId == randomId).Select(room => room.roomNumber).ToList();

            return existingRoomNumbers;
        }

        private bool ValidateInputs()
        {
            // Validate that all textboxes have data
            foreach (Control control in Controls)
            {
                if (control is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
                {
                    MessageBox.Show("Please input all the details of the room");
                    return false; // At least one textbox is empty
                }
            }

            return true; // All textboxes have data
        }
        private string GenerateRandomId()
        {
            return Guid.NewGuid().ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
