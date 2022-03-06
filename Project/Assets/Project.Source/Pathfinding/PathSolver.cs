using System.Collections.Generic;
using UnityEngine;

namespace Project.Source.Pathfinding
{
    public class PathSolver
    {
        private NodeData[] NodeDataCache;

        private readonly List<PathfindingNode> open;
        private readonly HashSet<PathfindingNode> closed;

        private readonly PathfindingGrid grid;

        public PathSolver(PathfindingGrid grid)
        {
            this.grid = grid;

            open = new List<PathfindingNode>();
            closed = new HashSet<PathfindingNode>();
        }

        public PathfindingGrid Grid => grid;

        public Path FindPath(Vector3 start, Vector3 destination, Path path = null, Heuristic heuristic = null)
        {
            return FindPath(grid.WorldPositionToNode(start), grid.WorldPositionToNode(destination), path, heuristic);
        }

        public Path FindPath(PathfindingNode start, PathfindingNode destination, Path path = null, Heuristic heuristic = null)
        {
            if (heuristic == null)
            {
                heuristic = Heuristics.Default;
            }

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

            Prepare(grid);

            open.Add(start);
            NodeDataCache[start.Index].FCost = 0;
            NodeDataCache[start.Index].GCost = 0;

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

                    if (!open.Contains(neighbor))
                    {
                        openPathfindingNodeCounter++;

                        open.Add(neighbor);

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

            path.Waypoints.Reverse();
        }

        private void Prepare(PathfindingGrid pathfindingGrid)
        {
            if (NodeDataCache == null || NodeDataCache.Length != pathfindingGrid.Nodes.Length)
            {
                NodeDataCache = new NodeData[pathfindingGrid.Nodes.Length];
            }

            open.Clear();
            closed.Clear();
        }

        private struct NodeData
        {
            public PathfindingNode Parent;
            public float FCost;
            public float GCost;
        }
    }
}