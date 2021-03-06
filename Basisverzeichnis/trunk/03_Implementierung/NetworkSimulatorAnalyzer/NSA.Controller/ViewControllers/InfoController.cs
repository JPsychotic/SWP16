﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NSA.Model.BusinessLogic;
using NSA.Model.NetworkComponents;
using NSA.Model.NetworkComponents.Helper_Classes;
using NSA.View.Controls.InfoControl;
using NSA.View.Forms;

namespace NSA.Controller.ViewControllers
{
    /// <summary>
    /// Implements the controller for the info control of the main form.
    /// is singleton
    /// </summary>
    public class InfoController
    {
        #region Singleton

        /// <summary>
        /// The instance
        /// </summary>
        public static InfoController Instance = new InfoController();

        #endregion Singleton

        private readonly InfoControl infoControl;

        // {2} if a detailed result is available
        private const string baseResult = "Simulation von {0} nach {1} fehlgeschlagen {2}";
        private const string sendPacket = "Hinpaket {0}";
        private const string receivedPacket = "Rückpaket {0}";

        private Simulation lastSimulation;
        private int executedScenarioCount;
        private bool hopsPageVisible;

        private List<Hardwarenode> currentHops = new List<Hardwarenode>();
        private List<Tuple<Result, Result>> currentResults = new List<Tuple<Result, Result>>();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize() {}

        /// <summary>
        /// Prevents a default instance of the <see cref="InfoController"/> class from being created.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">InfoControl was null/not found</exception>
        private InfoController()
        {
            infoControl = MainForm.Instance.GetComponent("InfoControl") as InfoControl;
            Debug.Assert(infoControl != null, "InfoControl was null/not found");

            var htc = infoControl.HistoryControl;
            var stc = infoControl.ScenariosControl;
            var hstc = infoControl.HopsControl;
            var rtc = infoControl.ResultsControl;

            Debug.Assert(htc != null, "HistoryTabControl was null/not found");
            Debug.Assert(stc != null, "ScenariosTabControl was null/not found");
            Debug.Assert(hstc != null, "HopsTabControl was null/not found");
            Debug.Assert(rtc != null, "ResultTabControl was null/not found");

            // History Control Eventhandler
            htc.HistoryRerunButtonClicked += historyTabPage_HistoryRerunButtonClicked;
            htc.HistoryClearButtonClicked += historyTabPage_HistoryClearButtonClicked;
            htc.HistoryDeleteButtonClicked += historyTabPage_HistoryDeleteButtonClicked;

            // Scenarios Control Eventhandler
            stc.StartScenarioButtonClicked += ScenarioTabPage_StartScenarioButtonClicked;

            // Hops Control (associated) Eventhandler
            hstc.PacketSelected += hopsTabPage_PacketSelected;
            infoControl.HopsTabPage_Selected += hopsTabPage_Selected;
            infoControl.HopsTabPage_Deselected += hopsTabPage_Deselected;
            hstc.HopSelected += Hstc_HopSelected;

            // Result Control Eventhandler
            rtc.ClearButtonClicked += resultsTabPage_ClearButtonClicked;
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
            infoControl.HistoryControl.DeleteHistoryData(row);
        }

        /// <summary>
        /// Handles the HistoryClearButtonClicked event of the HistoryTabPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private void historyTabPage_HistoryClearButtonClicked(object sender, EventArgs e)
        {
            SimulationManager.Instance.ClearHistory();
            infoControl.HistoryControl.Clear();
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

            var scenarioNameFull = row["Testszenario Pfad"].ToString();
            Testscenario ts = ProjectManager.Instance.GetTestscenarioByName(scenarioNameFull);
            if (ts == null) return;

            List<Simulation> failedSimulations = SimulationManager.Instance.StartTestscenario(ts);

            if (failedSimulations.Count == 0) AddScenarioResultToResultsTab(scenarioNameFull, "Erfolgreich", executedScenarioCount);
            else
            {
                foreach (Simulation s in failedSimulations)
                {
                    string simResult = string.Format(baseResult, s.Source, s.Destination, "");
                    AddScenarioResultToResultsTab(scenarioNameFull, simResult, executedScenarioCount);
                }

                AddScenarioResultToResultsTab(scenarioNameFull, "Fehler aufgetreten", executedScenarioCount);
            }

            executedScenarioCount++;
            infoControl.ChangeToResultsTab();
        }

        /// <summary>
        /// Handles the PacketSelected event of the HopsTabPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the name of the selected packet.</param>
        private void hopsTabPage_PacketSelected(object sender, string e)
        {
            var values = e.Split(' ');
            int index = int.Parse(values[1]);
            bool isSendPacket = values[0].Equals(string.Format(sendPacket, "").Trim(' '));
            bool isReceivedPacket = values[0].Equals(string.Format(receivedPacket, "").Trim(' '));
            List<Hardwarenode> hops = null;

            infoControl.HopsControl.ClearHopsOnly();

            if (isSendPacket)
            {
                hops = lastSimulation.PacketsSend[index].Hops;
            }
            else if (isReceivedPacket)
            {
                hops = lastSimulation.PacketsReceived[index].Hops;
            }

            if (hops == null)
                return;

            currentHops = hops;
            currentResults.Clear();

            if (hops.Count > 1)
                for (int i = 0; i < hops.Count - 1; i++)
                {
                    var results = SimulationManager.Instance.GetHopResult(isSendPacket, index, hops[i].Name,
                        hops[i + 1].Name);

                    string res1 = results.Item1.ErrorId == Result.Errors.NoError ? "kein Fehler" : results.Item1.Res;
                    string res2 = results.Item2.ErrorId == Result.Errors.NoError ? "kein Fehler" : results.Item2.Res;

                    infoControl.HopsControl.AddHop(hops[i].Name, res1, hops[i + 1].Name, res2);
                    currentResults.Add(results);
                }
            else
            {
                var results = SimulationManager.Instance.GetHopResult(isSendPacket, index, hops.First().Name);
                string res1 = results.Item1.ErrorId == Result.Errors.NoError ? "kein Fehler" : results.Item1.Res;
                infoControl.HopsControl.AddHop(hops[0].Name, res1, "-", "-");
                currentResults.Add(results);
            }

            if(hopsPageVisible) SimulationManager.Instance.HighlightPacketConnections(isSendPacket, index);
            Hstc_HopSelected(0);
        }


        private void Hstc_HopSelected(int HopIndex)
        {
            if (currentResults.Count == 0) return;

            Result srcResult = currentResults[HopIndex].Item1;
            int error1Index = -1;
            if (srcResult.LayerError != null)
            {
                error1Index = srcResult.LayerError.GetLayerIndex();
            }
            Result destResult = currentResults[HopIndex].Item2;
            int error2Index = -1;
            if (destResult.LayerError != null)
            {
                error2Index = destResult.LayerError.GetLayerIndex();
            }

            // Quick n Dirty, zu spät für was schöneres:
            // If there's more than one result set receiveError in dest to true in order to gray it out. 
            bool receiveError = currentHops.Count == 1;
            int sourceIndex = HopIndex;
            int destIndex = HopIndex + (currentHops.Count > 1 ? 1 : 0);
            infoControl.HopVisualizationControl.LoadHopInfo(
                currentHops[sourceIndex].Name,
                currentHops[sourceIndex].Layerstack.GetAllLayers().Select(n => n.GetLayerName()).ToList(),
                error1Index,
                srcResult.SendError,
                currentHops[destIndex].Name,
                currentHops[destIndex].Layerstack.GetAllLayers().Select(n => n.GetLayerName()).ToList(),
                error2Index,
                receiveError
                );
        }


        /// <summary>
        /// Handles the HopsTabPage_Selected event of the InfoControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void hopsTabPage_Selected(object sender, EventArgs e)
        {
            hopsPageVisible = true;

            var values = infoControl.HopsControl.SelectedPacket?.Split(' ');
            if(values == null) return;

            int index = int.Parse(values[1]);
            bool isSendPacket = values[0].Equals(string.Format(sendPacket, "").Trim(' '));
            bool isReceivedPacket = values[0].Equals(string.Format(receivedPacket, "").Trim(' '));

            if(!isSendPacket && !isReceivedPacket) return;

            SimulationManager.Instance.HighlightPacketConnections(isSendPacket, index);
        }

        /// <summary>
        /// Handles the HopsTabPage_Deselected event of the InfoControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void hopsTabPage_Deselected(object sender, EventArgs e)
        {
            hopsPageVisible = false;
            SimulationManager.Instance.UnhighlightConnections();
        }


        /// <summary>
        /// Handles the ClearButtonClicked event of the resultsTabPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void resultsTabPage_ClearButtonClicked(object sender, EventArgs e)
        {
            infoControl.ResultsControl.Clear();
            executedScenarioCount = 0;
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
            infoControl.HistoryControl.AddHistoryData(Sim.Id, expectedRes, simResult, Sim.Source, Sim.Destination);
            UpdatePacketsFromLastSimulation(Sim);
            // Quick n Dirty, ist zu verwurschtelt alles leider
            if (Sim.PacketsSend.Count > 0)
            {
                hopsTabPage_PacketSelected(this, "Hinpaket 0");
                Hstc_HopSelected(0);
            }
        }

        /// <summary>
        /// Adds the testscenario to scenario tab.
        /// </summary>
        /// <param name="T">The t.</param>
        public void AddTestscenarioToScenarioTab(Testscenario T)
        {
            infoControl.ScenariosControl.AddTestScenario(T.FileName);
        }

        /// <summary>
        /// Adds the scenario result to results tab.
        /// </summary>
        /// <param name="ScenarioName">Name of the scenario.</param>
        /// <param name="ScenarioResult">The scenario result.</param>
        /// <param name="Number">A consecutive number foreach executed testscenario.</param>
        private void AddScenarioResultToResultsTab(string ScenarioName, string ScenarioResult, int Number)
        {
            infoControl.ResultsControl.AddResultData(ScenarioName, ScenarioResult, Number);
        }

        /// <summary>
        /// Updates the hops from last simulation.
        /// </summary>
        /// <param name="Sim">The sim.</param>
        public void UpdatePacketsFromLastSimulation(Simulation Sim)
        {
            infoControl.HopsControl.Clear();

            var sendPackets = Sim.PacketsSend;
            var receivedPackets = Sim.PacketsReceived;
            
            for (int i = 0; i < sendPackets.Count; i++)
            {
                infoControl.HopsControl.AddPacket(string.Format(sendPacket, i));
            }

            for (int i = 0; i < receivedPackets.Count; i++)
            {
                infoControl.HopsControl.AddPacket(string.Format(receivedPacket, i));
            }
            Hstc_HopSelected(0);
        }

        /// <summary>
        /// Clears the complete information control.
        /// </summary>
        public void ClearInfoControl()
        {
            infoControl.HistoryControl.Clear();
            infoControl.ResultsControl.Clear();
            infoControl.HopsControl.Clear();
            infoControl.ScenariosControl.Clear();
            infoControl.HopVisualizationControl.ClearHopInfo();
            lastSimulation = null;
            executedScenarioCount = 0;
        }

        #endregion
    }
}