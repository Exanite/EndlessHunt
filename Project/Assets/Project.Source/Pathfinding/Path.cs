using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Source.Pathfinding
{
    public class Path
    {
        public bool IsValid;

        public Path()
        {
            Waypoints = new List<Vector3>();
        }
        
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

        public List<Vector3> Waypoints { get; }

        public float Length
        {
            get
            {
                float result = 0;

                for (var i = 0; i < Waypoints.Count - 1; i++)
                {
                    result += (Waypoints[i] - Waypoints[i + 1]).magnitude;
                }

                return result;
            }
        }

        public bool HasNext()
        {
            return Waypoints.Count > 0;
        }
        
        public Vector3 GetNext()
        {
            return Waypoints[Waypoints.Count - 1];
        }

        public bool TryGetNext(out Vector3 waypoint)
        {
            waypoint = Vector3.zero;

            if (HasNext())
            {
                waypoint = GetNext();

                return true;
            }

            return false;
        }
        
        public void Pop()
        {
            Waypoints.RemoveAt(Waypoints.Count - 1);
        }

        public void DrawWithGizmos()
        {
            for (var i = 0; i < Waypoints.Count - 1; i++)
            {
                Gizmos.DrawLine(Waypoints[i], Waypoints[i + 1]);
            }
        }
    }
}