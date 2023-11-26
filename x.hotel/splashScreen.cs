using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace x.hotel
{
    public partial class splashScreen : Form
    {
        public splashScreen()
        {
            InitializeComponent();
        }

        private void splashScreen_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < 100)
            {
                progressBar1.Value += 1;

                label1.Text = progressBar1.Value.ToString() + "%";

                // Display messages based on the progress value
                DisplayProgressMessage(progressBar1.Value);
            }
            else
            {
                timer1.Stop();
                Onboarding newForm = new Onboarding();
                newForm.Show();
                this.Hide();
            }
        }

        private void DisplayProgressMessage(int progressValue)
        {
            if (progressValue < 30)
            {
                label2.Text = "Obtaining authentication data from secure server...";
            }
            else if (progressValue < 60)
            {
                label2.Text = "Connecting to Firebase Realtime Database...";
            }
            else
            {
                label2.Text = "Starting...";
            }
        }
    }
}
