﻿using System;
using System.Collections.Generic;
using NSA.Model.NetworkComponents;
using NSA.View.Controls.NetworkView;
using NSA.View.Controls.NetworkView.NetworkElements.Base;
using NSA.View.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NSA.View.Controls.NetworkView.NetworkElements;
using System.Linq;
using System.Windows.Forms;

namespace NSA.Controller.ViewControllers
{
    public class NetworkViewController
    {
        #region Singleton

        public static NetworkViewController Instance = new NetworkViewController();

        #endregion Singleton

        private NetworkViewControl networkViewControl;
        private Random rnd = new Random();

        public void Initialize()
        {
            networkViewControl = MainForm.Instance.GetComponent("NetworkviewControl") as NetworkViewControl;
            if (networkViewControl == null) throw new NullReferenceException("NetworkviewControl is null");
            networkViewControl.SelectionChanged += EditorElement_Selected;
            networkViewControl.RemoveConnectionPressed += RemoveConnectionRequest;
            networkViewControl.RemoveElementPressed += RemoveHardwarenodeRequest;
            networkViewControl.NewConnectionCreated += OnNewConnectionCreated;
            networkViewControl.QuickSimulation += OnQuickSimulationCreated;
            networkViewControl.NodeRenamed += NetworkViewControl_NodeRenamed;
        }

        private void NetworkViewControl_NodeRenamed(string oldName, string newName)
        {
            NetworkManager.Instance.RenameHardwarenode(oldName, newName);
        }

        public void OnQuickSimulationCreated(string source, string target)
        {
            SimulationManager.Instance.CreateAndExecuteSimulation(source, target);
        }

        public void QuickSimulationRequest()
        {
            networkViewControl.CreateNewQuickSimulation();

        }

        private void OnNewConnectionCreated(Control c1, int port1, Control c2, int port2)
        {
            NetworkManager.Instance.CreateConnection(c1.Name, "eth" + port1, c2.Name, "eth" + port2);
        }

        public Point? GetLocationOfElementByName(string name)
        {
            return networkViewControl.Controls.OfType<EditorElementBase>().FirstOrDefault(s => s.Name == name)?.Location;
        }

        public void MoveElementToLocation(string name, Point loc)
        {
            var element = networkViewControl.Controls.OfType<EditorElementBase>().FirstOrDefault(s => s.Name == name);
            if (element != null) element.Location = loc;
        }

        private void RemoveHardwarenodeRequest(EditorElementBase e)
        {
            NetworkManager.Instance.RemoveHardwarenode(e.Name);
        }

        public void RemoveHardwarenode(string name)
        {
            networkViewControl.RemoveElement(networkViewControl.Controls.OfType<EditorElementBase>().First(n => n.Name == name));
        }

        private void RemoveConnectionRequest(VisualConnection c)
        {
            NetworkManager.Instance.RemoveConnection(c.Name);
        }

        public void RemoveConnection(string name)
        {
            networkViewControl.RemoveConnection(networkViewControl.connections.First(c => c.Name == name));
        }

        public void EditorElement_Selected(EditorElementBase selectedElement)
        {
            PropertyController.Instance.LoadElementProperties(selectedElement?.Name);
        }

        public void AddHardwarenode(Hardwarenode node)
        {
            var randomLoc = new Point(10 + (int)((networkViewControl.Width - 100) * rnd.NextDouble()), 10 + (int)((networkViewControl.Height - 100) * rnd.NextDouble()));
            if (node is Switch) networkViewControl.AddElement(new SwitchControl(randomLoc, node.Name));
            else if (node is Router) networkViewControl.AddElement(new RouterControl(randomLoc, node.Name));
            else if (node is Workstation) networkViewControl.AddElement(new WorkstationControl(randomLoc, node.Name));
        }

        public void AddConnection(Connection connection)
        {
            var node1 = GetControlByName(connection.Start.Name);
            var node2 = GetControlByName(connection.End.Name);
            // ReSharper disable once NotResolvedInText
            if (node1 == null || node2 == null) throw new ArgumentNullException("referenced start or end of connection is null");
            networkViewControl.AddElement(new VisualConnection(connection.Name, node1, connection.GetPortIndex(connection.Start), node2, connection.GetPortIndex(connection.End), networkViewControl));
        }

        public void CreateHardwarenodeRequest()
        {
            networkViewControl.CreateNewConnection();
        }

        private EditorElementBase GetControlByName(string name)
        {
            return networkViewControl.Controls.OfType<EditorElementBase>().First(s => s.Name == name);
        }

        public void SwitchChanged(Switch sw)
        {
            var switchControl = (SwitchControl)GetControlByName(sw.Name);
            var ifaces = new List<int>();
            foreach (var i in sw.Interfaces) ifaces.Add(int.Parse(i.Name.Replace(Interface.NamePrefix, "")));
            switchControl.SetInterfaces(ifaces);
        }

        public void AddInterfaceToHardwareNode(string NodeName, string InterfaceName)
        {
            networkViewControl.AddInterfaceToNode(NodeName, InterfaceName);
        }

        public void RemoveInterfaceFromNode(string NodeName, string InterfaceName)
        {
            networkViewControl.RemoveInterfaceFromNode(NodeName, InterfaceName);
        }

        public void SaveToBitmap()
        {
            var bitmap = networkViewControl.CreateScreenshot();
            if (bitmap == null) return;
            var saveFileDialog = new SaveFileDialog { Filter = "PNG|*.png" };
            var result = saveFileDialog.ShowDialog();
            if (result != DialogResult.OK) return;
            var file = saveFileDialog.FileName;
            bitmap.Save(file, ImageFormat.Png);
        }

        public void HighlightConnections(List<string> connectionNames)
        {
            if (connectionNames == null)
            {
                foreach (var c in networkViewControl.connections) c.Highlight(false);
            }
            else
            {
                foreach (var c in networkViewControl.connections) c.Highlight(connectionNames.Contains(c.Name));
            }
        }
    }
}