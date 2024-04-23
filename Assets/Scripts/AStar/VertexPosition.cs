using System;
using UnityEngine;

namespace AStar
{
    public class VertexPosition : IEquatable<VertexPosition>, IComparable<VertexPosition>
    {
        public static readonly Vector3Int[] PossibleNeighbours =
        {
            new(0, 0, 1), // Up
            new(0, 0, -1), // Down
            new(-1, 0, 0), // Left
            new(1, 0, 0) // Right
        };

        public VertexPosition PreviousVertexPosition = null;

        // totalCost is distance from Start point to this point
        // estimated Cost = Shortest Path between this point and the End point
        public float TotalCost, EstimatedCost;

        public VertexPosition(Vector3Int position, bool isTaken)
        {
            Position = Vector3Int.RoundToInt(position);
            IsTaken = isTaken;
            EstimatedCost = 0;
            TotalCost = 1;
        }

        public int X => Position.x;

        public int Y => Position.y;

        public int Z => Position.z;

        public Vector3Int Position { get; }

        public bool IsTaken { get; set; }

        public int CompareTo(VertexPosition other)
        {
            if (EstimatedCost < other.EstimatedCost) // Put other object before other in list
                return -1;

            if (EstimatedCost > other.EstimatedCost) // Put this object after other object
                return 1;

            return 0; // Equals
        }

        public bool Equals(VertexPosition other)
        {
            return other != null && Position == other.Position;
        }


        public int GetHashCode(VertexPosition obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}