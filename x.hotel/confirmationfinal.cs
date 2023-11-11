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
    public partial class confirmationfinal : Form
    {
        public confirmationfinal(string transactionId, int totalAmount)
        {
            confirmationfinal confirmationForm = new confirmationfinal(transactionId, totalAmount);
            confirmationForm.ShowDialog();
            InitializeComponent();
            textBox1.Text = transactionId;
            textBox2.Text = totalAmount.ToString();
        }

        private void confirmationfinal_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Booking confirmed!");
            this.Close();
        }
    }
}
