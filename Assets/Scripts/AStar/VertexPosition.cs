using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class VertexPosition : IEquatable<VertexPosition>, IComparable<VertexPosition>
    {

        public static readonly Vector3Int[] possibleNeighbours = 
        {
            new Vector3Int(0, 0, 1),  // Up
            new Vector3Int(0, 0, -1), // Down
            new Vector3Int(-1, 0, 0), // Left
            new Vector3Int(1, 0, 0)   // Right
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
        
        public bool isTaken
        {
            get => _isTaken;
            set => _isTaken = value;
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