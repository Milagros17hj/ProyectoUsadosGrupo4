using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using libreriaIII2025;

namespace ProyectoUsadosGrupo4
{
    public partial class frmlogin : Form
    {
        public DataSet ds;

        public frmlogin()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                string contraseña = Utilidades.codificar(txtClave.Text.Trim());

                string cmd = string.Format(
                    "SELECT numero_identificacion, contrasena, es_empleado, id_estado " +
                    "FROM Usuario WHERE email = '{0}' AND contrasena = '{1}'",
                    txtCorreo.Text.Trim(), contraseña);

                DataSet ds = Utilidades.ejecutar(cmd);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Correo o contraseña incorrectos. Por favor verifique...", "Ingreso a Sistema",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    limpiar();
                    return;
                }

                DataRow fila = ds.Tables[0].Rows[0];
                int estado = Convert.ToInt32(fila["id_estado"]);

                if (estado != 1) // 1 = habilitado
                {
                    MessageBox.Show("El usuario está inhabilitado.", "Ingreso a Sistema",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string identificacion = fila["numero_identificacion"].ToString();
                int esEmpleado = Convert.ToInt32(fila["es_empleado"]);

                // Si es empleado 
                if (esEmpleado == 1)
                {
                    string cmdRol = string.Format(
                        "SELECT id_rol FROM Empleado WHERE numero_identificacion = '{0}'",
                        identificacion);

                    DataSet dsRol = Utilidades.ejecutar(cmdRol);

                    if (dsRol.Tables[0].Rows.Count > 0)
                    {
                        Sesiones.Rol = Convert.ToInt32(dsRol.Tables[0].Rows[0]["id_rol"]);
                    }
                }
                else
                {
                    Sesiones.Rol = 5; // Cliente
                }

                Sesiones.Usuario = txtCorreo.Text.Trim();

                MessageBox.Show("Bienvenido al sistema.", "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                frmMenu menu = new frmMenu();
                menu.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en el login: " + ex.Message, "Ingreso a Sistema",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                limpiar();
            }
        }


        public void limpiar()
        {
            txtCorreo.Clear();
            txtClave.Clear();
            txtCorreo.Focus();
        }
    }

}
