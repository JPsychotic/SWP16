﻿using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows.Forms;

namespace NSA.View.Controls.InfoControl.ControlContents
{
    /// <summary>
    /// Class for the ResultsControl of the InfoControl. 
    /// It displays the result ofexecuted testscenarios.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class ResultsControl : UserControl
    {
        /// <summary>
        /// Occurs when the results clear button is clicked.
        /// </summary>
        public event EventHandler ClearButtonClicked;

        private readonly DataTable data = new DataTable();

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsControl"/> class.
        /// </summary>
        public ResultsControl()
        {
            InitializeComponent();
            dgvResults.AutoGenerateColumns = false;
            AddColumns();
        }


        /// <summary>
        /// Adds the columns to the datatable and datagridview.
        /// </summary>
        private void AddColumns()
        {
            data.Columns.Add("Nummer", typeof(int));
            data.Columns.Add("Testszenario", typeof(string));
            data.Columns.Add("Ergebnis", typeof(string));

            var dataCol0 = new DataGridViewTextBoxColumn
            {
                HeaderText = "Nr.",
                DataPropertyName = "Nummer",
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayIndex = 0
            };

            var dataCol1 = new DataGridViewTextBoxColumn
            {
                HeaderText = "Testszenario",
                DataPropertyName = "Testszenario",
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayIndex = 1
            };

            var dataCol2 = new DataGridViewTextBoxColumn
            {
                HeaderText = "Ergebnis",
                DataPropertyName = "Ergebnis",
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayIndex = 2
            };
            
            var bnCol1 = new DataGridViewButtonColumn
            {
                HeaderText = "Löschen",
                Text = "X",
                UseColumnTextForButtonValue = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayIndex = 3
            };

            dgvResults.DataSource = data;
            dgvResults.Columns.AddRange(dataCol0, dataCol1, dataCol2, bnCol1);
        }

        #region Eventhandling

        /// <summary>
        /// Handles the CellContentClick event of the dgvResults control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs" /> instance containing the event data.</param>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private void dgvResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                if (senderGrid.Columns[e.ColumnIndex].HeaderText == "Löschen")
                    data.Rows.Remove(data.Rows[e.RowIndex]);
            }
        }


        /// <summary>
        /// Handles the KeyDown event of the dgvResults control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void dgvResults_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete) return;

            var row = ((DataRowView)dgvResults.SelectedRows[0].DataBoundItem).Row;
            if (row == null) return;

            data.Rows.Remove(row);
        }

        /// <summary>
        /// Handles the Click event of the bnClearResults button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bnClearResults_Click(object sender, EventArgs e)
        {
            ClearButtonClicked?.Invoke(sender, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// Adds the result data.
        /// </summary>
        /// <param name="ScenarioName">Name of the scenario.</param>
        /// <param name="Result">The result.</param>
        /// <param name="Number">A consecutive number for each executed testscenario.</param>
        public void AddResultData(string ScenarioName, string Result, int Number)
        {
            var row = data.NewRow();
            row.ItemArray = new object[] { Number, Path.GetFileNameWithoutExtension(ScenarioName), Result };

            data.Rows.InsertAt(row, 0);
        }

        /// <summary>
        /// Clears this tab.
        /// </summary>
        public void Clear()
        {
            data.Rows.Clear();
        }
    }
}
