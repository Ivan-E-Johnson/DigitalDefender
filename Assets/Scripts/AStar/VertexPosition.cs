using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class VertexPosition : IEquatable<VertexPosition>, IComparable<VertexPosition>
    {

        public static List<Vector3Int> possibleNeighbours = new List<Vector3Int>()
        {
            Vector3Int.left,
            Vector3Int.right,
            Vector3Int.up,
            Vector3Int.down
        };

        // totalCost is distance from Startpoint to this point
        // estimated Cost = Shortest Path between this point and the End point
        public float totalCost, estimatedCost;
        public VertexPosition PreviousVertexPosition = null;
        private Vector3Int _position;
        private bool _isTaken;
        public int X
        {
            get => _position.x;
        }
        public int Z
        {
            get => _position.z;
        }
        
        public Vector3Int Position
        {
            get => _position;
        }

        public VertexPosition(Vector3Int position, bool isTaken)
        {
            _position = Vector3Int.RoundToInt(position);
            _isTaken = isTaken;
            estimatedCost = 0;
            totalCost = 1;
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
            return Position == other.Position;

        }

        public int CompareTo(VertexPosition other)
        {
            if (this.estimatedCost < other.estimatedCost) // Put other object before other in list
            {
                return -1;
            }

            if (this.estimatedCost > other.estimatedCost) // Put this object after other object
            {
                return 1;
            }

            return 0; // Equals
        }
        
    }
}