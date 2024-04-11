using System.Collections.Generic;
using UnityEngine;
namespace DigitalDefender
{
    public struct MapData
    {
        public bool[] ObsticalesArray;
        public List<KnightPeice> KnightPeicesList;
        public Vector3 StartPoint;
        public Vector3 EndPoint;

    }
}