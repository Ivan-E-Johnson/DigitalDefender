using System.Collections.Generic;
using UnityEngine;

namespace Maps
{
    public class KnightPiece
    {
        // Possibly refacor this or create helper function??
        public static readonly List<Vector3Int> PossibleMoves = new()
        {
            // index locations of possible moves of a Knight
            new(1, 0, 2),
            new(-1, 0, 2),
            new(1, 0, -2),
            new(-1, 0, -2),
            new(2, 0, 1),
            new(-2, 0, 1),
            new(2, 0, -1),
            new(-2, 0, -1)
        };

        public KnightPiece(Vector3Int position)
        {
            Position = position;
        }

        public Vector3Int Position { get; set; }
    }
}