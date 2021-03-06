﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace NSA.View.Forms
{
    public partial class MainForm : Form
    {
        #region Singleton
        
        /// <summary>
        /// Property for getting the current instance
        /// </summary>
        public static MainForm Instance = new MainForm();

        #endregion Singleton
        
        MainForm()
        {
            InitializeComponent();
            MainForm_Resize(null, null);
        }
        
        /// <summary>
        /// Looks for a user control by name
        /// </summary>
        /// <param name="name">Name of the control</param>
        /// <returns>The control, if a match was found, null otherwise.</returns>
        public UserControl GetComponent(string name)
        {
            foreach (UserControl control in Controls)
                if (control.Tag?.Equals(name) == true) return control;
            return null;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            int margin = 5;
            ToolbarControl.Location = new Point(1, 0);
            ToolbarControl.Size = new Size(Width - margin * 2, ToolbarControl.Height);
            networkViewControl.Location = new Point(margin, ToolbarControl.Location.Y + ToolbarControl.Height + margin);
            networkViewControl.Width = Width - propertyControl.Width - margin * 4 - 12;
            networkViewControl.Height = Height - ToolbarControl.Height - infoControl.Height - margin * 4 - 35;
            propertyControl.Location = new Point(networkViewControl.Width + networkViewControl.Location.X + margin, networkViewControl.Location.Y);
            propertyControl.Height = networkViewControl.Height + infoControl.Height;
            propertyControl.Refresh();
            infoControl.Location = new Point(margin, networkViewControl.Location.Y + networkViewControl.Height + margin);
            infoControl.Width = networkViewControl.Width;
            infoControl.Refresh();
            networkViewControl.Invalidate();
        }

        /// <summary>
        /// Turn on WS_EX_COMPOSITED
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
    }
}
