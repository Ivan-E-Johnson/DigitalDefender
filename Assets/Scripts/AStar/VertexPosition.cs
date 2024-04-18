using System;
using UnityEngine;

namespace AStar
{
    // Possible TODO: Make a base class for generic "MapVertexLocation" that can be used as a replacement for VertexPosition
    public class VertexPosition : IEquatable<VertexPosition>, IComparable<VertexPosition>
    {

        public static readonly Vector3Int[] PossibleNeighbours = 
        {
            new Vector3Int(0, 0, 1),  // Up
            new Vector3Int(0, 0, -1), // Down
            new Vector3Int(-1, 0, 0), // Left
            new Vector3Int(1, 0, 0)   // Right
        };

        // totalCost is distance from Start point to this point
        // estimated Cost = Shortest Path between this point and the End point
        public float TotalCost, EstimatedCost;
        public VertexPosition PreviousVertexPosition = null;
        private readonly Vector3Int _position;
        private bool _isTaken;
        public int X => _position.x;

        public int Y => _position.y;
        
        public int Z => _position.z;
        
        public Vector3Int Position => _position;

        public bool IsTaken
        {
            get => _isTaken;
            set => _isTaken = value;
        }

        public VertexPosition(Vector3Int position, bool isTaken)
        {
            _position = Vector3Int.RoundToInt(position);
            _isTaken = isTaken;
            EstimatedCost = 0;
            TotalCost = 1;
        }
        

        public int GetHashCode(VertexPosition obj)
        {
            return obj.GetHashCode();
        }
        public override int GetHashCode()
        {
            return _position.GetHashCode();
        }
        
        public bool Equals(VertexPosition other)
        {
            return other != null && Position == other.Position;

        }

        public int CompareTo(VertexPosition other)
        {
            if (this.EstimatedCost < other.EstimatedCost) // Put other object before other in list
            {
                return -1;
            }

            if (this.EstimatedCost > other.EstimatedCost) // Put this object after other object
            {
                return 1;
            }

            return 0; // Equals
        }
        
    }
}