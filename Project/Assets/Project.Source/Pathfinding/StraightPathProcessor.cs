using UnityEngine;

namespace Project.Source.Pathfinding
{
    public class StraightPathProcessor : IPathProcessor
    {
        private readonly PhysicsScene2D physics;
        private readonly int nonWalkableLayerMask;

        public StraightPathProcessor(PhysicsScene2D physics, int nonWalkableLayerMask)
        {
            this.physics = physics;
            this.nonWalkableLayerMask = nonWalkableLayerMask;
        }
        
        public void Process(Path path)
        {
            var waypoints = path.Waypoints;
            
            for (var i = waypoints.Count - 1; i >= 2; i--)
            {
                var pointA = waypoints[i];
                var pointB = waypoints[i - 2];
                
                var hit = physics.Linecast(pointA, pointB, nonWalkableLayerMask);
                var canMerge = !hit.collider;
                
                if (canMerge)
                {
                    waypoints.RemoveAt(i - 1);
                }
            }
        }
    }
}