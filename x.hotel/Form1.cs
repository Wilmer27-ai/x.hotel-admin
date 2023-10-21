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
    public partial class Form1 : Form
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "x0WQCfAAdZVlyYXDbbIh9W5fG51aNI8uNj6zYmMn",
            BasePath = "https://x-hotel-451cb-default-rtdb.asia-southeast1.firebasedatabase.app/",
        };
        IFirebaseClient client;
        public Form1()
        {
            InitializeComponent();
            client = new FireSharp.FirebaseClient(config);
            if (client != null)
            {
                MessageBox.Show("Connection Established");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
