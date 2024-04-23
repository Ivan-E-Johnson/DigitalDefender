using UnityEngine;

namespace Maps
{
    public static class MapHelper
    {
        public static void RandomlyChooseAndSetStartAndEnd(MapGrid mapGrid, ref MapCenterPoint startPoint,
            ref MapCenterPoint endPoint, bool randomPlacement,
            EdgeDirection startingEdge = EdgeDirection.Up, EdgeDirection endingEdge = EdgeDirection.Down)
        {
            
            // If we want to Enforce a minimum distance between start and end here would be where we do that;
            if (randomPlacement)
            {
                startPoint.Position = RandomlyChooseEdgePosition(mapGrid, EdgeDirection.None);
                endPoint.Position = RandomlyChooseEdgePosition(mapGrid, EdgeDirection.None);
            }
            else
            {
                startPoint.Position = RandomlyChooseEdgePosition(mapGrid, startingEdge);
                endPoint.Position = RandomlyChooseEdgePosition(mapGrid, endingEdge);
            }

            mapGrid.SetCell(startPoint.X, startPoint.Z, MapCellObjectType.Start);
            mapGrid.SetCell(endPoint.X, endPoint.Z, MapCellObjectType.End);
        }

        private static Vector3Int RandomlyChooseEdgePosition(MapGrid mapGrid,EdgeDirection startingDirection = EdgeDirection.None)
        {
            if (startingDirection == EdgeDirection.None) startingDirection = (EdgeDirection)Random.Range(1, 5);
            
            var edgePosition = Vector3Int.zero;
            switch (startingDirection)
            {
                case EdgeDirection.Up:
                    edgePosition = new Vector3Int(Random.Range(0, mapGrid.Width), 0, mapGrid.Length - 1);
                    break;
                case EdgeDirection.Down:
                    edgePosition = new Vector3Int(Random.Range(0, mapGrid.Width), 0, 0);
                    break;
                case EdgeDirection.Left:
                    edgePosition = new Vector3Int(0, 0, Random.Range(0, mapGrid.Length));
                    break;
                case EdgeDirection.Right:
                    edgePosition = new Vector3Int(mapGrid.Width - 1, 0, Random.Range(0, mapGrid.Length));
                    break;
            }
            return edgePosition;
        }
    }
}