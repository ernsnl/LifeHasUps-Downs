using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using Assets.Main;
using Assets.Scripts.Misc.CustomFunctions;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Misc.Handlers
{
    public class WorldHandler
    {
        private int _totalExploredNodes;

        public string seedNumberLocal;

        private Graph<TileHandler> EmptyWorld()
        {
            Graph<TileHandler> emptyWorld = new Graph<TileHandler>();
            return emptyWorld;
        }

        public Graph<TileHandler> BuildWorld(int minimumLevelSize, string seedNumber)
        {
            Graph<TileHandler> fullWorld = EmptyWorld();

            if (!string.IsNullOrEmpty(seedNumber))
                seedNumberLocal = seedNumber;
            else
                seedNumberLocal = GameHandler.Game.Random.RandomString(12);

            GameHandler.Game.Random.SetSeed(seedNumberLocal);

            Queue<GraphNode<TileHandler>> queue = new Queue<GraphNode<TileHandler>>();

            TileHandler model = new TileHandler();
            model.SetPosition(0, 0);
            GraphNode<TileHandler> graphModel = new GraphNode<TileHandler>(model);
            fullWorld.AddNode(graphModel);

            queue.Enqueue(graphModel);

            while (fullWorld.GraphNodes.Count <= minimumLevelSize && queue.Count != 0)
            {
                var dq = queue.Dequeue();

                #region PlacementControl

                #region East

                if (fullWorld.GraphNodes.Any(op => op.NodeObject.IsPlaced &&
                op.NodeObject.TileGateModel.westGate == 1 &&
                 op.NodeObject.PositionX == (dq.NodeObject.PositionX + 1) &&
                 op.NodeObject.PositionY == dq.NodeObject.PositionY))
                {
                    dq.NodeObject.TileGateModel.isCertainEast = true;
                }

                if (fullWorld.GraphNodes.Any(op => op.NodeObject.IsPlaced &&
                op.NodeObject.TileGateModel.westGate == 0 &&
                op.NodeObject.PositionX == (dq.NodeObject.PositionX + 1) &&
                op.NodeObject.PositionY == dq.NodeObject.PositionY))
                {
                    dq.NodeObject.TileGateModel.eastGate = 0;
                }

                #endregion

                #region North
                if (fullWorld.GraphNodes.Any(op => op.NodeObject.IsPlaced && op.NodeObject.TileGateModel.southGate == 1 &&
             op.NodeObject.PositionX == (dq.NodeObject.PositionX) &&
             op.NodeObject.PositionY == dq.NodeObject.PositionY + 1))
                {
                    dq.NodeObject.TileGateModel.isCertainNorth = true;
                }

                if (fullWorld.GraphNodes.Any(op => op.NodeObject.IsPlaced && op.NodeObject.TileGateModel.southGate == 0 &&
             op.NodeObject.PositionX == (dq.NodeObject.PositionX) &&
             op.NodeObject.PositionY == dq.NodeObject.PositionY + 1))
                {
                    dq.NodeObject.TileGateModel.northGate = 0;
                }

                #endregion

                #region West

                if (fullWorld.GraphNodes.Any(op => op.NodeObject.IsPlaced && op.NodeObject.TileGateModel.eastGate == 1 &&
                 op.NodeObject.PositionX == (dq.NodeObject.PositionX - 1) &&
                 op.NodeObject.PositionY == dq.NodeObject.PositionY))
                {
                    dq.NodeObject.TileGateModel.isCertainWest = true;
                }

                if (fullWorld.GraphNodes.Any(op => op.NodeObject.IsPlaced && op.NodeObject.TileGateModel.eastGate == 0 &&
                 op.NodeObject.PositionX == (dq.NodeObject.PositionX - 1) &&
                 op.NodeObject.PositionY == dq.NodeObject.PositionY))
                {
                    dq.NodeObject.TileGateModel.westGate = 0;
                }
                #endregion

                #region South
                if (fullWorld.GraphNodes.Any(op => op.NodeObject.IsPlaced && op.NodeObject.TileGateModel.northGate == 1 &&
                 op.NodeObject.PositionX == (dq.NodeObject.PositionX) &&
                 op.NodeObject.PositionY == dq.NodeObject.PositionY - 1))
                {
                    dq.NodeObject.TileGateModel.isCertainSouth = true;
                }
                if (fullWorld.GraphNodes.Any(op => op.NodeObject.IsPlaced && op.NodeObject.TileGateModel.northGate == 0 &&
                 op.NodeObject.PositionX == (dq.NodeObject.PositionX) &&
                 op.NodeObject.PositionY == dq.NodeObject.PositionY - 1))
                {
                    dq.NodeObject.TileGateModel.southGate = 0;
                }
                #endregion

                #endregion
                var selectedTile = dq.NodeObject.SelectTile();

                // Make it so every node can be visited
                foreach (var node in fullWorld.GraphNodes)
                {
                    node.NodeObject.Explored = false;
                }

                //Should Check Whether The Loop Is Closed When It has not supposed to be
                while (fullWorld.GraphNodes.Count > 1 && !GraphIsConsistent(dq, selectedTile, minimumLevelSize))
                {
                    selectedTile = dq.NodeObject.SelectTile();
                    // Make it so every node can be visited
                    foreach (var node in fullWorld.GraphNodes)
                    {
                        node.NodeObject.Explored = false;
                    }
                }
                dq.NodeObject.PlaceTile(selectedTile);


                if (dq.NodeObject.TileGateModel.southGate == 1
                    && !dq.NodeObject.TileGateModel.isCertainSouth
                    && !fullWorld.GraphNodes.Any(op =>
                        op.NodeObject.PositionX == (dq.NodeObject.PositionX)
                            && op.NodeObject.PositionY == dq.NodeObject.PositionY - 1))
                {
                    var newGraphNode = new GraphNode<TileHandler>(new TileHandler());
                    newGraphNode.NodeObject.SetPosition(dq.NodeObject.PositionX, dq.NodeObject.PositionY - 1);
                    newGraphNode.NodeObject.TileGateModel.isCertainNorth = true;

                    fullWorld.AddNode(newGraphNode);
                    fullWorld.AddUndirectedEdge(dq, newGraphNode, 0);

                    queue.Enqueue(newGraphNode);
                }


                if (dq.NodeObject.TileGateModel.eastGate == 1
                    && !dq.NodeObject.TileGateModel.isCertainEast
                     && !fullWorld.GraphNodes.Any(op =>
                        op.NodeObject.PositionX == (dq.NodeObject.PositionX + 1)
                            && op.NodeObject.PositionY == dq.NodeObject.PositionY))
                {
                    var newGraphNode = new GraphNode<TileHandler>(new TileHandler());
                    newGraphNode.NodeObject.SetPosition(dq.NodeObject.PositionX + 1, dq.NodeObject.PositionY);
                    newGraphNode.NodeObject.TileGateModel.isCertainWest = true;

                    fullWorld.AddNode(newGraphNode);
                    fullWorld.AddUndirectedEdge(dq, newGraphNode, 0);

                    queue.Enqueue(newGraphNode);
                }

                if (dq.NodeObject.TileGateModel.westGate == 1
                    && !dq.NodeObject.TileGateModel.isCertainWest
                     && !fullWorld.GraphNodes.Any(op =>
                        op.NodeObject.PositionX == (dq.NodeObject.PositionX - 1)
                            && op.NodeObject.PositionY == dq.NodeObject.PositionY))
                {
                    var newGraphNode = new GraphNode<TileHandler>(new TileHandler());
                    newGraphNode.NodeObject.SetPosition(dq.NodeObject.PositionX - 1, dq.NodeObject.PositionY);
                    newGraphNode.NodeObject.TileGateModel.isCertainEast = true;

                    fullWorld.AddNode(newGraphNode);
                    fullWorld.AddUndirectedEdge(dq, newGraphNode, 0);

                    queue.Enqueue(newGraphNode);
                }

                if (dq.NodeObject.TileGateModel.northGate == 1
                    && !dq.NodeObject.TileGateModel.isCertainNorth
                     && !fullWorld.GraphNodes.Any(op =>
                        op.NodeObject.PositionX == (dq.NodeObject.PositionX)
                            && op.NodeObject.PositionY == dq.NodeObject.PositionY + 1))
                {
                    var newGraphNode = new GraphNode<TileHandler>(new TileHandler());
                    newGraphNode.NodeObject.SetPosition(dq.NodeObject.PositionX, dq.NodeObject.PositionY + 1);
                    newGraphNode.NodeObject.TileGateModel.isCertainSouth = true;

                    fullWorld.AddNode(newGraphNode);
                    fullWorld.AddUndirectedEdge(dq, newGraphNode, 0);

                    queue.Enqueue(newGraphNode);
                }

            }

            var unPlacedNodes = fullWorld.GraphNodes.ToList().FindAll(op => op.NodeObject.IsPlaced == false);
            for (int i = 0; i < unPlacedNodes.Count; i++)
            {
                var unPlacedNode = unPlacedNodes[i];

                var eastNode = fullWorld.GraphNodes.ToList().FirstOrDefault(op => op.NodeObject.IsPlaced && op.NodeObject.PositionX == unPlacedNode.NodeObject.PositionX + 1 && op.NodeObject.PositionY == unPlacedNode.NodeObject.PositionY);
                var westNode = fullWorld.GraphNodes.ToList().FirstOrDefault(op => op.NodeObject.IsPlaced && op.NodeObject.PositionX == unPlacedNode.NodeObject.PositionX - 1 && op.NodeObject.PositionY == unPlacedNode.NodeObject.PositionY);
                var northNode = fullWorld.GraphNodes.ToList().FirstOrDefault(op => op.NodeObject.IsPlaced && op.NodeObject.PositionX == unPlacedNode.NodeObject.PositionX && op.NodeObject.PositionY == unPlacedNode.NodeObject.PositionY + 1);
                var southNode = fullWorld.GraphNodes.ToList().FirstOrDefault(op => op.NodeObject.IsPlaced && op.NodeObject.PositionX == unPlacedNode.NodeObject.PositionX && op.NodeObject.PositionY == unPlacedNode.NodeObject.PositionY - 1);

                if (eastNode != null)
                    unPlacedNode.NodeObject.TileGateModel.isCertainEast = eastNode.NodeObject.TileGateModel.westGate == 1;
                if (westNode != null)
                    unPlacedNode.NodeObject.TileGateModel.isCertainWest = westNode.NodeObject.TileGateModel.eastGate == 1;
                if (northNode != null)
                    unPlacedNode.NodeObject.TileGateModel.isCertainNorth = northNode.NodeObject.TileGateModel.southGate == 1;
                if (southNode != null)
                    unPlacedNode.NodeObject.TileGateModel.isCertainSouth = southNode.NodeObject.TileGateModel.northGate == 1;

                unPlacedNode.NodeObject.CloseWorld();
            }


            if (fullWorld.Count <= minimumLevelSize)
            {
                fullWorld = BuildWorld(minimumLevelSize, seedNumberLocal);
            }

            return fullWorld;
        }

        private bool GraphIsConsistent(GraphNode<TileHandler> Node, TileHandler selectedTile, int minimumLevelSize)
        {
            Queue<GraphNode<TileHandler>> q = new Queue<GraphNode<TileHandler>>();
            int totalPlacedTile = 1;
            q.Enqueue(Node);
            if (selectedTile.NumberOfPossibleNeighBors > Node.Neighbors.Count)
                return true;
            while (q.Count > 0)
            {
                var node = q.Dequeue();
                node.NodeObject.Explored = true;
                if (node.Neighbors.Count < node.NodeObject.NumberOfPossibleNeighBors)
                    return true;
                if (!node.NodeObject.IsPlaced)
                    return true;
                foreach (var neighbor in node.Neighbors)
                {
                    if (!neighbor.NodeObject.Explored)
                    {
                        totalPlacedTile++;
                        q.Enqueue(neighbor);
                    }
                }
            }
            if (totalPlacedTile > minimumLevelSize)
                return true;
            return false;
        }
    }
}