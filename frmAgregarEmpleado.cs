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
        string idTipoIdentificacion;
        string idProvincia;
        string sexo;
        bool cargandoDatos = false;


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
            limpiar();
        }
        private void limpiar()
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
            txtTeléfono.Clear();


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
        
        private void cmbRol_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                string cmd = string.Format("SELECT id_rol FROM Rol WHERE nombre = '{0}'", cmbRol.Text);
                ds = Utilidades.ejecutar(cmd);
                idRol = ds.Tables[0].Rows[0]["id_rol"].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar rol: 002" + ex.Message);
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
            grabar();
        }
        public void grabar()
        { 
            try
            {
                string fechaSistema;      
                string contraseña;
                int estado = 1;       
                int es_empleado = 1;

                fechaSistema = DateTime.Now.ToString("MM/dd/yyyy");
                contraseña = Utilidades.codificar(txtContraseña.Text);

                if (txtContraseña.Text == txtConfirmar.Text)
                {
                    // PARA LA TABLA EMPLEADOS 
                    string cmdEmpleado = string.Format(
                        "INSERT INTO Empleado (id_tipo_identificacion, numero_identificacion, nombre, primer_apellido, segundo_apellido, sexo, fecha_nacimiento, fecha_vencimiento_doc, email, telefono, id_provincia, canton, distrito, id_rol, id_estado) " +
                        "VALUES ({0}, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', {10}, '{11}', '{12}', {13}, {14})",
                        idTipoIdentificacion,
                        txtIdentificación.Text.Trim(),
                        txtNombre.Text.Trim(),
                        txtApellido1.Text.Trim(),
                        txtApellido2.Text.Trim(),
                        sexo,
                        dtpNacimiento.Value.ToString("yyyy-MM-dd"),
                        dtpVencimiento.Value.ToString("yyyy-MM-dd"),
                        txtEmail.Text.Trim(),
                        txtTeléfono.Text.Trim(),
                        idProvincia,
                        txtCanton.Text.Trim(),
                        txtDistrito.Text.Trim(),
                        idRol,
                        estado
                        );

                    Utilidades.ejecutar(cmdEmpleado);

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

                    MessageBox.Show("Empleado y usuario creados satisfactoriamente", "Guardar",
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
                string cmd = string.Format("select a.numero_identificacion as Cedula, a.nombre, a.primer_apellido,  a.segundo_apellido,  a.sexo,  a.fecha_nacimiento," +
                    "a.fecha_vencimiento_doc, b.nombre as Rol,  c.email as UsuarioEmail, c.id_estado as EstadoUsuario, c.fec_creacion, c.fec_modificacion " +
                    "from Empleado a, Rol b, Usuario c " +
                    "where a.numero_identificacion = '{0}' and a.id_rol = b.id_rol and a.numero_identificacion = c.numero_identificacion", txtIdentificación.Text.Trim());

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

        private void ckbEstado_CheckedChanged_1(object sender, EventArgs e)
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
                valor = 2; // 2 ES INHABILITADO EN LA DB
                cmd = string.Format("UPDATE Usuario SET id_estado = {0}, fec_modificacion = '{1}' where numero_identificacion = '{2}'",
                valor, fechaSistema, txtIdentificación.Text.Trim());
                Utilidades.ejecutar(cmd);

                MessageBox.Show("Se inhabilitó satisfactoriamente al usuario", "Actualización",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargar();
            }
            else
            { 
                valor = 1; // 1 ES HABILITADO EN LA DB
                cmd = string.Format("UPDATE Usuario SET id_estado = {0}, fec_modificacion = '{1}' WHERE numero_identificacion = '{2}'",
                valor, fechaSistema, txtIdentificación.Text.Trim());
                Utilidades.ejecutar(cmd);

                MessageBox.Show("Se habilitó satisfactoriamente al Usuario", "Actualización",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargar();

            }

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                string cmd = string.Format(
                    "SELECT a.numero_identificacion as Cedula, a.nombre, a.primer_apellido, a.segundo_apellido, a.sexo,a.fecha_nacimiento, a.fecha_vencimiento_doc, " +
                    " b.nombre as Rol, c.email AS UsuarioEmail, c.id_estado AS EstadoUsuario, c.fec_creacion, c.fec_modificacion " +
                    "from Empleado a, Rol b, Usuario c where a.nombre LIKE '%{0}%' and a.id_rol = b.id_rol and a.numero_identificacion = c.numero_identificacion",
                    txtNombre.Text.Trim());

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
                if (string.IsNullOrWhiteSpace(txtIdentificación.Text))
                {
                    MessageBox.Show("Debe ingresar el número de identificación para eliminar.", "Eliminar",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtIdentificación.Focus();
                    return;
                }

                //SE ELIMINA DE LA TABLA DE USUARIOS
                string cmdUsuario = string.Format(
                    "DELETE FROM Usuario WHERE numero_identificacion = '{0}'",
                    txtIdentificación.Text.Trim()
                );
                Utilidades.ejecutar(cmdUsuario);

                // SE ELIMINA DE LA TABLA EMPLEADOS
                string cmdEmpleado = string.Format(
                    "DELETE FROM Empleado WHERE numero_identificacion = '{0}'",
                    txtIdentificación.Text.Trim()
                );
                Utilidades.ejecutar(cmdEmpleado);

                MessageBox.Show("Se a eliminado al usuario satisfactoriamente", "Eliminar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                cargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al eliminar: 006 " + ex.Message);
            }
        }
    }
    
}
