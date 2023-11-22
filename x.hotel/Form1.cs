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
    public partial class LoginForm : Form
    {
        public IFirebaseConfig firebaseConfig = new FirebaseConfig
        {
            AuthSecret = "x0WQCfAAdZVlyYXDbbIh9W5fG51aNI8uNj6zYmMn",
            BasePath = "https://x-hotel-451cb-default-rtdb.asia-southeast1.firebasedatabase.app"
        };

        public IFirebaseClient firebaseClient;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
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

        private async void Button1_ClickAsync(object sender, EventArgs e)

        {
            await Login();
        }

        private async Task Login()
        {
            var username = username1.Text;
            var password = password2.Text;

            try
            {
                FirebaseResponse response = await Task.Run(() => firebaseClient.Get($"Admins/{username}"));

                if (response.Body != "null")
                {
                    AdminInfo admin = response.ResultAs<AdminInfo>();

                    if (admin.Password == password)
                    {
                        MessageBox.Show("Login successful!");
                        // Open the new form or perform any other action upon successful login
                        // For example, you can open a new form like this:
                        dashboard_xhotel newForm = new dashboard_xhotel();
                        // Show the new form
                        newForm.Show();

                    }
                    else
                    {
                        MessageBox.Show("Invalid password. Please try again.");
                    }
                }
                else
                {
                    MessageBox.Show("User not found. Please check your credentials.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                Console.WriteLine($"Error during login: {ex}");
            }

        }
        // Override the OnFormClosing method
    }
}