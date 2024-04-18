using Maps;
using UnityEngine;

namespace Maps
{
    public static class MapHelper
    {
        public static void RandomlyChooseAndSetStartAndEnd(MapGrid mapGrid, ref MapCenterPoint startPoint,
            ref MapCenterPoint endPoint, bool randomPlacement,
            EdgeDirection startingEdge = EdgeDirection.Left, EdgeDirection endingEdge = EdgeDirection.Right)
        {
            if (randomPlacement)
            {
                startPoint = RandomlyChooseEdgePosition(mapGrid);
                endPoint = RandomlyChooseEdgePosition(mapGrid);
            }
            else
            {
                startPoint = RandomlyChooseEdgePosition(mapGrid);
                endPoint = RandomlyChooseEdgePosition(mapGrid);
            }

            mapGrid.SetCell(startPoint.X, startPoint.X, MapCellObjectType.Start);
            mapGrid.SetCell(endPoint.X, endPoint.X, MapCellObjectType.End);
        }

        private static MapCenterPoint RandomlyChooseEdgePosition(MapGrid mapGrid,
            EdgeDirection startingDirection = EdgeDirection.None)
        {
            if (startingDirection == EdgeDirection.None) startingDirection = (EdgeDirection)Random.Range(1, 5);
            MapCenterPoint newEdgePosition = null;
            
            
            var randX = Random.Range(0, mapGrid.Width);
            var randZ = Random.Range(0, mapGrid.Length);
            switch (startingDirection)
            {
                case EdgeDirection.Up:
                    newEdgePosition = new MapCenterPoint(randX, 0, randZ);
                    break;
                case EdgeDirection.Down:
                    newEdgePosition = new MapCenterPoint(randX, mapGrid.Length - 1, randZ);
                    break;
                case EdgeDirection.Left:
                    newEdgePosition = new MapCenterPoint(0, randZ, randX);
                    break;
                case EdgeDirection.Right:
                    newEdgePosition = new MapCenterPoint(mapGrid.Width - 1, randZ, randX);
                    break;
            }

            if (newEdgePosition == null || mapGrid.IsCellTaken(newEdgePosition.X, newEdgePosition.Z))
            {
                throw new System.Exception("Edge Position is null");
            }
            return newEdgePosition;
        }
    }
}