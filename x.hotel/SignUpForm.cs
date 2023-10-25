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
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await SignUp();
        }

        private async Task SignUp()
        {
            string adminId = textBox1.Text;
            string username = textBox2.Text;
            string password = textBox3.Text;

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