using System;
using System.Collections.Generic;
using DigitalDefender;
using UnityEngine;

namespace AStar
{
    public static class AStar
    {
        public static List<Vector3Int> GetPath(Vector3Int startPosition, Vector3Int endPosition, bool[] obsticalsArray,
            MapGrid grid)
        {
            VertexPosition startVertexPosition = new VertexPosition(startPosition, false);
            VertexPosition endVertexPosition = new VertexPosition(endPosition, false);

            List<Vector3Int> path = new List<Vector3Int>();

            List<VertexPosition> openedList = new List<VertexPosition>();
            HashSet<VertexPosition> closedList = new HashSet<VertexPosition>();


            startVertexPosition.estimatedCost = ManhatanDistance(startVertexPosition, endVertexPosition);
            openedList.Add(startVertexPosition);

            VertexPosition currentVertex = null;

            while (openedList.Count > 0)
            {
                openedList.Sort();
                currentVertex = openedList[0];

                if (currentVertex.Equals(endVertexPosition))
                {
                    while (!currentVertex.Equals(startVertexPosition))
                    {
                        path.Add(currentVertex.Position);
                        currentVertex = currentVertex.PreviousVertexPosition;
                    }
                    path.Reverse();
                    break;
                }

                var arrayOfNeighbors = FindNeighborFor(currentVertex, grid, obsticalsArray);
                foreach (var neighbor in arrayOfNeighbors)
                {
                    if (neighbor == null || closedList.Contains(neighbor))
                    {
                        var totalCost = currentVertex.totalCost + 1;
                        var neighborEstimatedCost = ManhatanDistance(neighbor, endVertexPosition);
                        neighbor.totalCost = totalCost;
                        neighbor.PreviousVertexPosition = currentVertex;
                        neighbor.estimatedCost = totalCost + neighborEstimatedCost;
                        if (openedList.Contains(neighbor) == false)
                        {
                            openedList.Add(neighbor);
                        }
                    }
                }

                closedList.Add(currentVertex);
                openedList.Remove(currentVertex);
            }
            
            
            return path;
        }

        private static VertexPosition[] FindNeighborFor(VertexPosition currentVertex,  MapGrid grid, bool[] obsticalsArray)
        {
            VertexPosition[] arrayOfNeighbors = new VertexPosition[4]; // Set size for left, right, up, down

            int arrayIndex = 0;
            foreach (var possibleNeighbor in VertexPosition.possibleNeighbours)
            {
                Vector3Int position = new Vector3Int(currentVertex.X + possibleNeighbor.x, 0,
                    currentVertex.Z + possibleNeighbor.z);
                arrayOfNeighbors[arrayIndex] = new VertexPosition(position,obsticalsArray[grid.CalculateIndexFromVector3Int(position)]);
                arrayIndex++;
            }

            return arrayOfNeighbors;
        }

        private static float ManhatanDistance(VertexPosition startVertexPosition, VertexPosition endVertexPostion)
        {
            return Mathf.Abs(startVertexPosition.X - endVertexPostion.X) +
                   Mathf.Abs(startVertexPosition.Z - endVertexPostion.Z);
        }
    }
}
