using System.Collections.Generic;
using UnityEngine;

namespace Project.Source.Pathfinding
{
    public class PathfindingNode
    {
        public Vector3 Position;
        public bool IsWalkable;
        
        public List<PathfindingNode> Neighbors = new List<PathfindingNode>();
    }
}