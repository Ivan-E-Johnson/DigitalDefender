using Maps;
using UnityEngine;

namespace DigitalDefender
{
    public static class MapHelper
    {
        public static void RandomlyChooseAndSetStartAndEnd(MapGrid mapGrid,ref Vector3Int startPoint, ref Vector3Int endPoint, bool randomPlacement, 
            Direction startingEdge = Direction.Left, Direction endingEdge = Direction.Right)
        {
            if (randomPlacement)
            {
                startPoint = RandomlyChooseEdgePosition(mapGrid, startPoint);
                endPoint = RandomlyChooseEdgePosition(mapGrid, endPoint);
                
            }
            else
            {
                startPoint = RandomlyChooseEdgePosition(mapGrid, startPoint, startingEdge);
                endPoint = RandomlyChooseEdgePosition(mapGrid, endPoint, endingEdge);
            }
            mapGrid.SetCell(startPoint.x, startPoint.z, MapCellObjectType.Start);
            mapGrid.SetCell(endPoint.x, endPoint.z, MapCellObjectType.End);
            
        }

        private static Vector3Int RandomlyChooseEdgePosition(MapGrid mapGrid, Vector3Int startingPosition,
            Direction startingDirection = Direction.None)
        {
            if (startingDirection == Direction.None)
            {
                startingDirection = (Direction) Random.Range(1, 5);
            }

            Vector3Int edgePosition = Vector3Int.zero;
            switch (startingDirection)
            {
                case Direction.Up:
                    do
                    {
                        edgePosition = new Vector3Int(Random.Range(0, mapGrid.Width), 0, mapGrid.Length - 1);
                    } while (Vector3Int.Distance(edgePosition, startingPosition) <= 1);
                    break;
                case Direction.Down:
                    do
                    {
                        edgePosition = new Vector3Int(Random.Range(0, mapGrid.Width), 0, 0);
                    } while (Vector3Int.Distance(edgePosition, startingPosition) <= 1);
                    break;
                case Direction.Left:
                    do
                    {
                        edgePosition = new Vector3Int(0, 0, Random.Range(0, mapGrid.Length));
                    } while (Vector3Int.Distance(edgePosition, startingPosition) <= 1);
                    break;
                case Direction.Right:
                    do
                    {
                        edgePosition = new Vector3Int(mapGrid.Width - 1, 0, Random.Range(0, mapGrid.Length));
                    }while (Vector3Int.Distance(edgePosition, startingPosition) <= 1);

                    break;
            }
            return edgePosition;
        }
        

    }
}