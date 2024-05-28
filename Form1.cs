using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;

namespace RFIDMantenimiento
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Click += Form1_Click;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            textBox1.Text = "Usuario ";
            textBox2.Text = "Contraseña ";
            textBox1.ForeColor = SystemColors.GrayText;
            textBox2.ForeColor = SystemColors.GrayText;

            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;
            textBox2.Enter += textBox2_Enter;
            textBox2.Leave += textBox2_Leave;

            textBox2.TextChanged += textBox2_TextChanged;

            pictureBox2.Hide();
            label1.Hide();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string usuario = textBox1.Text;
            string contra = textBox2.Text;

            try
            {
                bool acceso = await AuthenticateWithMoodle(usuario, contra);

                if (acceso)
                {
                    this.Hide();

                    Form4 form4 = new Form4();
                    form4.ShowDialog();

                    this.Close();
                }
                else
                {
                    label1.Text = "Usuario o contraseña incorrectos.";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al autenticar con Moodle: " + ex.Message);
            }
        }
        //http://172.17.127.254:8200/webservice/rest/login.php

        private async Task<bool> AuthenticateWithMoodle(string username, string password)
        {
            using (var client = new HttpClient())
            {
                var postData = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>>();
                postData.Add(new System.Collections.Generic.KeyValuePair<string, string>("username", username));
                postData.Add(new System.Collections.Generic.KeyValuePair<string, string>("password", password));

                var content = new FormUrlEncodedContent(postData);
                var response = await client.PostAsync("http://172.17.127.254:8200/webservice/rest/login_a.php", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    return responseString.Contains("ok");
                }
                else
                {
                    Console.WriteLine("Error en la solicitud HTTP");
                    return false;
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "Contraseña ")
            {
                if (textBox2.PasswordChar == '●')
                {
                    textBox2.PasswordChar = '\0';
                }
                else
                {
                    textBox2.PasswordChar = '●';

                }
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Usuario ")
            {
                textBox1.Text = "";
                textBox1.ForeColor = SystemColors.WindowText;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Usuario ";
                textBox1.ForeColor = SystemColors.GrayText;
            }
        }
        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Contraseña ")
            {
                textBox2.Text = "";
                textBox2.ForeColor = SystemColors.WindowText;
                textBox2.PasswordChar = '●';
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = "Contraseña ";
                textBox2.ForeColor = SystemColors.GrayText;
                textBox2.PasswordChar = '\0';
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "Contraseña " || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                pictureBox2.Hide();
            }
            else
            {
                pictureBox2.Show();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ActiveControl = button1;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            // Cambiar el foco a otro control que no sea el control actual
            this.ActiveControl = null;
        }
    }
}