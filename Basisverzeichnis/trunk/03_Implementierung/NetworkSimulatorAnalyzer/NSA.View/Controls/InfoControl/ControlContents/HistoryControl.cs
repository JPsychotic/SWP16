﻿using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

namespace NSA.View.Controls.InfoControl.ControlContents
{
    public partial class HistoryControl : UserControl
    {
        private readonly DataTable data = new DataTable();

        public event EventHandler HistoryRerunButtonClicked;
        public event EventHandler HistoryDeleteButtonClicked;
        public event EventHandler HistoryClearButtonClicked;

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryControl"/> class.
        /// </summary>
        public HistoryControl()
        {
            InitializeComponent();
            dgvHistory.AutoGenerateColumns = false;
            AddColumns();
        }

        /// <summary>
        /// Adds the columns to the datatable and datagridview.
        /// </summary>
        private void AddColumns()
        {
            data.Columns.Add("Simulations ID", typeof(string));
            data.Columns.Add("Start", typeof(string));
            data.Columns.Add("Ziel", typeof(string));
            data.Columns.Add("Erwartetes Ergebnis", typeof(string));
            data.Columns.Add("Ergebnis", typeof(string));

            var dataCol1 = new DataGridViewTextBoxColumn
            {
                HeaderText = "Start",
                DataPropertyName = "Start",
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayIndex = 1
            };

            var dataCol2 = new DataGridViewTextBoxColumn
            {
                HeaderText = "Ziel",
                DataPropertyName = "Ziel",
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayIndex = 2
            };

            var dataCol3 = new DataGridViewTextBoxColumn
            {
                HeaderText = "Erwartetes Ergebnis",
                DataPropertyName = "Erwartetes Ergebnis",
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayIndex = 3
            };

            var dataCol4 = new DataGridViewTextBoxColumn
            {
                HeaderText = "Ergebnis",
                DataPropertyName = "Ergebnis",
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayIndex = 4
            };

            var bnCol1 = new DataGridViewButtonColumn
            {
                HeaderText = "Aktion",
                Text = "Erneut Ausführen",
                UseColumnTextForButtonValue = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayIndex = 5
            };

            var bnCol2 = new DataGridViewButtonColumn
            {
                HeaderText = "Löschen",
                Text = "X",
                UseColumnTextForButtonValue = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DisplayIndex = 6
            };

            dgvHistory.DataSource = data;
            dgvHistory.Columns.AddRange(dataCol1, dataCol2, dataCol3, dataCol4, bnCol1, bnCol2);
        }


        /// <summary>
        /// Adds the history data.
        /// </summary>
        /// <param name="SimID">The sim identifier.</param>
        /// <param name="ExpectedResult">The expected result.</param>
        /// <param name="Result">The result.</param>
        /// <param name="Source">The source.</param>
        /// <param name="Destination">The destination.</param>
        public void AddHistoryData(string SimID, string ExpectedResult, string Result, string Source, string Destination)
        {
            var row = data.NewRow();
            row.ItemArray = new object[] {SimID, Source, Destination, ExpectedResult, Result};

            data.Rows.InsertAt(row, 0);
        }


        /// <summary>
        /// Deletes the given history data row.
        /// </summary>
        /// <param name="Row">The row.</param>
        public void DeleteHistoryData(DataRow Row)
        {
            data.Rows.Remove(Row);
        }

        /// <summary>
        /// Clears this tab.
        /// </summary>
        public void Clear()
        {
            data.Rows.Clear();
        }

        #region Eventhandling

        /// <summary>
        /// Handles the CellContentClick event of the dgvHistory control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private void dgvHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                if (senderGrid.Columns[e.ColumnIndex].HeaderText == "Aktion")
                    HistoryRerunButtonClicked?.Invoke(data.Rows[e.RowIndex], new EventArgs());
                else if (senderGrid.Columns[e.ColumnIndex].HeaderText == "Löschen")
                    HistoryDeleteButtonClicked?.Invoke(data.Rows[e.RowIndex], new EventArgs());
            }
        }


        /// <summary>
        /// Handles the Click event of the bnClearHistory control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void bnClearHistory_Click(object sender, EventArgs e)
        {
            HistoryClearButtonClicked?.Invoke(sender, e);
        }

        #endregion
    }
}
