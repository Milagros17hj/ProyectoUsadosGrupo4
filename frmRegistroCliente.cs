using libreriaIII2025;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoUsadosGrupo4
{
    public partial class frmRegistroCliente : Form
    {
        public DataSet ds;                
        string idTipoIdentificacion;      
        string idProvincia;              
        string sexo;                      
        bool cargandoDatos = false;
        public frmRegistroCliente()
        {
            InitializeComponent();
        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();

        }
        private void limpiar()
        {

            ckbEstado.CheckedChanged -= ckbEstado_CheckedChanged;

            txtNombre.Clear();
            txtApellido1.Clear();
            txtApellido2.Clear();
            txtIdentificación.Clear();
            txtEmail.Clear();
            txtCanton.Clear();
            txtDistrito.Clear();
            txtTeléfono.Clear();
            txtContraseña.Clear();
            txtConfirmar.Clear();
            dgvDatos.DataSource = null;
            cmbIdentificación.SelectedItem = null;
            cmbSexo.SelectedItem = null;
            cmbProvincia.SelectedItem = null;
            dtpVencimiento.Value = DateTime.Now;
            dtpNacimiento.Value = DateTime.Now;
            ckbEstado.Checked = false;
            ckbEstado.Enabled = false; //PARA QUE ELL CLIENTE NO PUEDA CAMBIAR EL ESTADO

            // perfil siemmpre fijo porque es cliente
            cmbRol.SelectedItem = "Cliente";
            cmbRol.Enabled = false;


            if (ds != null) ds.Clear();
            txtNombre.Focus();

            ckbEstado.CheckedChanged += ckbEstado_CheckedChanged;
        }

        private void frmRegistroCliente_Load(object sender, EventArgs e)
        {
            try
            {
               
                string cmdTipo = "SELECT id_tipo, descripcion FROM TipoIdentificacion";
                ds = Utilidades.ejecutar(cmdTipo);
                cmbIdentificación.DataSource = ds.Tables[0].DefaultView;
                cmbIdentificación.DisplayMember = "descripcion";
                cmbIdentificación.ValueMember = "id_tipo";
                cmbIdentificación.SelectedItem = null;

               
                string cmdProv = "SELECT id_provincia, nombre FROM Provincia";
                ds = Utilidades.ejecutar(cmdProv);
                cmbProvincia.DataSource = ds.Tables[0].DefaultView;
                cmbProvincia.DisplayMember = "nombre";
                cmbProvincia.ValueMember = "id_provincia";
                cmbProvincia.SelectedItem = null;

                
                cmbRol.Items.Clear();
                cmbRol.Items.Add("Cliente");
                cmbRol.SelectedIndex = 0;
                cmbRol.Enabled = false; // no se puede cambiar


                if (Sesiones.Rol == 1) // Administrador
                {
                    btnConsultar.Enabled = true;
                    btnEliminar.Enabled = true;

                }
                else if (Sesiones.Rol == 5) // Cliente
                {
                    btnConsultar.Enabled = false;
                    btnEliminar.Enabled = false;

                }
                else
                {
                    // Otros roles 
                    btnConsultar.Enabled = true;
                    btnEliminar.Enabled = false;
 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los catálogos: 001 " + ex.Message);
            }

        }

        private void cmbIdentificación_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                string cmd = string.Format("SELECT id_tipo FROM TipoIdentificacion WHERE descripcion = '{0}'", cmbIdentificación.Text);
                ds = Utilidades.ejecutar(cmd);
                idTipoIdentificacion = ds.Tables[0].Rows[0]["id_tipo"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar tipo de identificación: 002 " + ex.Message);
            }

        }

        private void cmbProvincia_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                string cmd = string.Format("SELECT id_provincia FROM Provincia WHERE nombre = '{0}'", cmbProvincia.Text);
                ds = Utilidades.ejecutar(cmd);
                idProvincia = ds.Tables[0].Rows[0]["id_provincia"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar provincia: 002" + ex.Message);
            }
        }

        private void cmbSexo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                sexo = cmbSexo.SelectedItem.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar sexo:002" + ex.Message);
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Grabar();
        }

        public void Grabar()
        {
            try
            {
                string fechaSistema;
                string contraseña;
                int estado = 1;       
                int es_empleado = 0;  

                fechaSistema = DateTime.Now.ToString("MM/dd/yyyy");
                contraseña = Utilidades.codificar(txtContraseña.Text);

                if (txtContraseña.Text == txtConfirmar.Text)
                {
                    // PARA LA TABLA CLIENTES
                    string cmdCliente = string.Format(
                        "INSERT INTO Cliente (id_tipo_identificacion, numero_identificacion, nombre, primer_apellido, segundo_apellido, email, telefono, id_provincia, canton, distrito, id_estado) " +
                        "VALUES ({0}, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', {7}, '{8}', '{9}', {10})",
                        idTipoIdentificacion,
                        txtIdentificación.Text.Trim(),
                        txtNombre.Text.Trim(),
                        txtApellido1.Text.Trim(),
                        txtApellido2.Text.Trim(),
                        txtEmail.Text.Trim(),
                        txtTeléfono.Text.Trim(),
                        idProvincia,
                        txtCanton.Text.Trim(),
                        txtDistrito.Text.Trim(),
                        estado
                    );

                    Utilidades.ejecutar(cmdCliente);

                    // PARA LA TABLA USUARIOS
                    string cmdUsuario = string.Format(
                        "INSERT INTO Usuario (numero_identificacion, contrasena, email, es_empleado, id_estado, fec_creacion, fec_modificacion) " +
                        "VALUES ('{0}', '{1}', '{2}', {3}, {4}, '{5}', '{6}')",
                        txtIdentificación.Text.Trim(),
                        contraseña,
                        txtEmail.Text.Trim(),
                        es_empleado,
                        estado,
                        fechaSistema,
                        fechaSistema
                    );

                    Utilidades.ejecutar(cmdUsuario);

                    MessageBox.Show("Cliente y usuario creados satisfactoriamente", "Guardar",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    cargar();

                }
                else
                {
                    MessageBox.Show("Las contraseñas no coinciden, por favor verificar...", "Error de Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtContraseña.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error: 003 " + ex.Message);
            }
        }

        public void cargar()
        {
            try
            {
                string cmd = string.Format("select a.numero_identificacion as Cedula, a.nombre, a.primer_apellido, a.segundo_apellido, " +
                    "a.email, a.telefono, b.nombre as Provincia, c.email as UsuarioEmail, c.id_estado as EstadoUsuario, " +
                    "c.fec_creacion, c.fec_modificacion " +
                    "from Cliente a, Provincia b, Usuario c " +
                    "where a.id_provincia = b.id_provincia and a.numero_identificacion = c.numero_identificacion and a.numero_identificacion = '{0}'", txtIdentificación.Text.Trim());


                ds = Utilidades.ejecutar(cmd);
                dgvDatos.DataSource = ds.Tables[0].DefaultView;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtIdentificación.Text = ds.Tables[0].Rows[0]["Cedula"].ToString();
                    int estadoUsuario = Convert.ToInt32(ds.Tables[0].Rows[0]["EstadoUsuario"]);
                    cargandoDatos = true;
                    ckbEstado.Checked = (estadoUsuario == 2); 
                    cargandoDatos = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error: 004 " + ex.Message);
            }
        }

        private void ckbEstado_CheckedChanged(object sender, EventArgs e)
        {
            string cmd;
            int valor;
            string fechaSistema = DateTime.Now.ToString("MM/dd/yyyy");

            if (cargandoDatos) return;

            if (string.IsNullOrWhiteSpace(txtIdentificación.Text))
            {
                MessageBox.Show("Debe de agregar una identificación para la habilitación / inhabilitación", "Actualización",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIdentificación.Focus();
                return;
            }

            string validar = string.Format("SELECT COUNT(*) FROM Usuario WHERE numero_identificacion = '{0}'", txtIdentificación.Text.Trim());
            DataSet dsValidar = Utilidades.ejecutar(validar);
            int existe = Convert.ToInt32(dsValidar.Tables[0].Rows[0][0]);

            if (existe == 0)
            {
                MessageBox.Show("La identificación no existe", "Actualización",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                limpiar();
                return;
            }

            if (ckbEstado.Checked == true)
            {
                valor = 2; // Inhabilitado
                cmd = string.Format("UPDATE Usuario SET id_estado = {0}, fec_modificacion = '{1}' WHERE numero_identificacion = '{2}'",
                    valor, fechaSistema, txtIdentificación.Text.Trim());
                Utilidades.ejecutar(cmd);

                MessageBox.Show("Se inhabilitó satisfactoriamente al usuario", "Actualización",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargar();
            }
            else
            {
                valor = 1; // Habilitado
                cmd = string.Format("UPDATE Usuario SET id_estado = {0}, fec_modificacion = '{1}' WHERE numero_identificacion = '{2}'",
                    valor, fechaSistema, txtIdentificación.Text.Trim());
                Utilidades.ejecutar(cmd);

                MessageBox.Show("Se habilitó satisfactoriamente al usuario", "Actualización",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargar();
            }

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                // SOLO ADMI PUEDE CONSULTAR
                if (Sesiones.Rol != 1) // 1 = Admin
                {
                    MessageBox.Show("No tiene permisos para consultar", "Permisos",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string cmd = string.Format(
                    "select a.numero_identificacion as Cedula, a.nombre, a.primer_apellido, a.segundo_apellido, " +
                    "a.email, a.telefono, b.nombre as Provincia, c.email as UsuarioEmail, c.id_estado as EstadoUsuario, " +
                    "c.fec_creacion, c.fec_modificacion " +
                    "from Cliente a, Provincia b, Usuario c " +
                    "where a.nombre LIKE '%{0}%' " +
                    "and a.id_provincia = b.id_provincia " +
                    "and a.numero_identificacion = c.numero_identificacion",
                    txtNombre.Text.Trim());

                ds = Utilidades.ejecutar(cmd);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dgvDatos.DataSource = ds.Tables[0].DefaultView;

                    txtIdentificación.Text = ds.Tables[0].Rows[0]["Cedula"].ToString();
                    int estadoUsuario = Convert.ToInt32(ds.Tables[0].Rows[0]["EstadoUsuario"]);
                    cargandoDatos = true;
                    ckbEstado.Checked = (estadoUsuario == 2); 
                    cargandoDatos = false;
                }
                else
                {
                    MessageBox.Show("El cliente no existe en el sistema.", "Consulta",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al consultar 005 " + ex.Message);
            }
        }




        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        public void eliminar()
        {
            try
            {
                // sOLO ADMI PUEDE ELIMINAR
                if (Sesiones.Rol != 1) // 1 = Admin
                {
                    MessageBox.Show("No tiene permisos para eliminar", "Permisos",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtIdentificación.Text))
                {
                    MessageBox.Show("Debe ingresar el número de identificación para eliminar.", "Eliminar",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtIdentificación.Focus();
                    return;
                }

                // SE ELIMINA DE LA TABLA USUARIOS
                string cmdUsuario = string.Format(
                    "DELETE FROM Usuario WHERE numero_identificacion = '{0}'",
                    txtIdentificación.Text.Trim()
                );
                Utilidades.ejecutar(cmdUsuario);

                // SE ELIMINA DE LA TABLA CLIENTES
                string cmdCliente = string.Format(
                    "DELETE FROM Cliente WHERE numero_identificacion = '{0}'",
                    txtIdentificación.Text.Trim()
                );
                Utilidades.ejecutar(cmdCliente);

                MessageBox.Show("Se ha eliminado al cliente satisfactoriamente", "Eliminar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                limpiar();
                cargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al eliminar: 006 " + ex.Message);
            }
        }

    }
}
