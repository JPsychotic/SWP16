﻿using NSA.View.Controls.NetworkView;

namespace NSA.View.Forms
{
  partial class MainForm
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

    #region Vom Windows Form-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.propertyControl = new NSA.View.Controls.PropertyControl.PropertyControl();
            this.ToolbarControl = new NSA.View.Controls.Toolbar.ToolbarControl();
            this.infoControl = new NSA.View.Controls.InfoControl.InfoControl();
            this.networkViewControl = new NSA.View.Controls.NetworkView.NetworkViewControl();
            this.SuspendLayout();
            // 
            // propertyControl
            // 
            this.propertyControl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.propertyControl.AutoScroll = true;
            this.propertyControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.propertyControl.Location = new System.Drawing.Point(796, 73);
            this.propertyControl.Name = "propertyControl";
            this.propertyControl.Size = new System.Drawing.Size(243, 584);
            this.propertyControl.TabIndex = 3;
            this.propertyControl.Tag = "PropertyControl";
            // 
            // ToolbarControl
            // 
            this.ToolbarControl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ToolbarControl.AutoSize = true;
            this.ToolbarControl.Location = new System.Drawing.Point(12, 12);
            this.ToolbarControl.Name = "ToolbarControl";
            this.ToolbarControl.Size = new System.Drawing.Size(1013, 55);
            this.ToolbarControl.TabIndex = 2;
            this.ToolbarControl.Tag = "ToolbarControl";
            // 
            // infoControl
            // 
            this.infoControl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.infoControl.Location = new System.Drawing.Point(12, 440);
            this.infoControl.Name = "infoControl";
            this.infoControl.Size = new System.Drawing.Size(778, 217);
            this.infoControl.TabIndex = 1;
            this.infoControl.Tag = "InfoControl";
            // 
            // networkViewControl
            // 
            this.networkViewControl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.networkViewControl.BackColor = System.Drawing.SystemColors.Control;
            this.networkViewControl.Location = new System.Drawing.Point(12, 73);
            this.networkViewControl.Name = "networkViewControl";
            this.networkViewControl.Size = new System.Drawing.Size(778, 361);
            this.networkViewControl.TabIndex = 0;
            this.networkViewControl.Tag = "NetworkviewControl";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1051, 669);
            this.Controls.Add(this.propertyControl);
            this.Controls.Add(this.ToolbarControl);
            this.Controls.Add(this.infoControl);
            this.Controls.Add(this.networkViewControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(880, 500);
            this.Name = "MainForm";
            this.Text = "Network Simulator and Analyzer";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private NetworkViewControl networkViewControl;
    private Controls.InfoControl.InfoControl infoControl;
    private Controls.Toolbar.ToolbarControl ToolbarControl;
        private Controls.PropertyControl.PropertyControl propertyControl;
    }
}

