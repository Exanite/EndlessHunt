using UnityEngine;

namespace Project.Source.Pathfinding
{
    public class Heuristics
    {
        public static float Default(PathfindingNode a, PathfindingNode b)
        {
            return Euclidean(a, b);
        }

        public static float Euclidean(PathfindingNode a, PathfindingNode b)
        {
            return Vector3.Distance(a.Position, b.Position);
        }

        public static float Manhattan(PathfindingNode a, PathfindingNode b)
        {
            var dx = Mathf.Abs(a.Position.x - b.Position.x);
            var dy = Mathf.Abs(a.Position.y - b.Position.y);
            var dz = Mathf.Abs(a.Position.z - b.Position.z);

            return dx + dy + dz;
        }
    }
}