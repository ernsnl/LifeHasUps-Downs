using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Models
{
    public class Graph<T> : ICloneable
    {
        private List<GraphNode<T>> _graphNodeSet;

        public Graph()
        {
            _graphNodeSet = new List<GraphNode<T>>();
        }

        public void AddNode(GraphNode<T> node)
        {
            // adds a node to the graph
            _graphNodeSet.Add(node);
        }

        public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
        }

        public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);

            to.Neighbors.Add(from);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public List<GraphNode<T>> GraphNodes
        {
            get { return _graphNodeSet; }
        } 

        public int Count
        {
            get { return _graphNodeSet.Count; }
        }

    }
}

