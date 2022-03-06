using System.Collections.Generic;
using UnityEngine;

namespace Project.Source.Pathfinding
{
    public class PathSolver
    {
        private NodeData[] NodeDataCache;

        private readonly List<PathfindingNode> open;
        private readonly HashSet<PathfindingNode> closed;

        private readonly Heuristic heuristic;

        public PathSolver(PathfindingGrid grid, Heuristic heuristic = null)
        {
            this.Grid = grid;
            this.heuristic = heuristic ?? Heuristics.Default;

            open = new List<PathfindingNode>();
            closed = new HashSet<PathfindingNode>();
        }

        public PathfindingGrid Grid { get; }

        public Path FindPath(Vector3 start, Vector3 destination, Path path = null)
        {
            return FindPath(Grid.WorldPositionToNode(start), Grid.WorldPositionToNode(destination), path);
        }

        public Path FindPath(PathfindingNode start, PathfindingNode destination, Path path = null)
        {
            if (path == null)
            {
                path = new Path();
            }

            if (start == null
                || destination == null
                || !destination.IsWalkable)
            {
                return null;
            }

            Prepare(Grid);

            open.Add(start);
            NodeDataCache[start.Index].FCost = 0;
            NodeDataCache[start.Index].GCost = 0;
            NodeDataCache[start.Index].IsOpen = true;

            PathfindingNode current;
            var isSuccess = false;
            var openPathfindingNodeCounter = 1;

            while (open.Count > 0)
            {
                current = open[0];

                for (var i = 1; i < open.Count; i++)
                {
                    if (NodeDataCache[open[i].Index].FCost < NodeDataCache[current.Index].FCost)
                    {
                        current = open[i];
                    }
                }

                open.Remove(current);
                closed.Add(current);
                NodeDataCache[current.Index].IsOpen = false;

                if (current == destination)
                {
                    isSuccess = true;

                    break;
                }

                foreach (var neighbor in current.Neighbors)
                {
                    if (!neighbor.IsWalkable)
                    {
                        continue;
                    }

                    if (closed.Contains(neighbor))
                    {
                        continue;
                    }

                    if (!NodeDataCache[neighbor.Index].IsOpen)
                    {
                        openPathfindingNodeCounter++;

                        open.Add(neighbor);
                        NodeDataCache[neighbor.Index].IsOpen = true;

                        NodeDataCache[neighbor.Index].GCost = float.PositiveInfinity;
                        NodeDataCache[neighbor.Index].Parent = current;
                    }

                    var newGCost = NodeDataCache[current.Index].GCost + heuristic(current, neighbor);

                    if (newGCost < NodeDataCache[neighbor.Index].GCost)
                    {
                        NodeDataCache[neighbor.Index].GCost = newGCost;
                        NodeDataCache[neighbor.Index].Parent = current;
                    }

                    NodeDataCache[neighbor.Index].FCost = NodeDataCache[neighbor.Index].GCost + heuristic(neighbor, destination);
                }
            }

            // Debug.Log("Finished pathfinding. " +
            //     $"Opened {openPathfindingNodeCounter} PathfindingNodes and closed {closed.Count} PathfindingNodes");

            path.IsValid = isSuccess;

            if (isSuccess)
            {
                RetracePath(start, destination, path);
            }

            return path;
        }

        private void RetracePath(PathfindingNode start, PathfindingNode destination, Path path)
        {
            path.Waypoints.Clear();

            var current = destination;
            while (current != start)
            {
                path.Waypoints.Add(current.Position);

                current = NodeDataCache[current.Index].Parent;
            }
        }

        private void Prepare(PathfindingGrid pathfindingGrid)
        {
            if (NodeDataCache == null || NodeDataCache.Length != pathfindingGrid.Nodes.Length)
            {
                NodeDataCache = new NodeData[pathfindingGrid.Nodes.Length];
            }

            for (var i = 0; i < NodeDataCache.Length; i++)
            {
                NodeDataCache[i] = new NodeData();
            }

            open.Clear();
            closed.Clear();
        }

        private struct NodeData
        {
            public PathfindingNode Parent;
            public float FCost;
            public float GCost;

            /// <summary>
            ///     Is the node in the open list. <br/>
            ///     Doesn't replace the use of the list, but optimizes
            ///     the contains check that is otherwise needed
            /// </summary>
            public bool IsOpen;
        }
    }
}