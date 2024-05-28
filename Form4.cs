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
using System.Reflection.Emit;

namespace RFIDMantenimiento
{
    public partial class Form4 : Form
    {
        private System.Windows.Forms.ToolTip toolTip1;

        public Form4()
        {
            InitializeComponent();

            toolTip1 = new System.Windows.Forms.ToolTip();
            SetToolTips();

            this.Shown += Form4_Shown;
            this.MinimumSize = new Size(750, 490);
            this.Resize += new EventHandler(Form4_Resize);
        }

        private void SetToolTips()
        {
            toolTip1.SetToolTip(button1, "Gestión de usuarios");
            toolTip1.SetToolTip(button2, "Gestión de tokens");
        }

        private void Form4_Resize(object sender, EventArgs e)
        {
            int anchura = this.ClientSize.Width;
            int altura = this.ClientSize.Height;

            //MAIN BUTTONS
            // Calcular el espacio entre el borde superior e inferior del formulario y los botones
            int espacioVertical = (int)(altura * 0.25); // 25% de la altura del formulario

            // Calcular la altura disponible para los botones
            int alturaDisponible = altura - 2 * espacioVertical;

            // Calcular la altura de cada botón (la mitad de la altura disponible)
            int alturaBoton = alturaDisponible;

            // Calcular el ancho de cada botón
            int anchoBoton = (int)(altura * 0.4); // Restamos los espacios y la separación

            // Alinear los botones verticalmente en el centro del formulario
            int posY = (int)(espacioVertical * 1.3) + (alturaDisponible - alturaBoton);

            //LOGOUT BUTTON
            int alturaLogout = (int)(altura * 0.10);

            //LOGO MONLAU
            int alturaLogo = (int)(altura * 0.16);

            //TITULO
            int tamañoLetra = (int)(20 + (altura * 0.05));

            //CONDICION DE ANCHURA
            if (anchura < 1000)
            {
                if (alturaLogout > 70)
                {
                    alturaLogout = 70;
                }

                if (alturaLogo > 110)
                {
                    alturaLogo = 110;
                }

                if (anchoBoton > 280)
                {
                    anchoBoton = 280;
                    alturaBoton = 350;
                }

                if (tamañoLetra > 40)
                {
                    tamañoLetra = 40;
                }
            }
            else if (anchura < 1500)
            {
                if (alturaLogout > 105)
                {
                    alturaLogout = 105;
                }

                if (alturaLogo > 165)
                {
                    alturaLogo = 165;
                }

                if (anchoBoton > 420)
                {
                    anchoBoton = 420;
                    alturaBoton = 525;
                }

                if (tamañoLetra > 52)
                {
                    tamañoLetra = 52;
                }
            }
            else
            {
                if (tamañoLetra > 80)
                {
                    tamañoLetra = 80;
                }
            }

            button3.Size = new Size(alturaLogout, alturaLogout);
            button3.Location = new Point((int)(this.ClientSize.Width - alturaLogout * 1.4), (int)(alturaLogout * 0.4));

            pictureBox1.Size = new Size(alturaLogo, alturaLogo);
            pictureBox1.Location = new Point((int)(alturaLogo * 0.25), (int)(alturaLogo * 0.25));

            label3.Font = new Font(label3.Font.FontFamily, tamañoLetra, label3.Font.Style);
            label3.Location = new Point((int)(anchura * 0.5 - label3.Width * 0.5 + alturaLogo * 0.08), (int)(alturaLogo * 0.3));

            // Establecer la posición y tamaño del primer botón (izquierdo)
            button1.Size = new Size(anchoBoton, alturaBoton);
            button1.Location = new Point((int)(this.ClientSize.Width * 0.5) - anchoBoton - 7, posY);

            // Establecer la posición y tamaño del segundo botón (derecho)
            button2.Size = new Size(anchoBoton, alturaBoton);
            button2.Location = new Point((int)(this.ClientSize.Width * 0.5) + 7, posY);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();

            Form2 form2 = new Form2();
            form2.ShowDialog();

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            Form5 form5 = new Form5();
            form5.ShowDialog();

            this.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void Form4_Shown(object sender, EventArgs e)
        {
            // Establecer el control activo como null después de que el formulario se ha mostrado
            this.ActiveControl = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();

            Form1 form1 = new Form1();
            form1.ShowDialog();

            this.Close();
        }
    }
}
