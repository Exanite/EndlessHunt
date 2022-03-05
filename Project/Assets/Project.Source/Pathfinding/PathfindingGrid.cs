using UnityEngine;

namespace Project.Source.Pathfinding
{
    public class PathfindingGrid : MonoBehaviour
    {
        public LayerMask NonWalkableMask;

        public Vector2Int Size;
        public Vector2 NodeSize = Vector2.one;

        private PathfindingNode[] Nodes;

        private void OnEnable()
        {
            Nodes = new PathfindingNode[Size.x * Size.y];

            for (var y = 0; y < Size.y; y++)
            {
                for (var x = 0; x < Size.x; x++)
                {
                    var position = GetPositionOffset();
                    position += Vector3.right * NodeSize.x * x;
                    position += Vector3.up * NodeSize.y * y;

                    var isWalkable = !Physics2D.OverlapBox(position, NodeSize, 0, NonWalkableMask);
                    
                    var current = new PathfindingNode
                    {
                        Index = ToIndex(x, y),
                        
                        Position = position,
                        IsWalkable = isWalkable,
                    };

                    Nodes[ToIndex(x, y)] = current;

                    // -1, 0
                    if (x - 1 >= 0)
                    {
                        ConnectNodes(Nodes[ToIndex(x - 1, y)], current);
                    }

                    // 0, -1
                    if (y - 1 >= 0)
                    {
                        ConnectNodes(Nodes[ToIndex(x, y - 1)], current);
                    }

                    // -1, -1
                    if (x - 1 >= 0 && y - 1 >= 0)
                    {
                        ConnectNodes(Nodes[ToIndex(x - 1, y - 1)], current);
                    }

                    // +1, -1
                    if (x + 1 < Size.x && y - 1 >= 0)
                    {
                        ConnectNodes(Nodes[ToIndex(x + 1, y - 1)], current);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (Nodes == null)
            {
                return;
            }

            foreach (var node in Nodes)
            {
                if (!node.IsWalkable)
                {
                    continue;
                }

                Gizmos.color = Color.green;
                Gizmos.DrawCube(node.Position, new Vector3(NodeSize.x * 0.75f, NodeSize.y * 0.75f, 0.1f));

                foreach (var neighbor in node.Neighbors)
                {
                    if (!neighbor.IsWalkable)
                    {
                        continue;
                    }

                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(node.Position, neighbor.Position);
                }
            }
        }
        
        public int ToIndex(int x, int y)
        {
            return Size.x * y + x;
        }
        
        public int ToIndex(Vector2Int position)
        {
            return Size.x * position.y + position.x;
        }

        public Vector2Int ToPosition(int index)
        {
            return new Vector2Int(index / Size.x, index % Size.x);
        }

        private void ConnectNodes(PathfindingNode a, PathfindingNode b)
        {
            a.Neighbors.Add(b);
            b.Neighbors.Add(a);
        }

        private Vector3 GetPositionOffset()
        {
            var offset = (Vector2)Size / 2f;
            offset.Scale(NodeSize);
            offset += -NodeSize / 2f;

            return transform.position - (Vector3)offset;
        }
    }
}