using UnityEngine;

namespace Project.Source.Pathfinding
{
    public class StraightPathProcessor : IPathProcessor
    {
        public void Process(Path path)
        {
            var waypoints = path.Waypoints;
            
            for (var i = waypoints.Count - 1; i >= 2; i--)
            {
                var pointA = waypoints[i];
                var pointB = waypoints[i - 2];

                var hit = Physics2D.Linecast(pointA, pointB, GameSettings.Instance.NonWalkableLayerMask);
                var canMerge = !hit.collider;
                
                if (canMerge)
                {
                    waypoints.RemoveAt(i - 1);
                }
            }
        }
    }
}