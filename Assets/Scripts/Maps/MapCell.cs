using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalDefender
{
    public class MapCell
    {
        private int x, z;
        private bool isTaken;
        private MapCellObjectType objectType;

        public int X { get => x; }
        public int Z { get => z; }
        public bool IsTaken { get => isTaken;  set => isTaken = value; }
        public MapCellObjectType ObjectType { get => objectType; set => objectType = value; }

        public MapCell(int x, int z)
        {
            this.x = x;
            this.z = z;
            isTaken = false;
            objectType = MapCellObjectType.Empty;

        }


    }

    public enum MapCellObjectType
    {
        Empty,
        Eoad, // What is this?
        Obstical,
        Start,
        End

    }
}
