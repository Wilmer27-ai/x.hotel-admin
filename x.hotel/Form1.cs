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
                var username = username1.Text;
                var password = password2.Text;

                if (firebaseClient == null)
                {
                    MessageBox.Show("Firebase client is not initialized.");
                    return;
                }

                FirebaseResponse response = await Task.Run(() => firebaseClient.Get($"Admins/{username}"));

                Console.WriteLine($"Username: {username}");
                Console.WriteLine($"Response Body: {response.Body}");

                if (response.Body == "null")
                {
                    MessageBox.Show("Admin not found.");
                    return;
                }

                AdminInfo admin = response.ResultAs<AdminInfo>();

                // Debugging statements
                Console.WriteLine($"Entered Password: {password}");
                Console.WriteLine($"Stored Password: {admin?.Password}");

                if (admin != null && admin.Password != null && admin.Password.Trim() == password.Trim())
                {
                    MessageBox.Show("Admin login successful!");

                    // Debugging statement
                    Console.WriteLine("Admin login successful!");

                    dashboard_xhotel newForm = new dashboard_xhotel();
                    newForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Admin login failed. Password mismatch or admin not found.");

                    // Debugging statement
                    Console.WriteLine("Admin login failed. Password mismatch or admin not found.");
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



