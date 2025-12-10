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
    public partial class frmAgregarEmpleado : Form
    {
        public DataSet ds;
        string idRol;
        public frmAgregarEmpleado()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Clear();
            txtApellido1.Clear();
            txtApellido2.Clear();
            cmbIdentificación.SelectedItem = null;
            txtIdentificación.Clear();
            dtpVencimiento.Value = DateTime.Now;
            dtpNacimiento.Value = DateTime.Now;
            cmbSexo.SelectedItem = null;
            txtEmail.Clear();
            cmbProvincia.SelectedItem = null;
            txtCanton.Clear();
            txtDistrito.Clear();
            txtTelefóno.Clear();


            cmbRol.SelectedItem = null;
            txtContraseña.Clear();
            txtConfirmar.Clear();
            ckbEstado.Checked = false;

            if (ds != null) ds.Clear();

            txtNombre.Focus();
        }

        private void frmAgregarEmpleado_Load(object sender, EventArgs e)
        {
            try
            {
                // roles
                string cmdRol = "SELECT id_rol, nombre FROM Rol";
                ds = Utilidades.ejecutar(cmdRol);
                cmbRol.DataSource = ds.Tables[0].DefaultView;
                cmbRol.DisplayMember = "nombre";   
                cmbRol.ValueMember = "id_rol";       
                cmbRol.SelectedItem = null;

                // Catálogo de Tipo de Identificación
                string cmdTipo = "SELECT id_tipo, descripcion FROM TipoIdentificacion";
                ds = Utilidades.ejecutar(cmdTipo);
                cmbIdentificación.DataSource = ds.Tables[0].DefaultView;
                cmbIdentificación.DisplayMember = "descripcion";
                cmbIdentificación.ValueMember = "id_tipo";
                cmbIdentificación.SelectedItem = null;

                // Provincias
                string cmdProv = "SELECT id_provincia, nombre FROM Provincia";
                ds = Utilidades.ejecutar(cmdProv);
                cmbProvincia.DataSource = ds.Tables[0].DefaultView;
                cmbProvincia.DisplayMember = "nombre";
                cmbProvincia.ValueMember = "id_provincia";
                cmbProvincia.SelectedItem = null;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los catálogos: 001" + ex.Message);
            }
        }


        private void btnGrabar_Click(object sender, EventArgs e)
        {

        }
        public void grabar()
        {

        }
    }
}
