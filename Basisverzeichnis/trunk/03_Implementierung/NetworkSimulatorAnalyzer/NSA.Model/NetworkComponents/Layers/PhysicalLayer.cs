﻿using System.Collections.Generic;
using System.Net;
using NSA.Model.NetworkComponents.Helper_Classes;

namespace NSA.Model.NetworkComponents.Layers
{
    public class PhysicalLayer : ILayer
    {
        public bool ValidateReceive(Workstation currentNode, ValidationInfo valInfo, Dictionary<string, object> Tags, Hardwarenode destination)
        {
            return true;
        }

        public string GetLayerName()
        {
            return "Bitübertragungsschicht";
        }

        public bool SetLayerName(string newName)
        {
            return false;
        }

        public void ValidateSend(Workstation destination, Workstation currentNode, ValidationInfo valInfo, Dictionary<string, object> Tags)
        {
            
        }
    }
}
