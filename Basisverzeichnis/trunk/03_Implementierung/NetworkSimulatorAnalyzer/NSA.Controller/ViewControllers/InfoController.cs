﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using NSA.Model.BusinessLogic;
using NSA.Model.NetworkComponents;
using NSA.Model.NetworkComponents.Helper_Classes;
using NSA.View.Controls.InfoControl;
using NSA.View.Forms;

namespace NSA.Controller.ViewControllers
{
    public class InfoController
    {
        #region Singleton

        public static InfoController Instance = new InfoController();

        #endregion Singleton

        private readonly InfoControl infoControl;

        // {2} if a detailed result is available
        private const string baseResult = "Simulation von {0} nach {1} fehlgeschlagen {2}";
        private const string sendPacket = "Hinpaket {0}";
        private const string receivedPacket = "Rückpaket {0}";

        private Simulation lastSimulation;

        public void Initialize() {}

        /// <summary>
        /// Prevents a default instance of the <see cref="InfoController"/> class from being created.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">InfoControl was null/not found</exception>
        private InfoController()
        {
            infoControl = MainForm.Instance.GetComponent("InfoControl") as InfoControl;
            Debug.Assert(infoControl != null, "InfoControl was null/not found");

            var htc = infoControl.historyControl;
            var stc = infoControl.scenariosControl;
            var hstc = infoControl.hopsControl;

            Debug.Assert(htc != null, "HistoryTabControl was null/not found");
            Debug.Assert(stc != null, "ScenariosTabControl was null/not found");
            Debug.Assert(hstc != null, "HopsTabControl was null/not found");

            // History Control Eventhandler
            htc.HistoryRerunButtonClicked += historyTabPage_HistoryRerunButtonClicked;
            htc.HistoryClearButtonClicked += historyTabPage_HistoryClearButtonClicked;
            htc.HistoryDeleteButtonClicked += historyTabPage_HistoryDeleteButtonClicked;

            // Scenarios Control Eventhandler
            stc.StartScenarioButtonClicked += ScenarioTabPage_StartScenarioButtonClicked;

            // Hops Control Eventhanlder
            hstc.PacketSelected += hopsTabPage_PacketSelected;
        }

        #region Event Handling

        /// <summary>
        /// Handles the HistoryRerunButtonClicked event of the HistoryTabPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void historyTabPage_HistoryRerunButtonClicked(object sender, EventArgs e)
        {
            DataRow row = sender as DataRow;
            if (row == null) return;
            string simID = row["Simulations ID"].ToString();
            SimulationManager.Instance.RunSimulationFromHistory(simID);
        }

        /// <summary>
        /// Handles the HistoryDeleteButtonClicked event of the HistoryTabPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private void historyTabPage_HistoryDeleteButtonClicked(object sender, EventArgs e)
        {
            DataRow row = sender as DataRow;
            if (row == null) return;
            string simID = row["Simulations ID"].ToString();
            SimulationManager.Instance.DeleteSimulationFromHistory(simID);
            infoControl.historyControl.DeleteHistoryData(row);
        }

        /// <summary>
        /// Handles the HistoryClearButtonClicked event of the HistoryTabPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private void historyTabPage_HistoryClearButtonClicked(object sender, EventArgs e)
        {
            ClearHistory();
        }

        /// <summary>
        /// Handles the StartScenarioButtonClicked event of the ScenarioTabPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private void ScenarioTabPage_StartScenarioButtonClicked(object sender, EventArgs e)
        {
            DataRow row = sender as DataRow;
            if (row == null) return;

            var scenarioName = row["Testszenario"].ToString();
            Testscenario ts = ProjectManager.Instance.GetTestscenarioByName(scenarioName);
            if (ts == null) return;

            List<Simulation> failedSimulations = SimulationManager.Instance.StartTestscenario(ts);
            if (failedSimulations.Count == 0) addScenarioResultToResultsTab(scenarioName, "Erfolgreich");
            else
            {
                foreach (Simulation s in failedSimulations)
                {
                    string simResult = string.Format(baseResult, s.Source, s.Destination, "");
                    addScenarioResultToResultsTab(scenarioName, simResult);
                }

                addScenarioResultToResultsTab(scenarioName, "Fehler aufgetreten");
            }
        }

        /// <summary>
        /// Handles the PacketSelected event of the HopsTabPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void hopsTabPage_PacketSelected(object sender, string e)
        {
            var values = e.Split(' ');
            int index = int.Parse(values[1]);
            List<Hardwarenode> hops = null;

            infoControl.hopsControl.ClearHopsOnly();

            if (values[0].Equals(string.Format(sendPacket, "").Trim(' ')))
            {
                hops = lastSimulation.PacketsSend[index].Hops;
            }
            else if (values[0].Equals(string.Format(receivedPacket, "").Trim(' ')))
            {
                hops = lastSimulation.PacketsReceived[index].Hops;
            }

            if (hops == null)
                return;

            if (hops.Count == 1)
            {
                infoControl.hopsControl.AddHop(hops[0].Name, "-", "-", "-");
            }

            for (int i = 0; i < hops.Count - 1; i++)
            {
                var results = SimulationManager.Instance.GetHopResult(true, index, hops[i].Name, hops[i + 1].Name);

                string res1 = results.Item1.ErrorId == Result.Errors.NoError ? "kein Fehler" : results.Item1.Res;
                string res2 = results.Item2.ErrorId == Result.Errors.NoError ? "kein Fehler" : results.Item2.Res;

                infoControl.hopsControl.AddHop(hops[i].Name, res1, hops[i + 1].Name, res2);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the new simulation to history.
        /// </summary>
        /// <param name="Sim">The simulation.</param>
        public void AddSimulationToHistoryTab(Simulation Sim)
        {
            string expectedRes = Sim.ExpectedResult ? "Verbindung möglich" : "Verbindung nicht möglich";
            string simResult = SimulationManager.Instance.GetSimulationResult(Sim.Id) ? "Erfolgreich" : "Fehler aufgetreten";

            lastSimulation = Sim;
            infoControl.historyControl.AddHistoryData(Sim.Id, expectedRes, simResult, Sim.Source, Sim.Destination);
            UpdateHopsFromLastSimulation(Sim);
        }

        /// <summary>
        /// Clears the history.
        /// </summary>
        public void ClearHistory()
        {
            SimulationManager.Instance.ClearHistory();
            infoControl.historyControl.Clear();
        }


        /// <summary>
        /// Adds the testscenario to scenario tab.
        /// </summary>
        /// <param name="T">The t.</param>
        public void AddTestscenarioToScenarioTab(Testscenario T)
        {
            infoControl.scenariosControl.AddTestScenario(T.FileName, T.SimulationCount);
        }

        /// <summary>
        /// Adds the scenario result to results tab.
        /// </summary>
        /// <param name="ScenarioName">Name of the scenario.</param>
        /// <param name="ScenarioResult">The scenario result.</param>
        private void addScenarioResultToResultsTab(string ScenarioName, string ScenarioResult)
        {
            infoControl.resultsControl.AddResultData(ScenarioName, ScenarioResult);
        }

        /// <summary>
        /// Updates the hops from last simulation.
        /// </summary>
        /// <param name="Sim">The sim.</param>
        public void UpdateHopsFromLastSimulation(Simulation Sim)
        {
            infoControl.hopsControl.Clear();

            var sendPackets = Sim.PacketsSend;
            var receivedPackets = Sim.PacketsReceived;
            
            for (int i = 0; i < sendPackets.Count; i++)
            {
                infoControl.hopsControl.AddPacket(string.Format(sendPacket, i));
            }

            for (int i = 0; i < receivedPackets.Count; i++)
            {
                infoControl.hopsControl.AddPacket(string.Format(receivedPacket, i));
            }
        }

        /// <summary>
        /// Clears the complete information control.
        /// </summary>
        public void ClearInfoControl()
        {
            infoControl.historyControl.Clear();
            infoControl.resultsControl.Clear();
            infoControl.hopsControl.Clear();
            infoControl.scenariosControl.Clear();

            lastSimulation = null;
        }

        #endregion
    }
}