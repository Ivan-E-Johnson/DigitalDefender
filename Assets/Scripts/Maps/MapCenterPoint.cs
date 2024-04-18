using UnityEngine;

namespace Maps
{
    public class MapCenterPoint
    {
        private Vector3Int Position { get; set; }

        public int X => Position.x;
        public int Y => Position.y;
        public int Z => Position.z;

        public MapCenterPoint(Vector3Int position)
        {
            Position = position;
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
    }
}