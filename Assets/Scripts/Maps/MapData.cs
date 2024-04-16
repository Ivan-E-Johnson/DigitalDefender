using System.Collections.Generic;
using Maps;
using UnityEngine;
namespace DigitalDefender
{
    public struct MapData
    {
        public bool[] ObsticalesArray;
        public List<KnightPiece> KnightPeicesList;
        public Vector3Int StartPoint;
        public Vector3Int EndPoint;

    }
}