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
        private readonly IFirebaseConfig firebaseConfig = new FirebaseConfig
        {
            AuthSecret = "XX9AMWcYQ0S2FKOGWT0DWAzKwehEhkYew84T91lg",
            BasePath = "https://crud-daaa1-default-rtdb.firebaseio.com"
        };

        private IFirebaseClient firebaseClient;

        public Admin()
        {
            InitializeComponent();
            firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);

            if (firebaseClient == null)
            {
                MessageBox.Show("Failed to initialize Firebase client.");
                // Handle the situation where Firebase initialization fails.
                // You might want to disable some functionality or exit the application.
                // For now, you can return or throw an exception to stop further execution.
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SignUpForm newForm = new SignUpForm();
            newForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SignIn();
        }

        private async void SignIn()
        {
            try
            {
                string username = textBox1.Text;
                string password = textBox2.Text;

                if (firebaseClient == null)
                {
                    MessageBox.Show("Firebase client is not initialized.");
                    return;
                }

                FirebaseResponse response = await Task.Run(() => firebaseClient.Get($"Admin/{username}"));
                var user = response.ResultAs<AdminInfo>();

                if (user != null && user.Password == password)
                {
                    MessageBox.Show("Login successful!");
                    SignUpForm newForm = new SignUpForm();
                    newForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Login failed. Please check your credentials.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}\n\n{ex.StackTrace}");
            }
        }
    }
}