using System;
using System.Windows.Forms;

namespace x.hotel
{
    public partial class Onboarding : Form
    {
        public Onboarding()
        {
            InitializeComponent();
           this.FormClosing += Onboarding_FormClosing; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoginForm newForm = new LoginForm();
            newForm.Show();
        }

        // Handle the FormClosing event
        private void Onboarding_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // The form is being closed by the user, exit the application
                Application.Exit();
            }
            // You can add additional logic or conditions if needed
        }
    }
}
