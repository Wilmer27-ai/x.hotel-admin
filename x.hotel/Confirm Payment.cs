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
    public partial class Confirm_Payment : Form
    {
        private Room selectedRoom;
        private Guest guest;
        private IFirebaseConfig Config;
        private IFirebaseClient Client;

        public Confirm_Payment(Room selectedRoom, Guest guest)
        {
            InitializeComponent();
            this.selectedRoom = selectedRoom;
            this.guest = guest;
            InitializeFirebase();
            PopulateData();
        }
        private void InitializeFirebase()
        {
            Config = new FirebaseConfig
            {
                AuthSecret = "x0WQCfAAdZVlyYXDbbIh9W5fG51aNI8uNj6zYmMn",
                BasePath = "https://x-hotel-451cb-default-rtdb.asia-southeast1.firebasedatabase.app"
            };

            Client = new FireSharp.FirebaseClient(Config);
        }
        private void PopulateData()
        {
            // Assuming you have TextBox controls named textBoxTransactionId, textBoxTotalAmount, 
            // textBoxAmountPaid, and textBoxChange on your Confirm_Payment form

            // Auto-increment transaction ID (you need to implement this logic)
            string transactionId = GenerateTransactionId();

            // Calculate total amount (assuming you have a method to calculate it)
            int totalAmount = CalculateTotalAmount(selectedRoom);

            // Populate textboxes
            textBox1.Text = transactionId;
            textBox2.Text = totalAmount.ToString();
        }

        private string GenerateTransactionId()
        {
            // Implement your logic to generate a unique transaction ID here
            // For example, you can use a counter or concatenate current date and time
            return Guid.NewGuid().ToString();
        }

        private int CalculateTotalAmount(Room room)
        {
            // Implement your logic to calculate the total amount based on the selected room
            // For example, you can multiply roomDailyRate by the number of days booked
            return room.roomDailyRate;
        }




        private void button2_Click(object sender, EventArgs e)
        {
            // Get data from textboxes
            string transactionId = textBox1.Text;
            int totalAmount = Convert.ToInt32(textBox2.Text);
            int amountPaid = Convert.ToInt32(textBox3.Text);
            int change = amountPaid - totalAmount;

            // Create a Payment object
            Payment payment = new Payment
            {
                TransactionId = transactionId,
                TotalAmount = totalAmount,
                AmountPaid = amountPaid,
                Change = change
            };

            // Save payment data to Firebase
            FirebaseResponse response = Client.Set($"Payments/{transactionId}", payment);

            // Show a message or perform further actions
            MessageBox.Show("Payment confirmed!");
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numeric input for amount paid
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Recalculate and update the change when the amount paid changes
            UpdateChange();
        }

        private void UpdateChange()
        {
            // Get the amount paid and total amount
            if (int.TryParse(textBox3.Text, out int amountPaid) && int.TryParse(textBox2.Text, out int totalAmount))
            {
                // Calculate the change
                int change = amountPaid - totalAmount;

                // Display the change in TextBox4
                textBox4.Text = change.ToString();
            }
            else
            {
                // Handle invalid input or display an error message
                textBox4.Text = "Invalid input";
            }
        }
    }
}
