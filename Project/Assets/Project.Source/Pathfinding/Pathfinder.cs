using Project.Source.Utilities.Components;
using UnityEngine;

namespace Project.Source.Pathfinding
{
    public class Pathfinder : SingletonBehaviour<Pathfinder>
    {
        public PathfindingGrid grid;
        public Transform pointA;
        public Transform pointB;

        public bool hasPath;

        private readonly PathSolver solver = new PathSolver();
        private Path path;

        private void Update()
        {
            var nodeA = grid.WorldPositionToNode(pointA.position);
            var nodeB = grid.WorldPositionToNode(pointB.position);

            path = solver.FindPath(grid, nodeA, nodeB, Heuristics.Default);
            hasPath = path != null;
        }

        private void OnDrawGizmos()
        {
            var offset = Vector3.back * 0.1f;

            if (path != null)
            {
                for (var i = 0; i < path.Waypoints.Count - 1; i++)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(path.Waypoints[i], path.Waypoints[i + 1]);
                }
            }
        }
    }
}