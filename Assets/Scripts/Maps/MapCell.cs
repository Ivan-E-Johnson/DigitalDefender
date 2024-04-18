using UnityEngine;

namespace Maps
{
    public class MapCell : MapCenterPoint
    {
        public bool IsTaken { get; set; }
        public MapCellObjectType ObjectType { get; set; }

        // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/base
        public MapCell(int x, int z) :
            base(new Vector3Int(x, 0, z)) // Assuming Y is always 0; Can Change this if want to change map height
        {
            IsTaken = false;
            ObjectType = MapCellObjectType.Empty;
        }
    }

    public enum MapCellObjectType
    {
        Empty,
        Road,
        Obstacle,
        Start,
        End
    }
}