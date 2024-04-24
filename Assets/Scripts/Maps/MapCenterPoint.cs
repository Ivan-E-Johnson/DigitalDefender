using System;
using UnityEngine;

namespace Maps
{
    public class MapCenterPoint
    {
        private Vector3Int _position;
        public Vector3Int Position
        {
            get => _position;
            set => _position = value;
        }

        public int X => Position.x;
        public int Y => Position.y;
        public int Z => Position.z;

        public override string ToString()
        {
            return "Position" + Position;
        }

        public MapCenterPoint(Vector3Int position)
        {
            _position = position;
        }

        public MapCenterPoint(int x, int y, int z)
        {
            Position = new Vector3Int(x, y, z);
        }

        public MapCenterPoint()
        {
            Position = new Vector3Int(-1, -1, -1); // These should be invalid objects
        }
        
 

        public Vector3Int[] GetPossibleNeighbours()
        {
            return new Vector3Int[]
            {
                Position + Vector3Int.up,
                Position + Vector3Int.down,
                Position + Vector3Int.left,
                Position + Vector3Int.right
            };
        }

        public static float Distance(MapCenterPoint a, MapCenterPoint b)
        {
            if (a == null || b == null)
            {
                throw new ArgumentNullException(a == null ? nameof(a) : nameof(b));
            }
            return Vector3Int.Distance(a.Position, b.Position);
        }
    }
    
    
    
}