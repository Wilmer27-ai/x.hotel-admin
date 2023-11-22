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
    public partial class Onboarding : Form
    {
        public Onboarding()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoginForm newForm = new LoginForm();
            newForm.Show();
        }
    }
}
