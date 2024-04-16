namespace Maps
{
    public class MapCell
    {
        // Prefer to user Auto-Properties when possible
        public int X { get; }
        public int Z { get; }
        public bool IsTaken { get; set; }

        public MapCellObjectType ObjectType { get; set; }

        public MapCell(int x, int z)
        {
            this.X = x;
            this.Z = z;
            IsTaken = false;
            ObjectType = MapCellObjectType.Empty;

        }


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
