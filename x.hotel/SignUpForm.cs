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
    public partial class SignUpForm : Form
    {
        private readonly IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "XX9AMWcYQ0S2FKOGWT0DWAzKwehEhkYew84T91lg",
            BasePath = "https://crud-daaa1-default-rtdb.firebaseio.com"
        };

        private IFirebaseClient firebaseClient;

        public SignUpForm()
        {
            InitializeComponent();
        }

        private void SignUpForm_Load(object sender, EventArgs e)
        {
            firebaseClient = new FireSharp.FirebaseClient(config);

            if (firebaseClient == null)
            {
                MessageBox.Show("Failed to initialize Firebase client.");
                // Handle the situation where Firebase initialization fails.
                // For now, you can return or throw an exception to stop further execution.
                return;
            }
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            await SignUp();
        }

        private async Task SignUp()
        {
            var adminId = textBox1.Text;
            var username = textBox2.Text;
            var password = textBox3.Text;

            var admin = new AdminInfo
            {
                AdminId = adminId,
                Username = username,
                Password = password
            };

            try
            {
                FirebaseResponse response = await Task.Run(() => firebaseClient.Set($"Admins/{adminId}", admin));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Signup successful!");

                    // Add this line for debugging
                    Console.WriteLine($"Signup successful. AdminId: {adminId}, Username: {username}, Password: {password}");
                }
                else
                {
                    MessageBox.Show("Error occurred while signing up. Please try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");

                // Add this line for debugging
                Console.WriteLine($"Error during signup: {ex}");
            }
        }
    }
}





