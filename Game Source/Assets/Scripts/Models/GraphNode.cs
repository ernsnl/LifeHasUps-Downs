using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models
{
    public class GraphNode<T> : ICloneable
    {

        private List<GraphNode<T>> _neighbors;
        private T _object;


        public GraphNode() { }

        public GraphNode(T value)
        {
            _object = value;
        }

        public T NodeObject
        {
            get { return _object; }
            set { _object = value; }
        } 

        public List<GraphNode<T>> Neighbors
        {
            get
            {
                if (this._neighbors == null)
                    this._neighbors = new List<GraphNode<T>>();

                return this._neighbors;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}