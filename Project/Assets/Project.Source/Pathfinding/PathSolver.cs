using System.Collections.Generic;
using UnityEngine;

namespace Project.Source.Pathfinding
{
    public class PathSolver
    {
        private readonly Dictionary<PathfindingNode, PathfindingNode> parent = new Dictionary<PathfindingNode, PathfindingNode>();
        private readonly Dictionary<PathfindingNode, float> fCost = new Dictionary<PathfindingNode, float>();
        private readonly Dictionary<PathfindingNode, float> gCost = new Dictionary<PathfindingNode, float>();

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

            CleanupPathfindingData();

            open.Add(start);
            fCost[start] = 0;
            gCost[start] = 0;

            PathfindingNode current;
            var success = false;
            var lineOfSightCheckCounter = 0;
            var openPathfindingNodeCounter = 1;

            while (open.Count > 0)
            {
                current = open[0];

                for (var i = 1; i < open.Count; i++)
                {
                    if (fCost[open[i]] < fCost[current])
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
                    if (closed.Contains(neighbor))
                    {
                        continue;
                    }

                    if (!open.Contains(neighbor))
                    {
                        openPathfindingNodeCounter++;

                        open.Add(neighbor);

                        gCost[neighbor] = float.PositiveInfinity;
                        parent[neighbor] = current;
                    }

                    var newGCost = gCost[current] + heuristic(current, neighbor);

                    if (newGCost < gCost[neighbor])
                    {
                        gCost[neighbor] = newGCost;
                        parent[neighbor] = current;
                    }

                    fCost[neighbor] = gCost[neighbor] + heuristic(neighbor, destination);
                }
            }

            Debug.Log("Finished pathfinding. " +
                $"Opened {openPathfindingNodeCounter} PathfindingNodes, " +
                $"closed {closed.Count} PathfindingNodes, and " +
                $"performed {lineOfSightCheckCounter} line of sight checks");

            Path path = null;

            if (success)
            {
                var nodes = RetracePath(start, destination);

                path = new Path(nodes);
            }

            CleanupPathfindingData();

            return path;
        }

        private List<PathfindingNode> RetracePath(PathfindingNode start, PathfindingNode destination)
        {
            var result = new List<PathfindingNode>();
            var current = destination;

            while (current != start)
            {
                result.Add(current);

                current = parent[current];
            }

            result.Reverse();

            return result;
        }

        private void CleanupPathfindingData()
        {
            open.Clear();
            closed.Clear();
        }
    }
}