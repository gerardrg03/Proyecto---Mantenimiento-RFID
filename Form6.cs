using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RFIDMantenimiento
{
    public partial class Form6 : Form
    {
        private Form5 _parentForm;
        public Form6(Form5 parentForm)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            _parentForm = parentForm;

            button1.Enabled = false;
            textBox1.ReadOnly = true;
            label1.Focus();
            this.KeyPreview = true; // Allow the form to receive key events first
            this.KeyDown += new KeyEventHandler(Form1_KeyDown); // Subscribe to the KeyDown event
            this.KeyPress += new KeyPressEventHandler(Form1_KeyPress); // Subscribe to the KeyPress event
            // Timer setup
            keyPressTimer.Interval = TimeThresholdMilliseconds;
            keyPressTimer.Elapsed += KeyPressTimer_Elapsed;
            keyPressTimer.AutoReset = false; // Ensures the timer only runs once per keypress sequence

            lastKeyPressTime = DateTime.Now;

            this.Click += Form6_Click;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        StringBuilder inputBuffer = new StringBuilder();

        private System.Timers.Timer keyPressTimer = new System.Timers.Timer();
        private DateTime lastKeyPressTime;
        private const int TimeThresholdMilliseconds = 40; // Time threshold in milliseconds, adjust as needed

        private void KeyPressTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Clears the input buffer if the timer elapses
            inputBuffer.Clear();
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string nfcData = inputBuffer.ToString();
                textBox1.Text = nfcData; // Display the NFC data in textBox1
                //textBox1.ReadOnly = true; // Disable writing to the TextBox
                                          //Inserta aqui función para recojer nfcData

                inputBuffer.Clear(); // Clear the buffer for the next input
                e.Handled = true; // Mark the event as handled
            }
        }


        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Calculate the time since the last key press
            TimeSpan timeSinceLastKeyPress = DateTime.Now - lastKeyPressTime;

            // Reset the timer and the timestamp
            lastKeyPressTime = DateTime.Now;
            keyPressTimer.Stop();
            keyPressTimer.Start();

            // If the time since the last keypress is below the threshold, append the character
            if (timeSinceLastKeyPress.TotalMilliseconds < TimeThresholdMilliseconds)
            {
                inputBuffer.Append(e.KeyChar);
            }
            else
            {
                // If too much time has passed, clear the buffer before appending
                inputBuffer.Clear();
                inputBuffer.Append(e.KeyChar);
            }

            e.Handled = true;
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            
        }

        private void Form6_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "http://172.17.127.254:8200/webservice/rest/insert_token.php";

                    // Obtener los valores de los TextBox
                    string token = textBox1.Text;
                    

                    // Construir los datos que se enviarán en la solicitud POST
                    string postData = $"token={token}";
                    StringContent content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");

                    // Realizar la solicitud POST al servidor
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Datos enviados correctamente al servidor.");
                        this.Close();
                        _parentForm.updateData(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Error al enviar datos al servidor. Código de estado HTTP: " + response.StatusCode);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0)
            {
                button1.Enabled = true;
            }
        }
    }
}
