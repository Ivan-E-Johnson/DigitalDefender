using System.Collections.Generic;
using UnityEngine;

namespace Maps
{
    public class KnightPiece
    {
        // Possibly refacor this or create helper function??
        public static readonly List<Vector3Int> PossibleMoves = new List<Vector3Int>
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

        public Vector3Int Position { get; set; }

        public KnightPiece(Vector3Int position)
        {
            this.Position = position;
        }
        
        

    }
}