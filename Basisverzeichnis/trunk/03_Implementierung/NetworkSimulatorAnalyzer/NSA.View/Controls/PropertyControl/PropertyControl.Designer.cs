﻿namespace NSA.View.Controls.PropertyControl
{
    partial class PropertyControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.flpContents = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flpContents
            // 
            this.flpContents.AutoScroll = true;
            this.flpContents.BackColor = System.Drawing.SystemColors.Control;
            this.flpContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpContents.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpContents.Location = new System.Drawing.Point(0, 0);
            this.flpContents.Name = "flpContents";
            this.flpContents.Size = new System.Drawing.Size(233, 382);
            this.flpContents.TabIndex = 0;
            this.flpContents.WrapContents = false;
            this.flpContents.Scroll += new System.Windows.Forms.ScrollEventHandler(this.flpContents_Scroll);
            // 
            // PropertyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.flpContents);
            this.DoubleBuffered = true;
            this.Name = "PropertyControl";
            this.Size = new System.Drawing.Size(233, 382);
            this.ResumeLayout(false);

        }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel flpContents;
    }
}
