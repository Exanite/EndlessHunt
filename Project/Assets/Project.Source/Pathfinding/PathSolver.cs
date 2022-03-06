using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Source.Pathfinding
{
    public class PathSolver
    {
        private NodeData[] nodeDataCache;

        private readonly List<PathfindingNode> open;
        private readonly HashSet<PathfindingNode> closed;

        private readonly Heuristic heuristic;
        private readonly IPathProcessor[] pathProcessors;

        public PathSolver(PathfindingGrid grid, Heuristic heuristic = null, IEnumerable<IPathProcessor> pathProcessors = null)
        {
            Grid = grid;
            this.heuristic = heuristic ?? Heuristics.Default;
            this.pathProcessors = pathProcessors == null ? new IPathProcessor[0] : pathProcessors.ToArray();

            Path = new Path();

            open = new List<PathfindingNode>();
            closed = new HashSet<PathfindingNode>();
        }

        public PathfindingGrid Grid { get; }

        public Path Path { get; }

        public bool FindPath(Vector3 start, Vector3 destination)
        {
            return FindPath(Grid.WorldPositionToNode(start), Grid.WorldPositionToNode(destination));
        }

        public bool FindPath(PathfindingNode start, PathfindingNode destination)
        {
            if (start == null
                || destination == null
                || !destination.IsWalkable)
            {
                return false;
            }

            Prepare(Grid);

            open.Add(start);
            nodeDataCache[start.Index].FCost = 0;
            nodeDataCache[start.Index].GCost = 0;
            nodeDataCache[start.Index].IsOpen = true;

            PathfindingNode current;
            var isSuccess = false;

            while (open.Count > 0)
            {
                current = open[0];

                for (var i = 1; i < open.Count; i++)
                {
                    if (nodeDataCache[open[i].Index].FCost < nodeDataCache[current.Index].FCost)
                    {
                        current = open[i];
                    }
                }

                open.Remove(current);
                closed.Add(current);
                nodeDataCache[current.Index].IsOpen = false;

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

                    if (!nodeDataCache[neighbor.Index].IsOpen)
                    {
                        open.Add(neighbor);
                        nodeDataCache[neighbor.Index].IsOpen = true;

                        nodeDataCache[neighbor.Index].GCost = float.PositiveInfinity;
                        nodeDataCache[neighbor.Index].Parent = current;
                    }

                    var newGCost = nodeDataCache[current.Index].GCost + heuristic(current, neighbor);

                    if (newGCost < nodeDataCache[neighbor.Index].GCost)
                    {
                        nodeDataCache[neighbor.Index].GCost = newGCost;
                        nodeDataCache[neighbor.Index].Parent = current;
                    }

                    nodeDataCache[neighbor.Index].FCost = nodeDataCache[neighbor.Index].GCost + heuristic(neighbor, destination);
                }
            }

            Path.IsValid = isSuccess;

            if (isSuccess)
            {
                RetracePath(start, destination, Path);
            }
            
            ProcessPath(Path);

            return Path.IsValid;
        }

        private void RetracePath(PathfindingNode start, PathfindingNode destination, Path path)
        {
            path.Waypoints.Clear();

            var current = destination;
            while (current != start)
            {
                path.Waypoints.Add(current.Position);

                current = nodeDataCache[current.Index].Parent;
            }
        }

        private void ProcessPath(Path path)
        {
            foreach (var pathProcessor in pathProcessors)
            {
                pathProcessor.Process(path);
            }
        }

        private void Prepare(PathfindingGrid pathfindingGrid)
        {
            if (nodeDataCache == null || nodeDataCache.Length < pathfindingGrid.Nodes.Length)
            {
                nodeDataCache = new NodeData[pathfindingGrid.Nodes.Length];
            }

            for (var i = 0; i < nodeDataCache.Length; i++)
            {
                nodeDataCache[i] = new NodeData();
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