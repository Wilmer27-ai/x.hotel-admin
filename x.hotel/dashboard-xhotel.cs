using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;

namespace x.hotel
{
    public partial class dashboard_xhotel : Form
    {
        private IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "x0WQCfAAdZVlyYXDbbIh9W5fG51aNI8uNj6zYmMn",
            BasePath = "https://x-hotel-451cb-default-rtdb.asia-southeast1.firebasedatabase.app"
        };

        private IFirebaseClient Client;

        public dashboard_xhotel()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Addguestfinal newForm = new Addguestfinal();
            newForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ManageRooms newForm = new ManageRooms();
            newForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List_of_Guests newForm = new List_of_Guests();
            newForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Reports newForm = new Reports();
            newForm.Show();
        }
        private Timer timer;
        private void InitializeRealtimeUpdates()
        {
            try
            {
                // Initialize the Firebase client
                Client = new FireSharp.FirebaseClient(Config);

                // Set up a Timer to check for updates every 5 seconds (adjust as needed)
                timer = new Timer();
                timer.Interval = 5000; // 5 seconds
                timer.Tick += (sender, args) =>
                {
                    // Update the label with the latest count
                    UpdateCounts();
                };
                timer.Start();

                // Initial update to display the current count
                UpdateCounts();
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private void UpdateCounts()
        {
            try
            {
                // Fetch data from "Transactions" node
                FirebaseResponse transactionsResponse = Client.Get("Transactions");
                Dictionary<string, Transaction> transactions = JsonConvert.DeserializeObject<Dictionary<string, Transaction>>(transactionsResponse.Body);

                // Fetch data from "Rooms" node
                FirebaseResponse roomsResponse = Client.Get("Rooms");
                Dictionary<string, Room> rooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(roomsResponse.Body);

                // Get today's date
                DateTime today = DateTime.Today;

                // Calculate the number of occupied, vacant, and rooms checking out today
                int occupiedRoomCount = 0;
                int vacantRoomCount = 0;
                int checkoutsTodayCount = 0;
                int occupiedTransactionCount = 0;

                foreach (var room in rooms.Values)
                {
                    if (room.occupancyDetails != null && room.occupancyDetails.isOccupied)
                    {
                        DateTime startDate = DateTime.Parse(room.occupancyDetails.startDate);
                        DateTime endDate = DateTime.Parse(room.occupancyDetails.endDate);

                        // Check if today is within the entire date range of occupancy (including today)
                        if (today.Date >= startDate.Date && today.Date <= endDate.Date)
                        {
                            occupiedRoomCount++;

                            // Check if today is the endDate for the room
                            if (endDate.Date == today.Date)
                            {
                                checkoutsTodayCount++;
                            }
                        }

                        // Check if room occupancy is true
                        if (room.occupancyDetails.isOccupied)
                        {
                            occupiedTransactionCount++;
                        }
                    }
                    else
                    {
                        vacantRoomCount++;
                    }
                }

                // Update the labels with the counts on the UI thread
                Invoke((MethodInvoker)delegate
                {
                    label14.Text = occupiedRoomCount.ToString();
                    label13.Text = vacantRoomCount.ToString();
                    label12.Text = (rooms?.Count ?? 0).ToString();
                    label11.Text = checkoutsTodayCount.ToString();
                    label9.Text = occupiedTransactionCount.ToString();
                    label10.Text = occupiedRoomCount.ToString();
                });
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }








        private void dashboard_xhotel_Load(object sender, EventArgs e)
        {
            // You can call a method here to initialize the real-time updates
            InitializeRealtimeUpdates();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
