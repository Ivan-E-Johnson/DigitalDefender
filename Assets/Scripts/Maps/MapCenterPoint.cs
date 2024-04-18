using UnityEngine;

namespace Maps
{
    public class MapCenterPoint
    {
        public static readonly Vector3Int[] PossibleNeighbours =
        { 
            Position + Vector3Int.up,
            Position + Vector3Int.down,
            Position + Vector3Int.left,
            Position + Vector3Int.right
        };

        public int X => Position.x;
        public int Y => Position.y;
        public int Z => Position.z;
        public static Vector3Int Position { get; private set; }
        
        public MapCenterPoint(Vector3Int position)
        {
            Position = position;
        }

    }
}