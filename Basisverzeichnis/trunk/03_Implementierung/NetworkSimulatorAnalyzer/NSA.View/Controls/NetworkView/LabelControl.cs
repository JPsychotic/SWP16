﻿using System;
using System.Drawing;
using System.Windows.Forms;
using NSA.View.Controls.NetworkView.NetworkElements.Base;

namespace NSA.View.Controls.NetworkView
{
    public partial class LabelControl : EditorElementBase
    {
        public event Action<string, string> NameChanged;

        public new static int ZIndexStart = 1000;

        private EditorElementBase parentElement;

        [Obsolete("Dont use this. This is for the Designer only.")]
        public LabelControl()
        {
            InitializeComponent();
            textBox.Text = "Text";
        }

        public LabelControl(EditorElementBase element)
        {
            ZIndex = ZIndexStart++;
            element.LocationChanged += Element_LocationChanged;
            parentElement = element;
            InitializeComponent();
            textBox.TextChanged += Label1_TextChanged;
            textBox.KeyDown += TextBox1_KeyDown;
            textBox.LostFocus += TextBox1_LostFocus;
            textBox.Text = element.Name;
            Click += LabelControl_Click;
            Height = textBox.Height;
            Label1_TextChanged(parentElement, null);
            element.Disposed += Element_Disposed;
            textBox.Enabled = false;
            textBox.ForeColor = Color.Red;
        }

        private void LabelControl_Click(object sender, EventArgs e)
        {
            textBox.Enabled = true;
            textBox.Focus();
        }

        private void Element_Disposed(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
            Dispose();
        }

        private void TextBox1_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text)) textBox.Text = "Name";
        }

        private void Element_LocationChanged(object sender, EventArgs e)
        {
            Location = new Point(parentElement.Location.X + parentElement.Width / 2 - Width / 2 + 3, parentElement.Location.Y - textBox.Height);
        }

        private void Label1_TextChanged(object sender, EventArgs e)
        {
            var measure = TextRenderer.MeasureText(textBox.Text, textBox.Font);
            Width = textBox.Width = measure.Width;
            Height = measure.Height;
            NameChanged?.Invoke(parentElement.Name, textBox.Name);
            parentElement.Name = textBox.Text;
            Element_LocationChanged(parentElement, null);
        }
        

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox.Enabled = false;
                //this is to remove focus from the textbox
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
          
        }
    }
}
