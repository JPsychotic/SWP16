﻿using System.Windows.Forms;
using NSA.View.Controls.InfoControl.ControlContents;

namespace NSA.View.Controls.InfoControl
{
    partial class InfoControl
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
            this.TcPages = new System.Windows.Forms.TabControl();
            this.tabPageHistory = new System.Windows.Forms.TabPage();
            this.historyControl = new NSA.View.Controls.InfoControl.ControlContents.HistoryControl();
            this.tabPageHops = new System.Windows.Forms.TabPage();
            this.hopsControl = new NSA.View.Controls.InfoControl.ControlContents.HopsControl();
            this.tabPageHopVisualization = new System.Windows.Forms.TabPage();
            this.hopVisualizationControl = new NSA.View.Controls.InfoControl.ControlContents.HopVisualizationControl();
            this.tabPageScenarios = new System.Windows.Forms.TabPage();
            this.scenariosControl = new NSA.View.Controls.InfoControl.ControlContents.ScenariosControl();
            this.tabPageResults = new System.Windows.Forms.TabPage();
            this.resultsControl = new NSA.View.Controls.InfoControl.ControlContents.ResultsControl();
            this.TcPages.SuspendLayout();
            this.tabPageHistory.SuspendLayout();
            this.tabPageHops.SuspendLayout();
            this.tabPageHopVisualization.SuspendLayout();
            this.tabPageScenarios.SuspendLayout();
            this.tabPageResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcPages
            // 
            this.TcPages.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.TcPages.Controls.Add(this.tabPageHistory);
            this.TcPages.Controls.Add(this.tabPageHops);
            this.TcPages.Controls.Add(this.tabPageHopVisualization);
            this.TcPages.Controls.Add(this.tabPageScenarios);
            this.TcPages.Controls.Add(this.tabPageResults);
            this.TcPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TcPages.Location = new System.Drawing.Point(0, 0);
            this.TcPages.Multiline = true;
            this.TcPages.Name = "TcPages";
            this.TcPages.SelectedIndex = 0;
            this.TcPages.Size = new System.Drawing.Size(684, 195);
            this.TcPages.TabIndex = 0;
            this.TcPages.Selected += new System.Windows.Forms.TabControlEventHandler(this.tcPages_Selected);
            this.TcPages.Deselected += new System.Windows.Forms.TabControlEventHandler(this.tcPages_Deselected);
            // 
            // tabPageHistory
            // 
            this.tabPageHistory.Controls.Add(this.historyControl);
            this.tabPageHistory.Location = new System.Drawing.Point(4, 4);
            this.tabPageHistory.Name = "tabPageHistory";
            this.tabPageHistory.Size = new System.Drawing.Size(676, 169);
            this.tabPageHistory.TabIndex = 0;
            this.tabPageHistory.Text = "Verlauf";
            this.tabPageHistory.UseVisualStyleBackColor = true;
            // 
            // historyControl
            // 
            this.historyControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historyControl.Location = new System.Drawing.Point(0, 0);
            this.historyControl.Name = "historyControl";
            this.historyControl.Size = new System.Drawing.Size(676, 169);
            this.historyControl.TabIndex = 0;
            this.historyControl.Tag = "HistoryTabPage";
            // 
            // tabPageHops
            // 
            this.tabPageHops.Controls.Add(this.hopsControl);
            this.tabPageHops.Location = new System.Drawing.Point(4, 4);
            this.tabPageHops.Name = "tabPageHops";
            this.tabPageHops.Size = new System.Drawing.Size(676, 147);
            this.tabPageHops.TabIndex = 1;
            this.tabPageHops.Text = "Hops";
            this.tabPageHops.UseVisualStyleBackColor = true;
            // 
            // hopsControl
            // 
            this.hopsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hopsControl.Location = new System.Drawing.Point(0, 0);
            this.hopsControl.Name = "hopsControl";
            this.hopsControl.Size = new System.Drawing.Size(676, 147);
            this.hopsControl.TabIndex = 0;
            // 
            // tabPageHopVisualization
            // 
            this.tabPageHopVisualization.Controls.Add(this.hopVisualizationControl);
            this.tabPageHopVisualization.Location = new System.Drawing.Point(4, 4);
            this.tabPageHopVisualization.Name = "tabPageHopVisualization";
            this.tabPageHopVisualization.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHopVisualization.Size = new System.Drawing.Size(676, 147);
            this.tabPageHopVisualization.TabIndex = 4;
            this.tabPageHopVisualization.Text = "Hop-Visualisierung";
            this.tabPageHopVisualization.UseVisualStyleBackColor = true;
            // 
            // hopVisualizationControl
            // 
            this.hopVisualizationControl.AutoScroll = true;
            this.hopVisualizationControl.AutoSize = true;
            this.hopVisualizationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hopVisualizationControl.Location = new System.Drawing.Point(3, 3);
            this.hopVisualizationControl.Name = "hopVisualizationControl";
            this.hopVisualizationControl.Size = new System.Drawing.Size(670, 141);
            this.hopVisualizationControl.TabIndex = 0;
            // 
            // tabPageScenarios
            // 
            this.tabPageScenarios.Controls.Add(this.scenariosControl);
            this.tabPageScenarios.Location = new System.Drawing.Point(4, 4);
            this.tabPageScenarios.Name = "tabPageScenarios";
            this.tabPageScenarios.Size = new System.Drawing.Size(676, 147);
            this.tabPageScenarios.TabIndex = 2;
            this.tabPageScenarios.Text = "Szenarios";
            this.tabPageScenarios.UseVisualStyleBackColor = true;
            // 
            // scenariosControl
            // 
            this.scenariosControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenariosControl.Location = new System.Drawing.Point(0, 0);
            this.scenariosControl.Name = "scenariosControl";
            this.scenariosControl.Size = new System.Drawing.Size(676, 147);
            this.scenariosControl.TabIndex = 0;
            // 
            // tabPageResults
            // 
            this.tabPageResults.Controls.Add(this.resultsControl);
            this.tabPageResults.Location = new System.Drawing.Point(4, 4);
            this.tabPageResults.Name = "tabPageResults";
            this.tabPageResults.Size = new System.Drawing.Size(676, 147);
            this.tabPageResults.TabIndex = 3;
            this.tabPageResults.Text = "Ergebnisse";
            this.tabPageResults.UseVisualStyleBackColor = true;
            // 
            // resultsControl
            // 
            this.resultsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultsControl.Location = new System.Drawing.Point(0, 0);
            this.resultsControl.Name = "resultsControl";
            this.resultsControl.Size = new System.Drawing.Size(676, 147);
            this.resultsControl.TabIndex = 0;
            // 
            // InfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TcPages);
            this.Name = "InfoControl";
            this.Size = new System.Drawing.Size(684, 195);
            this.TcPages.ResumeLayout(false);
            this.tabPageHistory.ResumeLayout(false);
            this.tabPageHops.ResumeLayout(false);
            this.tabPageHopVisualization.ResumeLayout(false);
            this.tabPageHopVisualization.PerformLayout();
            this.tabPageScenarios.ResumeLayout(false);
            this.tabPageResults.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPageHops;
        private System.Windows.Forms.TabPage tabPageScenarios;
        private System.Windows.Forms.TabPage tabPageResults;
        private System.Windows.Forms.TabPage tabPageHistory;
        private System.Windows.Forms.TabPage tabPageHopVisualization;
        private HistoryControl historyControl;
        private HopsControl hopsControl;
        private ScenariosControl scenariosControl;
        private ResultsControl resultsControl;
        private HopVisualizationControl hopVisualizationControl;
    }
}
