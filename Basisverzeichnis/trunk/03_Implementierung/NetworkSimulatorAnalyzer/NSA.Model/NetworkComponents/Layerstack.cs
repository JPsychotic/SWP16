﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NSA.Model.NetworkComponents
{
    /// <summary>
    /// Layerstack
    /// </summary>
    public class Layerstack
    {
        private readonly List<ILayer> layers;

        /// <summary>
        /// Initializes a new instance of the <see cref="Layerstack"/> class.
        /// </summary>
        public Layerstack()
        {
            layers = new List<ILayer>();
        }

        /// <summary>
        /// Adds a layer to the stack.
        /// </summary>
        /// <param name="Lay">The layer to be added.</param>
        public void AddLayer(ILayer Lay)
        {
            layers.Add(Lay);
        }

        /// <summary>
        /// Removes a layer from the stack.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <exception cref="System.InvalidOperationException">Layer with the name:  + name +  does not exist.</exception>
        public void RemoveLayer(string Name)
        {
            bool decrease = false;
            List<ILayer> list = layers.OrderBy(I => I.GetLayerIndex()).ToList();
            foreach (ILayer l in list)
            {
                if (decrease)
                    l.SetLayerIndex(l.GetLayerIndex() - 1);
                if (l.GetLayerName().Equals(Name))
                    decrease = true;
            }
            foreach (ILayer l in layers)
            {
                if (l.GetLayerName() != Name) continue;
                layers.Remove(l);
                return;
            }
            throw new InvalidOperationException("Layer with the name: " + Name + " does not exist.");
        }

        /// <summary>
        /// Returns the size of the layerstack.
        /// </summary>
        /// <returns>The size</returns>
        public int GetSize()
        {
            return layers.Count;
        }

        /// <summary>
        /// Returns the layer at the index.
        /// </summary>
        /// <param name="Index">The index.</param>
        /// <returns>The layer</returns>
        public ILayer GetLayer(int Index)
        {
            return layers.OrderBy(I => I.GetLayerIndex()).ToList()[Index];
        }

        /// <summary>
        /// Inserts a layer at the index.
        /// </summary>
        /// <param name="Index">The index.</param>
        /// <param name="Layer">The layer.</param>
        public void InsertAt(int Index, ILayer Layer)
        {
            List<ILayer> list = layers.OrderBy(I => I.GetLayerIndex()).ToList();
            for (int i = Index; i < list.Count; i++)
            {
                list[i].SetLayerIndex(list[i].GetLayerIndex() + 1);
            }
            AddLayer(Layer);
        }

        /// <summary>
        /// Sets the index.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="NewIndex">The new index.</param>
        public void SetIndex(string Name, int NewIndex)
        {
            ILayer l = GetLayerByName(Name);
            l.SetLayerIndex(NewIndex);
            RemoveLayer(Name);
            InsertAt(NewIndex, l);
        }

        /// <summary>
        /// Gets the layer with the name.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <returns></returns>
        public ILayer GetLayerByName(string Name)
        {
            return layers.FirstOrDefault(L => L.GetLayerName().Equals(Name));
        }

        /// <summary>
        /// Sets the name.
        /// </summary>
        /// <param name="OldName">The old name.</param>
        /// <param name="NewName">The new name.</param>
        /// <returns>True if it worked. False if the newName is already taken by another layer or when there is no layer with the old name</returns>
        public bool SetName(string OldName, string NewName)
        {
            foreach (ILayer l in layers)
            {
                if (l.GetLayerName() == NewName)
                    return false;
            }
            foreach (ILayer l in layers)
            {
                if (l.GetLayerName() == OldName)
                {
                    return l.SetLayerName(NewName);
                }
            }
            return false;
        }

        /// <summary>
        /// Gets all layers.
        /// </summary>
        /// <returns></returns>
        public List<ILayer> GetAllLayers()
        {
            return layers.OrderBy(I => I.GetLayerIndex()).ToList();
        }

        /// <summary>
        /// Determines whether the name is taken or not.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <returns></returns>
        public bool IsNameTaken(string Name)
        {
            foreach (ILayer l in layers)
            {
                if (l.GetLayerName() == Name)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Creates a new unique name.
        /// </summary>
        /// <returns></returns>
        public string CreateUniqueName()
        {
            for (int i = 1; ; i++)
            {
                string name = $"Neue Schicht {i}";
                if (layers.All(L => L.GetLayerName() != name)) return name;
            }
        }
    }
}
