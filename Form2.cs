using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RFIDMantenimiento
{
    public partial class Form2 : Form
    {
        private ToolTip toolTip1;

        public Form2()
        {
            InitializeComponent();
            this.Click += Form2_Click;

            toolTip1 = new ToolTip();
            SetToolTips();

            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(750, 490);
            this.Resize += new EventHandler(Form2_Resize);
            
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.Sorted += dataGridView1_Sorted;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.AllowUserToAddRows = false;
            

            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "ID";
            dataGridView1.Columns[1].Name = "Usuario";
            dataGridView1.Columns[2].Name = "Nombre";
            dataGridView1.Columns[3].Name = "Apellido";
            dataGridView1.Columns[4].Name = "Mail";
            dataGridView1.Columns[5].Name = "Token";

            

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBox1.Items.Add("Escoger Columna");

            comboBox1.SelectedIndex = 0;
            comboBox1.ForeColor = SystemColors.GrayText;
            comboBox1.DropDown += comboBox1_DropDown;
            comboBox1.DropDownClosed += comboBox1_DropDownClosed;

            textBox1.Text = "Filtrar por Columna";
            textBox1.ForeColor = SystemColors.GrayText;

            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;
        }

        private void SetToolTips()
        {
            toolTip1.SetToolTip(pictureBox1, "Actualizar");
            toolTip1.SetToolTip(pictureBox2, "Editar Fila");
            toolTip1.SetToolTip(pictureBox3, "Salir");
        }

        //Responsive
        private void Form2_Resize(object sender, EventArgs e)
        {
            int dataWidth = (int)(this.ClientSize.Width * 0.8); // Ancho del DataGridView al 80% del ancho del formulario
            int dataHeight = (int)(this.ClientSize.Height * 0.8); // Altura del DataGridView al 80% de la altura del formulario

            // Alinea el DataGridView en el centro del formulario
            dataGridView1.Width = dataWidth;
            dataGridView1.Height = dataHeight;
            dataGridView1.Location = new Point((this.ClientSize.Width - dataWidth) / 2, (this.ClientSize.Height - dataHeight) / 2 + 20); // Deja un espacio de 20 píxeles entre el DataGridView y las herramientas

            // Ajusta la ubicación y el tamaño del ComboBox
            comboBox1.Left = dataGridView1.Left + (int)(0.05 * dataWidth) - 20;
            comboBox1.Top = dataGridView1.Top - comboBox1.Height - 20; // Un espacio de 20 píxeles entre las herramientas y el DataGridView
            //comboBox1.Font = new Font("Microsoft Sans Serif", 6 +(int)(0.003 * dataWidth));
            comboBox1.Size = new System.Drawing.Size(130 + (int)(0.05 * dataWidth), comboBox1.Height);

            // Ajusta la ubicación y el tamaño del TextBox
            textBox1.Left = dataGridView1.Left + (int)(0.38 * dataWidth) - (int)(0.05 * dataWidth) - 20; // Un espacio de 10 píxeles entre el ComboBox y el TextBox
            textBox1.Top = comboBox1.Top;
            textBox1.Size = new System.Drawing.Size(130 + (int)(0.05 * dataWidth), textBox1.Height);

            // Ajusta la ubicación y el tamaño del CheckBox
            checkBox1.Left = dataGridView1.Left + (int)(0.68 * dataWidth) - (int)(0.05 * dataWidth) - 25; // Alinea el CheckBox a la derecha del DataGridView
            checkBox1.Top = comboBox1.Top + 3;

            // Ajusta la ubicación y el tamaño del PictureBox
            pictureBox3.Left = (dataGridView1.Left + (int)(0.97 * dataWidth) - (int)(0.05 * dataWidth));
            //dataGridView1.Left + (int)(0.91 * dataWidth) - (int)(0.05 * dataWidth); // Alinea el PictureBox a la derecha del DataGridView
            pictureBox3.Top = comboBox1.Top - 10;

            // Ajusta la ubicación y el tamaño del PictureBox
            pictureBox2.Left = (dataGridView1.Left + (int)(0.97 * dataWidth) - (int)(0.05 * dataWidth)) - 80;
            //dataGridView1.Left + (int)(0.91 * dataWidth) - (int)(0.05 * dataWidth); // Alinea el PictureBox a la derecha del DataGridView
            pictureBox2.Top = comboBox1.Top - 10;

            // Ajusta la ubicación y el tamaño del PictureBox
            pictureBox1.Left = dataGridView1.Left + (int)(0.97 * dataWidth) - (int)(0.05 * dataWidth) - 40; // Alinea el PictureBox a la derecha del DataGridView
            pictureBox1.Top = comboBox1.Top - 10;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar si la celda seleccionada es válida y no es el encabezado de columna
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Seleccionar toda la fila de la celda seleccionada
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            // Deseleccionar cualquier celda seleccionada automáticamente
            if (dataGridView1.SelectedCells.Count > 0)
            {
                dataGridView1.ClearSelection();
            }
        }

        public async void updateData(object sender, EventArgs e)
        {
            await QueryMoodleAndDisplayDataAsync();
            if(dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.Selected)
            {
                //Empezar sin una celda seleccionada
                dataGridView1.CurrentCell.Selected = false;
            }
            dataGridView1.ClearSelection();

        }

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox1.ForeColor = SystemColors.GrayText;
            comboBox1_DropDownClosed(sender, e);

            textBox1.Text = "Filtrar por Columna";
            textBox1.ForeColor = SystemColors.GrayText;
            pictureBox1.Focus();

            if (checkBox1.Checked)
            {
                checkBox1.Checked = false;
            }

            updateData(sender, e);
        }

        //
        private async Task QueryMoodleAndDisplayDataAsync()
        {
            string testApi = "http://172.17.127.254:8200/webservice/rest/more_data.php";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(testApi);

                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(apiResponse);

                        XmlNodeList nodes = xmlDoc.SelectNodes("/users/user");

                        if (nodes != null && nodes.Count > 0)
                        {
                            dataGridView1.Rows.Clear();

                            foreach (XmlNode user in nodes)
                            {
                                string id = user.SelectSingleNode("id")?.InnerText ?? "";
                                string username = user.SelectSingleNode("username")?.InnerText ?? "";
                                string firstname = user.SelectSingleNode("firstname")?.InnerText ?? "";
                                string lastname = user.SelectSingleNode("lastname")?.InnerText ?? "";
                                string email = user.SelectSingleNode("email")?.InnerText ?? "";
                                string token = user.SelectSingleNode("nfc_token")?.InnerText ?? "";

                                String columna = "";
                                if (comboBox1.SelectedItem != null && (string)comboBox1.SelectedItem != "Escoger Columna" && (string)comboBox1.SelectedItem != " ")
                                {
                                    columna = comboBox1.SelectedItem.ToString();
                                }
                                if (comboBox1.SelectedItem != null)
                                { 
                                    switch (comboBox1.SelectedItem.ToString())
                                    {
                                        case "ID":
                                            if (checkBox1.Checked)
                                            {
                                                if (token == "")
                                                {
                                                    if (id.Contains(textBox1.Text.ToString()))
                                                    {
                                                        dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                    }
                                                    else if (textBox1.Text.ToString().Contains("Filtrar por"))
                                                    {
                                                        dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (id.Contains(textBox1.Text.ToString()))
                                                {
                                                    dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                }
                                                else if (textBox1.Text.ToString().Contains("Filtrar por"))
                                                {
                                                    dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                }
                                            }
                                            break;
                                        case "Usuario":
                                            if (checkBox1.Checked)
                                            {
                                                if (token == "")
                                                {
                                                    if (username.Contains(textBox1.Text.ToString()))
                                                    {
                                                        dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                    }
                                                    else if (textBox1.Text.ToString().Contains("Filtrar por"))
                                                    {
                                                        dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (username.Contains(textBox1.Text.ToString()))
                                                {
                                                    dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                }
                                                else if (textBox1.Text.ToString().Contains("Filtrar por"))
                                                {
                                                    dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                }
                                            }
                                            break;
                                        case "Nombre":
                                            if (checkBox1.Checked)
                                            {
                                                if (token == "")
                                                {
                                                    if (firstname.Contains(textBox1.Text.ToString()))
                                                    {
                                                        dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                    }
                                                    else if (textBox1.Text.ToString().Contains("Filtrar por"))
                                                    {
                                                        dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (firstname.Contains(textBox1.Text.ToString()))
                                                {
                                                    dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                }
                                                else if (textBox1.Text.ToString().Contains("Filtrar por"))
                                                {
                                                    dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                }
                                            }
                                            break;
                                        case "Apellido":
                                            if (checkBox1.Checked)
                                            {
                                                if (token == "")
                                                {
                                                    if (lastname.Contains(textBox1.Text.ToString()))
                                                    {
                                                        dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                    }
                                                    else if (textBox1.Text.ToString().Contains("Filtrar por"))
                                                    {
                                                        dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (lastname.Contains(textBox1.Text.ToString()))
                                                {
                                                    dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                }
                                                else if (textBox1.Text.ToString().Contains("Filtrar por"))
                                                {
                                                    dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                }
                                            }
                                            break;
                                        case "Mail":
                                            if (checkBox1.Checked)
                                            {
                                                if (token == "")
                                                {
                                                    if (email.Contains(textBox1.Text.ToString()))
                                                    {
                                                        dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                    }
                                                    else if (textBox1.Text.ToString().Contains("Filtrar por"))
                                                    {
                                                        dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (email.Contains(textBox1.Text.ToString()))
                                                {
                                                    dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                }
                                                else if (textBox1.Text.ToString().Contains("Filtrar por"))
                                                {
                                                    dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                }
                                            }
                                            break;
                                        case "Token":
                                            if (checkBox1.Checked)
                                            {
                                                if (token == "")
                                                {
                                                    if (token.Contains(textBox1.Text.ToString()))
                                                    {
                                                        dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                    }
                                                    else if (textBox1.Text.ToString().Contains("Filtrar por"))
                                                    {
                                                        dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (token.Contains(textBox1.Text.ToString()))
                                                {
                                                    dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                }
                                                else if (textBox1.Text.ToString().Contains("Filtrar por"))
                                                {
                                                    dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                }
                                            }
                                            break;
                                        default:
                                            if (checkBox1.Checked)
                                            {
                                                if (token == "")
                                                {
                                                    dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                                }
                                            }
                                            else
                                            {
                                                dataGridView1.Rows.Add(id, username, firstname, lastname, email, token);
                                            }
                                            break;
                                    }
                                }       
                            }
                            
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron datos de usuarios.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al consultar Moodle: " + ex.Message);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            updateData(sender, e);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            updateData(sender, e);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            updateData(sender, e);
            
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains("Filtrar por") )
            {
                textBox1.Text = "";
                textBox1.ForeColor = SystemColors.WindowText;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if ((string)comboBox1.SelectedItem == " " || (string)comboBox1.SelectedItem == "Escoger Columna")
                {
                    textBox1.Text = "Filtrar por Columna";
                    textBox1.ForeColor = SystemColors.GrayText;
                }
                else
                {
                    textBox1.Text = "Filtrar por " + comboBox1.SelectedItem.ToString();
                    textBox1.ForeColor = SystemColors.GrayText;
                }
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            if((string)comboBox1.SelectedItem == "Escoger Columna")
            {
                comboBox1.Items.Clear();
                comboBox1.ForeColor = SystemColors.WindowText;
                comboBox1.Items.Add(" ");
                comboBox1.Items.Add("ID");
                comboBox1.Items.Add("Usuario");
                comboBox1.Items.Add("Nombre");
                comboBox1.Items.Add("Apellido");
                comboBox1.Items.Add("Mail");
                comboBox1.Items.Add("Token");
            }
            
        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            if((string)comboBox1.SelectedItem == " " || comboBox1.SelectedIndex == -1)
            {
                comboBox1.Items.Clear();
                comboBox1.ForeColor = SystemColors.GrayText;
                comboBox1.Items.Add("Escoger Columna");
                comboBox1.SelectedIndex = 0;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateData(sender, e);
            if (string.IsNullOrWhiteSpace(textBox1.Text) || textBox1.Text.ToString().Contains("Filtrar por "))
            {
                if ((string)comboBox1.SelectedItem == " " || (string)comboBox1.SelectedItem == "Escoger Columna")
                {
                    textBox1.Text = "Filtrar por Columna";
                    textBox1.ForeColor = SystemColors.GrayText;
                }
                else
                {
                    textBox1.Text = "Filtrar por " + comboBox1.SelectedItem.ToString();
                    textBox1.ForeColor = SystemColors.GrayText;
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow filaSeleccionada = dataGridView1.SelectedRows[0];
                if(filaSeleccionada.Cells["ID"].Value != null)
                {
                    Form3 form3 = new Form3(
                        filaSeleccionada.Cells["ID"].Value.ToString(),
                        filaSeleccionada.Cells["Usuario"].Value.ToString(),
                        filaSeleccionada.Cells["Nombre"].Value.ToString(),
                        filaSeleccionada.Cells["Apellido"].Value.ToString(),
                        filaSeleccionada.Cells["Mail"].Value.ToString(),
                        filaSeleccionada.Cells["Token"].Value.ToString(),
                        this
                    );
                    form3.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No se ha seleccionado ninguna fila.\nSeleccione la fila que quiera modificar.");
                }
            }
            else 
            {
                MessageBox.Show("No se ha seleccionado ninguna fila.\nSeleccione la fila que quiera modificar.");
            }
        }

        private void Form2_Click(object sender, EventArgs e)
        {
            // Cambiar el foco a otro control que no sea el control actual
            this.ActiveControl = null;
            dataGridView1.ClearSelection();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();

            Form4 form4 = new Form4();
            form4.ShowDialog();

            this.Close();
        }
    }
}

