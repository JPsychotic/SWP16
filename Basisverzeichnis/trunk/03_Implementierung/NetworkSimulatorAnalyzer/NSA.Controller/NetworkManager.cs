using System;
using NSA.Model.NetworkComponents;

namespace NSA.Controller
{
    internal class NetworkManager
    {
        public Network Network1 { get; }

        // Default constructor:
        public NetworkManager()
        {
            createConfigControls();
        }

        // Constructor:
        public NetworkManager(Network network)
        {
            Network1 = network;
            createConfigControls();
        }

        private void createConfigControls()
        {

        }

        public void HardwarenodeSelected()
        {

        }

        public void InterfaceChanged()
        {

        }

        public void RouteChanged()
        {

        }

        public void GatewayChanged()
        {

        }

        public void CreateHardwareNode(string name, Enum typ)
        {

        }

        public void CreateConnection(string start, string end)
        {

        }

        public void RemoveHardwarenode(string name)
        {

        }

        public void RemoveConnection(string name)
        {

        }
    }
}
