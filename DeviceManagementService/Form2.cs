using DeviceManagementService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using Newtonsoft.Json;

namespace DeviceManagementService
{
    public partial class Form2 : Form
    {
        public class Config
        {
            public string deviceId;
            public string serverIp;
        }
        public void mqttSubs(System.Windows.Forms.ListView listView1, Label label1)
        {
            MqttClient _mqttClient = null;
            try
            {
                Config conf = new Config();
                conf.serverIp = textBox8.Text;

                try
                {
                    _mqttClient = new MqttClient(conf.serverIp);
                    _mqttClient.Connect("Connection Successful" + conf.deviceId);
                }
                catch (MqttConnectionException e)
                {
                    MessageBox.Show("An error occurred while connecting to the broker: " + e.Message);
                }

                if (_mqttClient != null)
                {
                    List<string> newConfig = new List<string>();
                    using (DeviceManagementContext db = new DeviceManagementContext())
                    {
                        foreach (var line in db.Devices)
                        {
                            if (line.DeviceId != null)
                            {
                                newConfig.Add(line.DeviceId);
                            }
                        }
                    }

                    if (newConfig.Count == 0)
                    {
                        MessageBox.Show("Unable to subscribe to any topic.");
                    }
                    else
                    {
                        foreach (var ctsId in newConfig)
                        {
                            string topic = ctsId;

                            ListViewItem item = new ListViewItem(topic);

                            string json = JsonConvert.SerializeObject(conf);
                            byte[] msg = Encoding.Default.GetBytes(json);


                            _mqttClient.Subscribe(new string[] { topic + "/reply" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                            if (_mqttClient.IsConnected)
                            {
                                item.SubItems.Add("Subscribed!");
                                item.ForeColor = Color.Green;
                            }
                            else
                            {
                                item.SubItems.Add("Not subscribed!");
                                item.ForeColor = Color.Red;
                            }

                            listView1.Items.Add(item);
                        }
                        label12.Text = "Subscribed topics (" + newConfig.Count + ")";
                    }
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show("Connection error: " + e.Message);
            }
        }

        DeviceManagementContext db = new DeviceManagementContext();
        int sl = 0;
        bool dg = true;

        public Form2()
        {
            InitializeComponent();
            fill();
        }
        private void fill()
        {
            try
            {
                db = new DeviceManagementContext();
                dataGridView1.DataSource = db.Devices.Where(d => d.Databit == dg).Select(d => new { d.Id, d.DeviceName, d.RoomId, d.Ip, d.Port, d.DeviceId, d.MqttServer, d.Rs232port, d.Baudrate, d.Ssid, d.Password }).ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: \n\n" + e.Message);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            fill();
            Label signatureLabel = new Label();
            signatureLabel.AutoSize = true;
            signatureLabel.Text = "Design by software developer Suat Mollasalihoglu";
            signatureLabel.Font = new Font("Arial", 10, FontStyle.Italic);
            signatureLabel.ForeColor = Color.LightSeaGreen;
            signatureLabel.Location = new Point(this.ClientSize.Width - signatureLabel.PreferredWidth - 10, this.ClientSize.Height - signatureLabel.PreferredHeight - 10);
            this.Controls.Add(signatureLabel);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int MAX_ROOM_COUNT = db.Rooms.Count();

            string marka = comboBox1.SelectedItem.ToString();
            Device newDevice = null;

            try
            {
                switch (marka)
                {
                    case "GE":
                        newDevice = new Device();
                        newDevice.DeviceName = comboBox1.SelectedItem.ToString();
                        newDevice.Databit = true;
                        newDevice.Rs232port = comboBox2.Text;
                        newDevice.Baudrate = comboBox3.Text;
                        newDevice.DeviceId = textBox4.Text;
                        newDevice.Ssid = textBox5.Text;
                        newDevice.Password = textBox6.Text;
                        newDevice.MqttServer = textBox7.Text;

                        int roomId;
                        if (!int.TryParse(textBox3.Text, out roomId))
                        {
                            MessageBox.Show("The number of rooms is invalid. Please enter a valid number of rooms");
                            return;
                        }
                        if (roomId > MAX_ROOM_COUNT)
                        {
                            MessageBox.Show($"Maximum {MAX_ROOM_COUNT} room supported. Please enter a valid number of rooms.");
                            return;
                        }
                        var existingCihaz = db.Devices.FirstOrDefault(c => c.DeviceId == newDevice.DeviceId);
                        if (existingCihaz != null)
                        {
                            MessageBox.Show($"Device ID {newDevice.DeviceId} already exists. Please enter a different Device ID.");
                            return;
                        }

                        newDevice.RoomId = roomId;

                        if (string.IsNullOrWhiteSpace(newDevice.DeviceId) || string.IsNullOrWhiteSpace(newDevice.Ssid) || string.IsNullOrWhiteSpace(newDevice.Password) || string.IsNullOrWhiteSpace(newDevice.MqttServer) || string.IsNullOrWhiteSpace(newDevice.Rs232port) || string.IsNullOrWhiteSpace(newDevice.Baudrate))
                        {
                            MessageBox.Show("Do not leave any blank space.");
                            return;
                        }
                        db.Devices.Add(newDevice);
                        db.SaveChanges();
                        fill();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                        textBox6.Clear();
                        textBox7.Clear();
                        break;

                    case "MAQUET":
                        newDevice = new Device();
                        newDevice.DeviceName = comboBox1.SelectedItem.ToString();
                        newDevice.Databit = true;
                        newDevice.Rs232port = comboBox2.Text;
                        newDevice.Baudrate = comboBox3.Text;
                        newDevice.DeviceId = textBox4.Text;
                        newDevice.Ssid = textBox5.Text;
                        newDevice.Password = textBox6.Text;
                        newDevice.MqttServer = textBox7.Text;

                        if (!int.TryParse(textBox3.Text, out roomId))
                        {
                            MessageBox.Show("The number of rooms is invalid. Please enter a valid number of rooms");
                            return;
                        }
                        if (roomId > MAX_ROOM_COUNT)
                        {
                            MessageBox.Show($"Maximum {MAX_ROOM_COUNT} room is supported. Please enter a valid number of rooms.");
                            return;
                        }
                        var existingCihaz1 = db.Devices.FirstOrDefault(c => c.DeviceId == newDevice.DeviceId);
                        if (existingCihaz1 != null)
                        {
                            MessageBox.Show($"Device ID {newDevice.DeviceId}already available. Please enter a different Device ID.");
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(newDevice.DeviceId) || string.IsNullOrWhiteSpace(newDevice.Ssid) || string.IsNullOrWhiteSpace(newDevice.Password) || string.IsNullOrWhiteSpace(newDevice.MqttServer) || string.IsNullOrWhiteSpace(newDevice.Rs232port) || string.IsNullOrWhiteSpace(newDevice.Baudrate))
                        {
                            MessageBox.Show("Do not leave any blank space.");
                            return;
                        }

                        newDevice.RoomId = roomId;
                        db.Devices.Add(newDevice);
                        db.SaveChanges();
                        fill();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                        textBox6.Clear();
                        textBox7.Clear();
                        break;

                    case "LEONI":
                        newDevice = new Device();
                        newDevice.DeviceName = comboBox1.SelectedItem.ToString();
                        newDevice.Databit = true;
                        newDevice.Rs232port = comboBox2.Text;
                        newDevice.Baudrate = comboBox3.Text;
                        newDevice.DeviceId = textBox4.Text;
                        newDevice.Ssid = textBox5.Text;
                        newDevice.Password = textBox6.Text;
                        newDevice.MqttServer = textBox7.Text;

                        if (!int.TryParse(textBox3.Text, out roomId))
                        {
                            MessageBox.Show("The number of rooms is invalid. Please enter a valid number of rooms");
                            return;
                        }
                        if (roomId > MAX_ROOM_COUNT)
                        {
                            MessageBox.Show($"Maximum {MAX_ROOM_COUNT} room is supported. Please enter a valid number of rooms.");
                            return;
                        }
                        var existingCihaz2 = db.Devices.FirstOrDefault(c => c.DeviceId == newDevice.DeviceId);
                        if (existingCihaz2 != null)
                        {
                            MessageBox.Show($"Device ID {newDevice.DeviceId} already available. Please enter a different Device ID.");
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(newDevice.DeviceId) || string.IsNullOrWhiteSpace(newDevice.Ssid) || string.IsNullOrWhiteSpace(newDevice.Password) || string.IsNullOrWhiteSpace(newDevice.MqttServer) || string.IsNullOrWhiteSpace(newDevice.Rs232port) || string.IsNullOrWhiteSpace(newDevice.Baudrate))
                        {
                            MessageBox.Show("Do not leave any blank space.");
                            return;
                        }

                        newDevice.RoomId = roomId;
                        db.Devices.Add(newDevice);
                        db.SaveChanges();
                        fill();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                        textBox6.Clear();
                        textBox7.Clear();
                        break;

                    case "DRAGER":
                        newDevice = new Device();
                        newDevice.DeviceName = comboBox1.SelectedItem.ToString();
                        newDevice.Databit = true;
                        newDevice.Rs232port = comboBox2.Text;
                        newDevice.Baudrate = comboBox3.Text;
                        newDevice.DeviceId = textBox4.Text;
                        newDevice.Ssid = textBox5.Text;
                        newDevice.Password = textBox6.Text;
                        newDevice.MqttServer = textBox7.Text;

                        if (!int.TryParse(textBox3.Text, out roomId))
                        {
                            MessageBox.Show("The number of rooms is invalid. Please enter a valid number of rooms");
                            return;
                        }
                        if (roomId > MAX_ROOM_COUNT)
                        {
                            MessageBox.Show($"Maximum {MAX_ROOM_COUNT} room is supported. Please enter a valid number of rooms.");
                            return;
                        }
                        var existingCihaz3 = db.Devices.FirstOrDefault(c => c.DeviceId == newDevice.DeviceId);
                        if (existingCihaz3 != null)
                        {
                            MessageBox.Show($"Device ID {newDevice.DeviceId} already available. Please enter a different Device ID.");
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(newDevice.DeviceId) || string.IsNullOrWhiteSpace(newDevice.Ssid) || string.IsNullOrWhiteSpace(newDevice.Password) || string.IsNullOrWhiteSpace(newDevice.MqttServer) || string.IsNullOrWhiteSpace(newDevice.Rs232port) || string.IsNullOrWhiteSpace(newDevice.Baudrate))
                        {
                            MessageBox.Show("Do not leave any blank space.");
                            return;
                        }

                        newDevice.RoomId = roomId;
                        db.Devices.Add(newDevice);
                        db.SaveChanges();
                        fill();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                        textBox6.Clear();
                        textBox7.Clear();
                        break;

                    case "MINDRAY":
                        newDevice = new Device();
                        newDevice.DeviceName = comboBox1.SelectedItem.ToString();
                        newDevice.Databit = true;
                        newDevice.Ip = textBox1.Text;
                        newDevice.Port = textBox2.Text;

                        if (!int.TryParse(textBox3.Text, out roomId))
                        {
                            MessageBox.Show("The number of rooms is invalid. Please enter a valid number of rooms");
                            return;
                        }
                        if (roomId > MAX_ROOM_COUNT)
                        {
                            MessageBox.Show($"Maximum {MAX_ROOM_COUNT} room is supported. Please enter a valid number of rooms.");
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(newDevice.Ip) || string.IsNullOrWhiteSpace(newDevice.Port))
                        {
                            MessageBox.Show("Do not leave any blank space.");
                            return;
                        }

                        newDevice.RoomId = roomId;
                        db.Devices.Add(newDevice);
                        db.SaveChanges();
                        fill();
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        break;

                    case "TMS":
                        newDevice = new Device();
                        newDevice.DeviceName = comboBox1.SelectedItem.ToString();
                        newDevice.Databit = true;
                        newDevice.Ip = textBox1.Text;
                        newDevice.Port = textBox2.Text;

                        if (!int.TryParse(textBox3.Text, out roomId))
                        {
                            MessageBox.Show("The number of rooms is invalid. Please enter a valid number of rooms");
                            return;
                        }
                        if (roomId > MAX_ROOM_COUNT)
                        {
                            MessageBox.Show($"Maximum {MAX_ROOM_COUNT} room is supported. Please enter a valid number of rooms.");
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(newDevice.Ip) || string.IsNullOrWhiteSpace(newDevice.Port))
                        {
                            MessageBox.Show("Do not leave any blank space.");
                            return;
                        }

                        newDevice.RoomId = roomId;
                        db.Devices.Add(newDevice);
                        db.SaveChanges();
                        fill();
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int MAX_ROOM_COUNT = db.Rooms.Count();
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
            int deviceId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
            Device editedDevice = db.Devices.FirstOrDefault(s => s.Id == deviceId);

            if (editedDevice.DeviceName == "GE" || editedDevice.DeviceName == "MAQUET" || editedDevice.DeviceName == "LEONI" || editedDevice.DeviceName == "DRAGER")
            {
                editedDevice.DeviceName = comboBox1.SelectedItem.ToString();
                editedDevice.Rs232port = comboBox2.SelectedItem.ToString();
                editedDevice.Baudrate = comboBox3.SelectedItem.ToString();
                editedDevice.Ip = textBox1.Text;
                editedDevice.Port = textBox2.Text;
                editedDevice.RoomId = Convert.ToInt32(textBox3.Text);
                editedDevice.DeviceId = textBox4.Text;
                editedDevice.Ssid = textBox5.Text;
                editedDevice.Password = textBox6.Text;
                editedDevice.MqttServer = textBox7.Text;
                if (!int.TryParse(textBox3.Text, out int bedId))
                {
                    MessageBox.Show("The number of rooms is invalid. Please enter a valid number of rooms");
                    return;
                }
                if (bedId > MAX_ROOM_COUNT)
                {
                    MessageBox.Show($"Maximum {MAX_ROOM_COUNT} room is supported. Please enter a valid number of rooms.");
                    return;
                }
                //var existingCihaz3 = db.Devices.FirstOrDefault(c => c.DeviceId == editedDevice.DeviceId);
                //if (existingCihaz3 != null)
                //{
                //    MessageBox.Show($"Device ID {editedDevice.DeviceId} already available. Please enter a different Device ID.");
                //    return;
                //}

                if (string.IsNullOrWhiteSpace(editedDevice.DeviceId) || string.IsNullOrWhiteSpace(editedDevice.Ssid) || string.IsNullOrWhiteSpace(editedDevice.Password) || string.IsNullOrWhiteSpace(editedDevice.MqttServer) || string.IsNullOrWhiteSpace(editedDevice.Rs232port) || string.IsNullOrWhiteSpace(editedDevice.Baudrate))
                {
                    MessageBox.Show("Do not leave any blank space.");
                    return;
                }
                db.SaveChanges();
                fill();
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
            }
            else
            {
                editedDevice.DeviceName = comboBox1.SelectedItem.ToString();
                editedDevice.Ip = textBox1.Text;
                editedDevice.Port = textBox2.Text;
                editedDevice.RoomId = Convert.ToInt32(textBox3.Text);
                db.SaveChanges();
                fill();
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
            int id = Convert.ToInt32(selectedRow.Cells["Id"].Value);

            Device deletedDevice = db.Devices.Where(s => s.Id == id).FirstOrDefault();

            try
            {
                if (deletedDevice != null)
                {
                    db.Devices.Remove(deletedDevice);
                    db.SaveChanges();
                    fill();
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);

                    //if (dataGridView1.SelectedRows.Count > 0)
                    //{
                    //    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 1 || comboBox1.SelectedIndex == 2 || comboBox1.SelectedIndex == 3)
            {
                textBox1.Visible = false;
                label2.Visible = false;
                textBox2.Visible = false;
                label3.Visible = false;

                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                label9.Visible = true;
                label10.Visible = true;
                comboBox2.Visible = true;
                comboBox3.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = true;
                textBox6.Visible = true;
                textBox7.Visible = true;
            }
            else if (comboBox1.SelectedIndex == 4 || comboBox1.SelectedIndex == 5)
            {
                textBox1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
                label2.Visible = true;
                label3.Visible = true;

                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                comboBox2.Visible = false;
                comboBox3.Visible = false;
                textBox4.Visible = false;
                textBox5.Visible = false;
                textBox6.Visible = false;
                textBox7.Visible = false;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                sl = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0]?.Value?.ToString());
                comboBox1.SelectedItem = dataGridView1.CurrentRow.Cells[1]?.Value?.ToString();
                comboBox2.SelectedItem = dataGridView1.CurrentRow.Cells[7]?.Value?.ToString();
                comboBox3.SelectedItem = dataGridView1.CurrentRow.Cells[8]?.Value?.ToString();
                textBox1.Text = GetCellValue(dataGridView1.CurrentRow, 3);
                textBox2.Text = GetCellValue(dataGridView1.CurrentRow, 4);
                textBox3.Text = GetCellValue(dataGridView1.CurrentRow, 2);
                textBox4.Text = GetCellValue(dataGridView1.CurrentRow, 5);
                textBox5.Text = GetCellValue(dataGridView1.CurrentRow, 9);
                textBox6.Text = GetCellValue(dataGridView1.CurrentRow, 10);
                textBox7.Text = GetCellValue(dataGridView1.CurrentRow, 6);
            }
        }
        private string GetCellValue(DataGridViewRow row, int columnIndex)
        {
            if (columnIndex >= 0 && columnIndex < row.Cells.Count)
            {
                return row.Cells[columnIndex]?.Value?.ToString();
            }
            return string.Empty;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mqttSubs(listView1, label12);
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }
    }
}
