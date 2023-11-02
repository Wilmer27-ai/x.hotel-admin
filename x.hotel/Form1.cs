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
    public partial class Admin : Form
    {
        public IFirebaseConfig firebaseConfig = new FirebaseConfig
        {
            AuthSecret = "XX9AMWcYQ0S2FKOGWT0DWAzKwehEhkYew84T91lg",
            BasePath = "https://crud-daaa1-default-rtdb.firebaseio.com"
        };

        public IFirebaseClient firebaseClient;

        public Admin()
        {
            InitializeComponent();
            firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);

            if (firebaseClient == null)
            {
                MessageBox.Show("Failed to initialize Firebase client.");
                // Handle the situation where Firebase initialization fails.
                // For now, you can return or throw an exception to stop further execution.
                return;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            SignUpForm newForm = new SignUpForm();
            newForm.Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SignIn();
        }

        private async void SignIn()
        {
            try
            {
                var Username = textBox1.Text;
                var Password = textBox2.Text;

                if (firebaseClient == null)
                {
                    MessageBox.Show("Firebase client is not initialized.");
                    return;
                }

                FirebaseResponse response = await Task.Run(() => firebaseClient.Get($"Admins/{Username}"));
                var User = response.ResultAs<AdminInfo>();

                // Debugging statements
                Console.WriteLine($"Entered Username: {Username}");
                Console.WriteLine($"Entered Password: {Password}");
                Console.WriteLine($"Stored Password: {User?.Password}");

                if (User != null && User.Password != null && User.Password.Trim() == Password.Trim())
                {
                    MessageBox.Show("Login successful!");

                    // Debugging statement
                    Console.WriteLine("Login successful!");

                    dashboard_xhotel newForm = new dashboard_xhotel();
                    newForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Login failed. Please check your credentials.");

                    // Debugging statement
                    Console.WriteLine("Login failed. Password mismatch or user not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                MessageBox.Show($"An error occurred: {ex.Message}\n\n{ex.StackTrace}");
            }
        }
    }
}