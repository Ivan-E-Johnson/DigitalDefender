using UnityEngine;
using System.Collections.Generic;

namespace DigitalDefender
{
    public class KnightPeice
    {
        // Possibly refacor this or create helper function??
        public static List<Vector3Int> PossibleMoves = new List<Vector3Int>
        {
            // index locations of possible moves of a Knight
            new Vector3Int(1, 0, 2),
            new Vector3Int(-1, 0,2),
            new Vector3Int(1, 0, -2),
            new Vector3Int(-1, 0, -2),
            new Vector3Int(2, 0, 1),
            new Vector3Int(-2, 0, 1),
            new Vector3Int(2, 0, -1),
            new Vector3Int(-2, 0, -1)
        };
        
        private Vector3Int _position;
        public Vector3Int Position { get => _position; set => _position = value; }
        
        public KnightPeice(Vector3Int position)
        {
            this.Position = position;
        }
        
        

    }
}