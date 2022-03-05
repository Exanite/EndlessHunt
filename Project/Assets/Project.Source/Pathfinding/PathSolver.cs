using System.Collections.Generic;
using UnityEngine;

namespace Project.Source.Pathfinding
{
    public class PathSolver
    {
        private struct NodeData
        {
            public PathfindingNode Parent;
            public float FCost;
            public float GCost;
        }

        private NodeData[] NodeDataCache;
        
        private readonly List<PathfindingNode> open = new List<PathfindingNode>();
        private readonly HashSet<PathfindingNode> closed = new HashSet<PathfindingNode>();

        private readonly List<PathfindingNode> neighborResultsCache = new List<PathfindingNode>();
        private readonly List<PathfindingNode> isDirectlyWalkableCache = new List<PathfindingNode>();

        public Path FindPath(PathfindingGrid grid, PathfindingNode start, PathfindingNode destination, Heuristic heuristic = null)
        {
            if (heuristic == null)
            {
                heuristic = Heuristics.Default;
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
            var success = false;
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
                    success = true;

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

            Path path = null;

            if (success)
            {
                var nodes = RetracePath(start, destination);

                path = new Path(nodes);
            }
            
            return path;
        }

        private List<PathfindingNode> RetracePath(PathfindingNode start, PathfindingNode destination)
        {
            var result = new List<PathfindingNode>();
            var current = destination;

            while (current != start)
            {
                result.Add(current);

                current = NodeDataCache[current.Index].Parent;
            }

            result.Reverse();

            return result;
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
    }
}