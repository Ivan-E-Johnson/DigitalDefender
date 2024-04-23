namespace Maps
{
    public class MapCell
    {
        public MapCell(int x, int z)
        {
            X = x;
            Z = z;
            IsTaken = false;
            ObjectType = MapCellObjectType.Empty;
        }

        // Prefer to user Auto-Properties when possible
        public int X { get; }
        public int Z { get; }
        public bool IsTaken { get; set; }

        public MapCellObjectType ObjectType { get; set; }
    }

    public enum MapCellObjectType
    {
        Empty,
        Road,
        Obstical,
        Start,
        End
    }
}