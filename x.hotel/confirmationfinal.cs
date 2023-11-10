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
            InitializeComponent();
            // Set the labels with the provided values
            label1.Text = $"Transaction ID: {transactionId}";
            label2.Text = $"Total Amount: ${totalAmount}";
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
