using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.Http;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace RFIDMantenimiento
{

    public partial class Form5 : Form
    {
        DataGridViewComboBoxColumn cbcs = new DataGridViewComboBoxColumn();
        bool update = false;
        private bool isProcessing = false;
        private Control previousControl;
        private bool isMessageBoxShown = false;
        private System.Windows.Forms.ToolTip toolTip1;

        public Form5()
        {
            InitializeComponent();
            this.Click += Form5_Click;

            toolTip1 = new System.Windows.Forms.ToolTip();
            SetToolTips();

            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(750, 490);
            this.Resize += new EventHandler(Form5_Resize);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.EditingControlShowing += dataGridView1_EditingControlShowing;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "ID";
            dataGridView1.Columns[1].Name = "Token";
            dataGridView1.Columns[2].Name = "Fecha Registro";
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns.Add(cbcs);
            
            cbcs.Name = "Estado";
            cbcs.HeaderText = "Estado";
            cbcs.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            cbcs.FlatStyle = FlatStyle.Flat;

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
            toolTip1.SetToolTip(pictureBox2, "Añadir Token");
            toolTip1.SetToolTip(pictureBox3, "Salir");
        }

        private void Form5_Click(object sender, EventArgs e)
        {
            // Cambiar el foco a otro control que no sea el control actual
            dataGridView1.ClearSelection();
            this.ActiveControl = null;
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar si la celda seleccionada es válida y no es el encabezado de columna
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex < 3)
            {
                if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.Selected)
                {
                    //Empezar sin una celda seleccionada
                    dataGridView1.CurrentCell.Selected = false;
                }
            }

            if(dataGridView1.IsCurrentCellInEditMode && dataGridView1.CurrentCell is DataGridViewComboBoxCell && e.ColumnIndex == dataGridView1.Columns["Estado"].Index)
            {
                // Verificar si el control de edición está disponible
                if (dataGridView1.EditingControl != null)
                {
                    // Obtener el ComboBox que se muestra actualmente en la celda
                    DataGridViewComboBoxEditingControl comboBox = dataGridView1.EditingControl as DataGridViewComboBoxEditingControl;

                    // Mostrar el ComboBox automáticamente
                    comboBox.DroppedDown = true;
                }
            }

           
        }


        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView1.CurrentCell is DataGridViewComboBoxCell && dataGridView1.CurrentCell.OwningColumn.Name == "Estado")
            {
                // Verificar si el control de edición está disponible
                if (e.Control is System.Windows.Forms.ComboBox comboBox)
                {
                    dataGridView1.ClearSelection();

                    // Seleccionar la nueva celda
                    dataGridView1.CurrentCell = dataGridView1.CurrentCell;

                    comboBox.DropDownClosed += comboBox_DropDownClosed;

                    //comboBox.DropDownClosed += comboBox_DropDownClosed;

                    if (!dataGridView1.IsCurrentCellInEditMode)
                    {
                        // Iniciar la edición de la celda actual
                        dataGridView1.BeginEdit(true);
                    }

                }
            }
        }

        //Responisve
        private void Form5_Resize(object sender, EventArgs e)
        {
            int dataWidth = (int)(this.ClientSize.Width * 0.8);
            int dataHeight = (int)(this.ClientSize.Height * 0.8);

            // Alinea el DataGridView en el centro del formulario
            dataGridView1.Width = dataWidth;
            dataGridView1.Height = dataHeight;
            dataGridView1.Location = new Point((this.ClientSize.Width - dataWidth) / 2, (this.ClientSize.Height - dataHeight) / 2 + 20);

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
            checkBox1.Left = dataGridView1.Left + (int)(0.67 * dataWidth) - (int)(0.05 * dataWidth) - 25; // Alinea el CheckBox a la derecha del DataGridView
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

        //Actualizar datos del datagridview
        public async void updateData(object sender, EventArgs e)
        {
            //update = true; 
            await QueryMoodleAndDisplayDataAsync();
            dataGridView1.ClearSelection();
        }

        //Llenar datagridview con datos de la base de datos
        private async Task QueryMoodleAndDisplayDataAsync()
        {
            string testApi = "http://172.17.127.254:8200/webservice/rest/token_table_data.php";

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

                        XmlNodeList nodes = xmlDoc.SelectNodes("/data/row");

                        if (nodes != null && nodes.Count > 0)
                        {
                            dataGridView1.Rows.Clear();

                            int cuenta = -1;
                            foreach (XmlNode row in nodes)
                            {
                                cuenta += 1; 

                                string id = row.SelectSingleNode("id")?.InnerText ?? "";
                                string token = row.SelectSingleNode("token")?.InnerText ?? "";
                                string registration_date = row.SelectSingleNode("registration_date")?.InnerText ?? "";
                                string status = row.SelectSingleNode("status")?.InnerText ?? "";

                                String columna = "";
                                if (comboBox1.SelectedItem != null && (string)comboBox1.SelectedItem != "Escoger Columna" && (string)comboBox1.SelectedItem != " ")
                                {
                                    columna = comboBox1.SelectedItem.ToString();
                                }
                                if (comboBox1.SelectedItem != null)
                                {
                                    int rowIndex;
                                    DataGridViewComboBoxCell comboBoxCell;
                                    switch (comboBox1.SelectedItem.ToString())
                                    {
                                        case "ID":
                                            if (id.Contains(textBox1.Text.ToString()))
                                            {
                                                if (!checkBox1.Checked)
                                                {
                                                    rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                    comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                    if (status.Equals("activo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }
                                                    else if (status.Equals("inactivo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "inactivo";
                                                    }
                                                    else
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "perdido";
                                                    }
                                                }
                                                else
                                                {
                                                    if (status.Equals("activo"))
                                                    {
                                                        rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                        comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }
                                                    
                                                }
                                            }
                                            else if(textBox1.ToString().Contains("Filtrar por "))
                                            {
                                                if (!checkBox1.Checked)
                                                {
                                                    rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                    comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                    if (status.Equals("activo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }
                                                    else if (status.Equals("inactivo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "inactivo";
                                                    }
                                                    else
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "perdido";
                                                    }
                                                }
                                                else
                                                {
                                                    if (status.Equals("activo"))
                                                    {
                                                        rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                        comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }

                                                }
                                            }

                                            break;
                                        case "Token":
                                            if (token.Contains(textBox1.Text.ToString()))
                                            {
                                                if (!checkBox1.Checked)
                                                {
                                                    rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                    comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                    if (status.Equals("activo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }
                                                    else if (status.Equals("inactivo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "inactivo";
                                                    }
                                                    else
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "perdido";
                                                    }
                                                }
                                                else
                                                {
                                                    if (status.Equals("activo"))
                                                    {
                                                        rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                        comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }

                                                }
                                            }
                                            else if (textBox1.ToString().Contains("Filtrar por "))
                                            {
                                                if (!checkBox1.Checked)
                                                {
                                                    rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                    comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                    if (status.Equals("activo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }
                                                    else if (status.Equals("inactivo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "inactivo";
                                                    }
                                                    else
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "perdido";
                                                    }
                                                }
                                                else
                                                {
                                                    if (status.Equals("activo"))
                                                    {
                                                        rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                        comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }

                                                }
                                            }

                                            break;
                                        case "Fecha Registro":

                                            if (registration_date.Contains(textBox1.Text.ToString()))
                                            {
                                                if (!checkBox1.Checked)
                                                {
                                                    rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                    comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                    if (status.Equals("activo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }
                                                    else if (status.Equals("inactivo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "inactivo";
                                                    }
                                                    else
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "perdido";
                                                    }
                                                }
                                                else
                                                {
                                                    if (status.Equals("activo"))
                                                    {
                                                        rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                        comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }

                                                }
                                            }
                                            else if (textBox1.ToString().Contains("Filtrar por "))
                                            {
                                                if (!checkBox1.Checked)
                                                {
                                                    rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                    comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                    if (status.Equals("activo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }
                                                    else if (status.Equals("inactivo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "inactivo";
                                                    }
                                                    else
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "perdido";
                                                    }
                                                }
                                                else
                                                {
                                                    if (status.Equals("activo"))
                                                    {
                                                        rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                        comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }

                                                }
                                            }

                                            break;
                                        case "Estado":
                                            if (status.Contains(textBox1.Text.ToString()))
                                            {
                                                if (!checkBox1.Checked)
                                                {
                                                    rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                    comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                    if (status.Equals("activo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }
                                                    else if (status.Equals("inactivo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "inactivo";
                                                    }
                                                    else
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "perdido";
                                                    }
                                                }
                                                else
                                                {
                                                    if (status.Equals("activo"))
                                                    {
                                                        rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                        comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }

                                                }
                                            }
                                            else if (textBox1.ToString().Contains("Filtrar por "))
                                            {
                                                if (!checkBox1.Checked)
                                                {
                                                    rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                    comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                    if (status.Equals("activo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }
                                                    else if (status.Equals("inactivo"))
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "inactivo";
                                                    }
                                                    else
                                                    {
                                                        comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                        comboBoxCell.Value = "perdido";
                                                    }
                                                }
                                                else
                                                {
                                                    if (status.Equals("activo"))
                                                    {
                                                        rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                        comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                        comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                        comboBoxCell.Value = "activo";
                                                    }

                                                }
                                            }
                                            break;                                        
                                        default:
                                            if (!checkBox1.Checked)
                                            {
                                                rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                if (status.Equals("activo"))
                                                {
                                                    comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                    comboBoxCell.Value = "activo";
                                                }
                                                else if (status.Equals("inactivo"))
                                                {
                                                    comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                    comboBoxCell.Value = "inactivo";
                                                }
                                                else
                                                {
                                                    comboBoxCell.Items.AddRange("inactivo", "perdido");
                                                    comboBoxCell.Value = "perdido";
                                                }
                                            }
                                            else
                                            {
                                                if (status.Equals("activo"))
                                                {
                                                    rowIndex = dataGridView1.Rows.Add(id, token, registration_date);
                                                    comboBoxCell = dataGridView1.Rows[rowIndex].Cells["Estado"] as DataGridViewComboBoxCell;

                                                    comboBoxCell.Items.AddRange("inactivo", "perdido", "activo");
                                                    comboBoxCell.Value = "activo";
                                                }

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

        //Cambiar valor combobox del datagridview
        private async void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            // Verificar si el ComboBox se cambió manualmente
            if (!update && !isProcessing)
            {
                isProcessing = true;
                this.ActiveControl = null;
                DataGridViewComboBoxEditingControl comboBox = sender as DataGridViewComboBoxEditingControl;

                if (comboBox != null)
                {
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                    string tokenValue = dataGridView1.Rows[rowIndex].Cells["Token"].Value.ToString();
                    string newValue = comboBox.SelectedItem.ToString();
                    string oldValue = dataGridView1.Rows[rowIndex].Cells["Estado"].Value.ToString();

                    if ((newValue == "activo" || newValue == "inactivo" || newValue == "perdido") && newValue != oldValue && !isMessageBoxShown)
                    {
                        isMessageBoxShown = true;
                        // Mostrar un mensaje de confirmación
                        DialogResult result = MessageBox.Show("¿Estás seguro de cambiar el valor?", "Confirmar cambio", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            try
                            {

                                using (HttpClient client = new HttpClient())
                                {
                                    string url = "http://172.17.127.254:8200/webservice/rest/update_status.php";

                                    string postData = $"token={tokenValue}&newValue={newValue}&oldValue={oldValue}";
                                    StringContent content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");

                                    // Realizar la solicitud GET al servidor
                                    HttpResponseMessage response = await client.PostAsync(url, content);

                                    // Verificar si la solicitud fue exitosa
                                    if (response.IsSuccessStatusCode)
                                    {
                                        MessageBox.Show("Datos enviados correctamente al servidor. " + response.StatusCode);

                                        dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error al cambiar el estado. Código de estado HTTP: " + response.Content);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: " + ex.Message);
                            }
                        }
                        else
                        {
                            await QueryMoodleAndDisplayDataAsync();
                        }
                        isMessageBoxShown = false;
                        dataGridView1.ClearSelection();
                    }
                    isProcessing = false;

                    // Volver a enfocar el control anterior
                    if (previousControl != null)
                    {
                        previousControl.Focus();
                        previousControl = null;
                    }
                }
            }
            else
            {
                update = false;
            }
        }

       
        //Mostrar filtros del combobox
        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            if ((string)comboBox1.SelectedItem == "Escoger Columna")
            {
                comboBox1.Items.Clear();
                comboBox1.ForeColor = SystemColors.WindowText;
                comboBox1.Items.Add(" ");
                comboBox1.Items.Add("ID");
                comboBox1.Items.Add("Token");
                comboBox1.Items.Add("Fecha Registro");
                comboBox1.Items.Add("Estado");
            }

        }

        //Poner el texto por defecto del combobox
        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            if ((string)comboBox1.SelectedItem == " " || comboBox1.SelectedIndex == -1)
            {
                comboBox1.Items.Clear();
                comboBox1.ForeColor = SystemColors.GrayText;
                comboBox1.Items.Add("Escoger Columna");
                comboBox1.SelectedIndex = 0;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains("Filtrar por"))
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

        //Cargar datos al abrir form
        private void Form5_Load_1(object sender, EventArgs e)
        {
            updateData(sender, e);
        }

        //Volver al menú
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();

            Form4 form4 = new Form4();
            form4.ShowDialog();

            this.Close();
        }

        //Actualizar los datos
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox1.ForeColor = SystemColors.GrayText;
            comboBox1_DropDownClosed(sender, e);

            textBox1.Text = "Filtrar por Columna";
            textBox1.ForeColor = SystemColors.GrayText;
            pictureBox1.Focus();

            dataGridView1.CurrentCell.Selected = false;
            updateData(sender, e);
        }

        //Cambiar valor de filtro de la columna
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
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

        //Actualizar segun cambie el filtro del textbox
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            updateData(sender, e);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //Abrir ventana de añadir token
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(this);
            form6.ShowDialog();
        }

        //Actualizar datos al cambiar el checkbox
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            updateData(sender, e);
        }
    }
}
