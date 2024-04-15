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

            
            // Debug.Log($"Start Position: {startPosition}, End Position: {endPosition}");
            // Debug.Log($"Obstacles Array Null: {obsticalsArray == null}, Grid Null: {grid == null}");

            startVertexPosition.estimatedCost = ManhatanDistance(startVertexPosition, endVertexPosition);
            openedList.Add(startVertexPosition);

            VertexPosition currentVertex = null;

            while (openedList.Count > 0)
            {
                openedList.Sort();
                currentVertex = openedList[0];

                if (currentVertex.Equals(endVertexPosition))
                {
                    // Debug.Log("Path Found");
                    while (!currentVertex.Equals(startVertexPosition))
                    {
                        path.Add(currentVertex.Position);
                        currentVertex = currentVertex.PreviousVertexPosition;
                    }
                    path.Reverse();
                    break;
                }
                
                var arrayOfNeighbors = _FindNeighborFor(currentVertex, grid, obsticalsArray);
                foreach (var neighbor in arrayOfNeighbors)
                {
                    
                    if (neighbor == null || closedList.Contains(neighbor))
                    {
                       continue;
                    }
                    // Debug.Log($"Neighbor: {neighbor.Position} isTaken: {neighbor.isTaken}");
                    
                    if (neighbor.isTaken == false)
                    {
                        var totalCost = currentVertex.totalCost + 1;
                        var neighborEstimatedCost = ManhatanDistance(neighbor, endVertexPosition);
                        neighbor.totalCost = totalCost;
                        neighbor.PreviousVertexPosition = currentVertex;
                        neighbor.estimatedCost = totalCost + neighborEstimatedCost;
                        
                        // Debug.Log("openedList Count: " + openedList.Count);
                        // Debug.Log("closedList Count: " + closedList.Count);
                        // Debug.Log("openedList.Contains(neighbor) == false: " + (openedList.Contains(neighbor) == false));
                        // Debug.Log($"Neighbor: {neighbor.Position} Total Cost: {neighbor.totalCost} Estimated Cost: {neighbor.estimatedCost}");
                        
                        // Debug.Log($"Neighbor: {neighbor.Position} Total Cost: {neighbor.totalCost} Estimated Cost: {neighbor.estimatedCost}");
                        if (openedList.Contains(neighbor) == false)
                        {
                            
                            Debug.Log($"Adding Neighbor: {neighbor.Position} Total Cost: {neighbor.totalCost} Estimated Cost: {neighbor.estimatedCost}");
                            openedList.Add(neighbor);
                            Debug.Log("openedList Count: " + openedList.Count);

                        }    
                    }

                    
                }
                // Debug.Log("*******************");
                // Debug.Log($"Current Vertex: {currentVertex.Position} has been checked" );
                // Debug.Log($"Current Vertex: {currentVertex.Position} has been added to closed list" );
                // Debug.Log($"Current Vertex: {currentVertex.Position} has been removed from opened list" );
                // Debug.Log("openedList Count: " + openedList.Count);
                // Debug.Log("closedList Count: " + closedList.Count);

                // Debug.Log(D);
                closedList.Add(currentVertex);
                openedList.Remove(currentVertex);
            }
            
            
            return path;
        }

        private static VertexPosition[] _FindNeighborFor(VertexPosition currentVertex,  MapGrid grid, bool[] obsticalsArray)
        {
            VertexPosition[] arrayOfNeighbors = new VertexPosition[4]; // Set size for left, right, up, down

            int arrayIndex = 0;
            foreach (var possibleNeighbor in VertexPosition.possibleNeighbours)
            {
                Vector3Int positionVector3Int = new Vector3Int(currentVertex.X + possibleNeighbor.x, 0,
                    currentVertex.Z + possibleNeighbor.z);
                // Debug.Log($"Possible Neighbor: {positionVector3Int}");
                if (grid.IsCellValid(positionVector3Int.x, positionVector3Int.z))
                {
                    // Debug.Log($"Valid Neighbor: {positionVector3Int}");
                    arrayOfNeighbors[arrayIndex] = new VertexPosition(positionVector3Int,obsticalsArray[grid.CalculateIndexFromVector3Int(positionVector3Int)]);
                    arrayIndex++;   
                }
                
               
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
