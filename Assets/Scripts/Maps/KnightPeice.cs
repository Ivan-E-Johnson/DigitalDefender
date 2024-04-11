using UnityEngine;
using System.Collections.Generic;

namespace DigitalDefender
{
    public class KnightPeice
    {
        // Possibly refacor this or create helper function??
        public static List<Vector3> PossibleMoves = new List<Vector3>
        {
            // index locations of possible moves of a Knight
            new Vector3(1, 0, 2),
            new Vector3(-1, 0,2),
            new Vector3(1, 0, -2),
            new Vector3(-1, 0, -2),
            new Vector3(2, 0, 1),
            new Vector3(-2, 0, 1),
            new Vector3(2, 0, -1),
            new Vector3(-2, 0, -1)
        };
        
        private Vector3 position;
        public Vector3 Position { get => position; set => position = value; }
        
        public KnightPeice(Vector3 position)
        {
            this.Position = position;
        }
        
        

    }
}