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
        private int calculatedAmount;  // Declare calculatedAmount as a class-level variable

        public confirmationfinal(string customerName, string customerPhoneNumber, int calculatedAmount, string transactionId)
        {
            InitializeComponent();

            // Display transaction details using labels
            TransLabel.Text = transactionId;
            NameLabel.Text = customerName;
            ContactLabel.Text = customerPhoneNumber;
            TotalAmountLabel.Text = $"{calculatedAmount}"; // Use calculatedAmount

            // Set the class-level calculatedAmount
            this.calculatedAmount = calculatedAmount;

            

            // Subscribe to the TextChanged event of textBox3
            textBox3.TextChanged += textBox3_TextChanged;
        }

        private void confirmationfinal_Load(object sender, EventArgs e)
        {
            // If you need to perform any additional logic when the form loads, you can add it here
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            MessageBox.Show("Booking confirmed!");
            this.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Check if the entered text is a valid number
            if (int.TryParse(textBox3.Text, out int enteredPayment))
            {
                // Calculate and display the change
                int change = enteredPayment - calculatedAmount;

                // Display the change in textBox2
                textBoxchange.Text = change.ToString();
            }
            else
            {
                // Clear textBox2 if the entered text is not a valid number
                textBoxchange.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            dashboard_xhotel newForm = new dashboard_xhotel();
            newForm.Show();
        }
    }
}
