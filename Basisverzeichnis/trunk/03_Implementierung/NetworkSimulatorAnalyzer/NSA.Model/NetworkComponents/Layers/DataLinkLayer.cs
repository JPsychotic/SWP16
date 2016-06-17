﻿using System.Collections.Generic;
using NSA.Model.NetworkComponents.Helper_Classes;

namespace NSA.Model.NetworkComponents.Layers
{
    public class DataLinkLayer : ILayer
    {
        public void ValidateSend(Workstation Destination, Workstation CurrentNode, ValidationInfo ValInfo, Dictionary<string, object> Tags)
        {
            if (ValInfo.Iface == null)
                return;
            if (CurrentNode.GetConnections().ContainsKey(ValInfo.Iface.Name))
            {
                ValInfo.NextNodes.Add(CurrentNode.GetConnections()[ValInfo.Iface.Name].Start.Equals(CurrentNode) ? CurrentNode.GetConnections()[ValInfo.Iface.Name].End : CurrentNode.GetConnections()[ValInfo.Iface.Name].Start);
                return;
            }
            ValInfo.Res.ErrorId = 2;
            ValInfo.Res.Res = "There is no Connection at the Interface from choosen the Route.";
            ValInfo.Res.LayerError = new DataLinkLayer();
            ValInfo.Res.SendError = true;
            ValInfo.NextNodes = null;
        }

        public bool ValidateReceive(Workstation CurrentNode, ValidationInfo ValInfo, Dictionary<string, object> Tags, Hardwarenode Destination)
        {
            if (ValInfo.NextNodeIp == null)
                return true;
            List<Interface> ifaces = CurrentNode.GetInterfaces();
            foreach (Interface i in ifaces)
            {
                if (ValInfo.NextNodeIp.Equals(i.IpAddress))
                    return true;
            }
            ValInfo.Res.ErrorId = 3;
            ValInfo.Res.Res = "The Connection is to the wrong node.";
            ValInfo.Res.LayerError = new DataLinkLayer();
            ValInfo.Res.SendError = false;
            return false;
        }

        public string GetLayerName()
        {
            return "Sicherungsschicht";
        }

        public bool SetLayerName(string NewName)
        {
            return false;
        }
    }
}
