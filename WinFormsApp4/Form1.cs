using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp4
{
    public partial class Form1 : Form
    {
        string fullname, password, username;
        Image picture;
        public Form1()
        {
            InitializeComponent();
        }

        private void SaveData()
        {
            fullname = textBox1.Text;
            password = textBox2.Text;
            username = textBox3.Text;

            if (pictureBox1.Image != null)
            {
                picture = pictureBox1.Image;
            }

            byte[] imageData = null;
            if (picture != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    picture.Save(ms, ImageFormat.Jpeg);
                    imageData = ms.ToArray();
                }
            }

            string connectionString = "Data Source=DESKTOP-MJEN1H5\\SQLEXPRESS;Initial Catalog=DB_KYAN;Persist Security Info=True;User ID=mm;Password=1;";
            string insertQuery = "INSERT INTO TBL_ADMIN (FULL_NAME, ADMIN_USERNAME, PASSWORD, PICTURE) " +
                                 "VALUES (@fullname, @username, @password, @picture)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@fullname", fullname);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@picture", (object)imageData ?? DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Data Saved Successfully");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Choose a Picture";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    picture = Image.FromFile(openFileDialog.FileName);
                    pictureBox1.Image = picture;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveData();
        }
    }
}


