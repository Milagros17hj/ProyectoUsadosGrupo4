namespace ProyectoUsadosGrupo4
{
    partial class frmManualUsuario
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.btnManualUsuario = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnManualUsuario
            // 
            this.btnManualUsuario.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnManualUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManualUsuario.Location = new System.Drawing.Point(115, 128);
            this.btnManualUsuario.Name = "btnManualUsuario";
            this.btnManualUsuario.Size = new System.Drawing.Size(147, 131);
            this.btnManualUsuario.TabIndex = 1;
            this.btnManualUsuario.Text = "Manual de Usuario";
            this.btnManualUsuario.UseVisualStyleBackColor = false;
            this.btnManualUsuario.Click += new System.EventHandler(this.btnManualUsuario_Click);
            // 
            // frmManualUsuario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 387);
            this.Controls.Add(this.btnManualUsuario);
            this.Name = "frmManualUsuario";
            this.Text = "frmManualUsuario";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.Button btnManualUsuario;
    }
}