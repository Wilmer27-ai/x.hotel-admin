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
        IFirebaseConfig Config = new FirebaseConfig
        {
            AuthSecret = "x0WQCfAAdZVlyYXDbbIh9W5fG51aNI8uNj6zYmMn",
            BasePath = "https://x-hotel-451cb-default-rtdb.asia-southeast1.firebasedatabase.app"
        };

        IFirebaseClient Client;
        public SignUpForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             SignUp();
        }

        private void SignUpForm_Load(object sender, EventArgs e)
        {
            Client = new FireSharp.FirebaseClient(Config);
        }

        private async void SignUp()
        {
            string adminId = textBox1.Text;
            string username = textBox2.Text;
            string password = textBox3.Text;

            var Admin = new AdminInfo
            {
                AdminId = adminId,
                Username = username,
                Password = password
            };

            try
            {
                FirebaseResponse response = await Client.SetAsync("Admins/" + adminId, Admin);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Signup successful!");
                }
                else
                {
                    MessageBox.Show("Error occurred while signing up. Please try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }



        }
    }
}
