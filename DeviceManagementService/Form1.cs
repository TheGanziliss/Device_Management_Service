using Microsoft.Data.SqlClient;

namespace DeviceManagementService
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ServerIp = textBox1.Text;
            string UserName = textBox2.Text;
            string Password = textBox3.Text;
            string Database = textBox4.Text;

            textBox3.UseSystemPasswordChar = true;

            if (string.IsNullOrEmpty(ServerIp) || string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Do not leave any blank space.");
                return;
            }

            try
            {
                ServerSettings.ServerIp = ServerIp;
                ServerSettings.userName = UserName;
                ServerSettings.password = Password;
                ServerSettings.database = Database;

                MessageBox.Show("Informations Saved.");

                Form2 form2 = new Form2();
                form2.Show();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("The entered information is invalid. Please try again.\n\n" + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Label signatureLabel = new Label();
            signatureLabel.AutoSize = true;
            signatureLabel.Text = "Design by software developer Suat Mollasalihoglu";
            signatureLabel.Font = new Font("Arial", 10, FontStyle.Italic);
            signatureLabel.ForeColor = Color.LightSeaGreen;
            signatureLabel.Location = new Point(this.ClientSize.Width - signatureLabel.PreferredWidth - 10, this.ClientSize.Height - signatureLabel.PreferredHeight - 10);
            this.Controls.Add(signatureLabel);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }
    }
}