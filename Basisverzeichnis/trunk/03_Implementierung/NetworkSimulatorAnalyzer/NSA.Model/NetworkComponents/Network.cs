﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace NSA.Model.NetworkComponents
{
    public class Network
    {
        private readonly List<Hardwarenode> nodes;
        public List<Connection> Connections { get; private set; }

        public Network()
	    {
            nodes = new List<Hardwarenode>();
            Connections = new List<Connection>();
	    }

        /// <summary>
        /// Returns the Hardwarenode with the name.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <returns>The Hardwarenode with this name or default value</returns>
        public Hardwarenode GetHardwarenodeByName(string Name)
	    {
	        return nodes.FirstOrDefault(N => N.Name == Name);
	    }

        /// <summary>
        /// Adds a hardwarenode.
        /// </summary>
        /// <param name="NewNode">The new node.</param>
        public void AddHardwarenode(Hardwarenode NewNode)
	    {
	        nodes.Add(NewNode);
	    }

        /// <summary>
        /// Adds the connection.
        /// </summary>
        /// <param name="StartNodeInterfaceName">Start name of the node interface.</param>
        /// <param name="EndNodeInterfaceName">End name of the node interface.</param>
        /// <param name="NewConnection">The new connection.</param>
        /// <returns>True on success, false if the connection could not be added because
        /// the connection already exists or the connection contains an invalid start- or end-node or if 
        /// the interface of the start- or endnode is already used.</returns>
        public bool AddConnection(string StartNodeInterfaceName, string EndNodeInterfaceName, Connection NewConnection)
	    {
            if (!nodes.Contains(NewConnection.Start) || !nodes.Contains(NewConnection.End))
            {
                // Start or end-node do not exist!
                return false;
            }
            if (Connections.Count(C => C.Equals(NewConnection)) > 0)
            {
                // Connection already exists!
                return false;
            }

            if (NewConnection.Start.InterfaceIsUsed(StartNodeInterfaceName))
            {
                // Interface of startnode is already used!
                return false;
            }

            if (NewConnection.End.InterfaceIsUsed(EndNodeInterfaceName))
            {
                // Interface of endnode is already used!
                return false;
            }
            
            NewConnection.Start.AddConnection(StartNodeInterfaceName, NewConnection);
            NewConnection.End.AddConnection(EndNodeInterfaceName, NewConnection);
            Connections.Add(NewConnection);

            return true;
	    }

        /// <summary>
        /// Removes the hardwarnode.
        /// </summary>
        /// <param name="Name">The name.</param>
        public void RemoveHardwarnode(string Name)
        {
            nodes.RemoveAll(S => S.Name == Name);
        }

        /// <summary>
        /// Removes the connection.
        /// </summary>
        /// <param name="ConnectionName">Name of the connection.</param>
        public void RemoveConnection(string ConnectionName)
	    {
            var connection = Connections.FirstOrDefault(C => C.Name == ConnectionName);

            if (connection == null) return;

            Connections.Remove(connection);
            connection.Start.RemoveConnection(Interface.NamePrefix + connection.GetPortIndex(connection.Start));
            connection.End.RemoveConnection(Interface.NamePrefix + connection.GetPortIndex(connection.End));
        }

        /// <summary>
        /// Gets the workstation by ip.
        /// </summary>
        /// <param name="Ip">The ip.</param>
        /// <returns></returns>
        public Hardwarenode GetWorkstationByIp(IPAddress Ip)
        {
            return nodes.FirstOrDefault(H => H.HasIp(Ip));
        }

        /// <summary>
        /// Gets all hardwarenodes.
        /// </summary>
        /// <returns>all Hardwarenodes</returns>
        public List<Hardwarenode> GetAllHardwarenodes()
        {
            return nodes.ToList();
        }

        /// <summary>
        /// Gets all workstations.
        /// </summary>
        /// <returns>all Workstations</returns>
        public List<Workstation> GetAllWorkstations()
        {
            return nodes.OfType<Workstation>().ToList();
        }

        /// <summary>
        /// Gets the name of the connection by.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <returns>the connection with its name</returns>
        public Connection GetConnectionByName(string Name)
        {
            return Connections.FirstOrDefault(N => N.Name == Name);
        }

        /// <summary>
        /// Gets the routers with internetconnection.
        /// </summary>
        /// <returns>A List of routers</returns>
        public List<Router> GetRouters()
        {
            return nodes.OfType<Router>().Where(R => R.IsGateway).ToList();
        }

        public List<Connection> GetAllConnections()
	    {
            return Connections.ToList();
	    }

        /// <summary>
        /// Gets all hardwarenodes belonging to a subnet.
        /// </summary>
        /// <param name="Subnetmask">The subnetmask.</param>
        /// <returns>A list of hardwarenodes who belong to the subnet.</returns>
        public List<Workstation> GetHardwareNodesForSubnet(string Subnetmask)
        {
            List<Workstation> resultNodes = new List<Workstation>();
            IPAddress subnetAddress;
            bool ok = IPAddress.TryParse(Subnetmask, out subnetAddress);
            Debug.Assert(ok, "Invalid Subnetmask");

            List<Workstation> allWorkstations = GetAllWorkstations();
            // Iterate through all workstations
            foreach (Workstation w in allWorkstations)
            {
                List<Interface> ifaces = w.Interfaces;
                // Iterate through all interfaces of the current workstation.
                foreach (Interface iface in ifaces)
                {
                    if (subnetAddress.Equals(iface.Subnetmask))
                    {
                        // Workstation is in the same subnet.
                        resultNodes.Add(w);
                        break;
                    }
                }

            }

            return resultNodes;
        }
    }
}
