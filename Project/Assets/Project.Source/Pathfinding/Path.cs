using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Source.Pathfinding
{
    public class Path
    {
        public Path(List<Vector3> waypoints)
        {
            Waypoints = waypoints ?? throw new ArgumentNullException(nameof(waypoints));
        }

        public Path(List<PathfindingNode> PathfindingNodes)
        {
            Waypoints = new List<Vector3>(PathfindingNodes.Count);

            foreach (var PathfindingNode in PathfindingNodes)
            {
                Waypoints.Add(PathfindingNode.Position);
            }
        }

        public List<Vector3> Waypoints { get; private set; }
    }
}