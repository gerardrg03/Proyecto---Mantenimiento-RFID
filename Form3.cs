using Newtonsoft.Json.Linq;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RFIDMantenimiento
{
    public partial class Form3 : Form
    {
        string tokenGlobal;
        private Form2 _parentForm;
        public Form3(string ID, string username, string firstname, string lastname, string email, string token, Form2 parentForm)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            tokenGlobal = token;
            _parentForm = parentForm;

            textBox1.Text = ID;
            textBox2.Text = username;
            textBox3.Text = firstname;
            textBox4.Text = lastname;
            textBox5.Text = email;
            comboBox1.Text = token;

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            
            //comboBox1.ForeColor = SystemColors.GrayText;
            comboBox1.DropDown += comboBox1_DropDown;
            comboBox1.DropDownClosed += comboBox1_DropDownClosed;

            
            CargarTokens();

            if (!string.IsNullOrEmpty(token))
            {
                comboBox1.SelectedIndex = 1;
                comboBox1.Enabled = false;
            }
            
            textBox1.Enabled = false;

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
        }

        private async void CargarTokens()
        {
            comboBox1.Items.Add(" ");

            if (!string.IsNullOrEmpty(tokenGlobal))
            {
                comboBox1.Items.Add(tokenGlobal);
                //SetComboBoxText(token.ToString());
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "http://172.17.127.254:8200/webservice/rest/list_tokens.php";

                    // Realizar la solicitud GET al servidor
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer la respuesta del servidor
                        string responseData = await response.Content.ReadAsStringAsync();

                        // Dividir la respuesta en tokens individuales
                        string[] tokens = responseData.Split(',');

                        // Agregar los tokens al ComboBox
                        comboBox1.Items.AddRange(tokens);
                    }
                    else
                    {
                        MessageBox.Show("Error al obtener los tokens del servidor. Código de estado HTTP: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "http://172.17.127.254:8200/webservice/rest/update_data.php";

                    // Obtener los valores de los TextBox
                    string id = textBox1.Text;
                    string username = textBox2.Text;
                    string firstname = textBox3.Text;
                    string lastname = textBox4.Text;
                    string email = textBox5.Text;
                    string nfcToken = "";

                    if(comboBox1.Enabled)
                    {
                        nfcToken = comboBox1.Text;
                    }
                    
                    // Construir los datos que se enviarán en la solicitud POST
                    string postData = $"id={id}&username={username}&firstname={firstname}&lastname={lastname}&email={email}&nfc_token={nfcToken}";
                    StringContent content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");

                    // Realizar la solicitud POST al servidor
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Datos enviados correctamente al servidor.");
                        this.Close();
                        //Form2 form2 = new Form2();
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            if ((string)comboBox1.SelectedItem == tokenGlobal)
            {
                comboBox1.Items.Clear();
                //comboBox1.ForeColor = SystemColors.WindowText;
                CargarTokens();
            }
        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            if ((string)comboBox1.SelectedItem == tokenGlobal + " (actual)" || comboBox1.SelectedIndex == -1)
            {
                comboBox1.Items.Clear();
                //comboBox1.ForeColor = SystemColors.GrayText;
                comboBox1.Items.Add(tokenGlobal);
                comboBox1.SelectedIndex = 0;
            }
        }

        public void SetComboBoxText(string text)
        {
            if (comboBox1.Items.Contains(text + " (actual)"))
            {
                string fin = comboBox1.SelectedItem.ToString().Replace(" (actual)", "");
                comboBox1.SelectedItem = fin; 
            }
        }
    }
}
