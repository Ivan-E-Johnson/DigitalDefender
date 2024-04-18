using System.Collections.Generic;
using Maps;
using UnityEngine;

namespace Maps
{
    public struct MapData
    {
        public bool[] ObsticalesArray;
        public List<KnightPiece> KnightPeicesList;
        public MapCenterPoint StartPoint;
        public MapCenterPoint EndPoint;
        public List<Vector3Int> Path;

        public override string ToString()
        {
            return $"{nameof(StartPoint)}: {StartPoint}, {nameof(EndPoint)}: {EndPoint}";
        }
        
    }
}