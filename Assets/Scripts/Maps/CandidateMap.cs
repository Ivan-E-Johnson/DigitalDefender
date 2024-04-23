using System;
using System.Collections.Generic;
using System.Linq;
using Maps;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DigitalDefender
{
    public class CandidateMap
    {
        private readonly List<KnightPiece> _knightPiecesList;
        private List<Vector3Int> _pathList;
        private Vector3Int _startPoint, _endPoint;
        public MapGrid mapGrid;


        public CandidateMap(MapGrid mapGrid, int numberOfPieces)
        {
            this.mapGrid = mapGrid;
            NumberOfPeices = numberOfPieces;
            _knightPiecesList = new List<KnightPiece>();
        }


        public MapGrid MapGrid => mapGrid;
        public int NumberOfPeices { get; }

        public bool[] BoolObsticalesArray { get; private set; }

        public void CreateMap(Vector3Int startPoint, Vector3Int endPoint, bool autoRepair)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
            BoolObsticalesArray = new bool[mapGrid.Width * mapGrid.Length];
            RandomlyPlaceKnightPieces(NumberOfPeices);
            _FillObsticalArrayFromKnightLocations();
            //_FindPath();
            if (autoRepair && _pathList.Count == 0)
            {
                Debug.Log("No Path Found, Attempting to repair");
                _RepairMap();
            }

            foreach (var path in _pathList)
            {
                // Debug.Log($"Path: {path}");
                // Hacky way to visualize the path
                // _CreateIndicator(new Vector3Int(path.x, 0, path.z), Color.magenta, 
                //     PrimitiveType.Cylinder);
            }
        }

        private void _RepairMap()
        {
            // This method attempts to remove the minimum number of obstacles to ensure a path exists from start to end.

            var number_of_obsticals_to_remove = 2;
            var number_of_current_obsticals = BoolObsticalesArray.Count(x => x);
            var list_of_indexes_with_obsticals = BoolObsticalesArray
                .Select((obstacle, index) => new { Obstacle = obstacle, Index = index })
                .Where(x => x.Obstacle)
                .Select(x => x.Index)
                .ToList();

            // Attempt to find a path until no obstacles are left to check or a valid path is found
            while (number_of_current_obsticals > 0 && _pathList.Count == 0)
            {
                var random_obstical_indexes = new List<int>();
                for (var i = 0; i < number_of_obsticals_to_remove && list_of_indexes_with_obsticals.Count > 0; i++)
                {
                    var random_index = Random.Range(0, list_of_indexes_with_obsticals.Count);
                    var index_to_remove = list_of_indexes_with_obsticals[random_index];
                    list_of_indexes_with_obsticals
                        .RemoveAt(random_index); // Remove index from consideration for future iterations
                    BoolObsticalesArray[index_to_remove] = false; // Remove the obstacle at the randomly selected index
                    random_obstical_indexes.Add(index_to_remove);
                }

                // Recalculate the path and see if it is valid
                _pathList = AStar.AStar.GetPath(_startPoint, _endPoint, BoolObsticalesArray, mapGrid);
                if (_pathList != null && _pathList.Count > 0)
                {
                    Debug.Log($"Path found with {number_of_current_obsticals} obstacles still in place.");
                    break; // A valid path is found, exit the loop
                }

                // If no path is found, reset the obstacles removed and try again if conditions allow
                foreach (var index in random_obstical_indexes) BoolObsticalesArray[index] = true;
                number_of_current_obsticals = BoolObsticalesArray.Count(x => x);
            }

            // If a valid path is still not found, additional logic may be needed to handle the scenario.
            if (_pathList == null || _pathList.Count == 0)
                Console.WriteLine("Unable to clear a path with minimal obstacle removal.");
            // Consider other strategies or notify the user/system
        }


        private void _FindPath()
        {
            if (_startPoint == null || _endPoint == null || BoolObsticalesArray == null || mapGrid == null)
                throw new ArgumentNullException("One or more arguments are null");

            _pathList = AStar.AStar.GetPath(_startPoint, _endPoint, BoolObsticalesArray, MapGrid);

            Debug.Log($"Path Length: {_pathList.Count}");

            foreach (var positionVector3Int in _pathList)
            {
                // Debug.Log($"Postion :{positionVector3Int}");
            }
        }

        // private static void _CreateIndicator(Vector3Int position, Color color, PrimitiveType primitiveType)
        // {
        //     var element = GameObject.CreatePrimitive(primitiveType);
        //     element.transform.position = position + new Vector3(0.5f, 0.5f, 0.5f);
        //     element.GetComponent<Renderer>().material.color = color;
        // }

        private bool CheckIfPositionCanBeObstical(Vector3Int position)
        {
            if (position == _startPoint || position == _endPoint) return false;
            var index = mapGrid.CalculateIndexFromCoordinates(position.x, position.z);
            if (BoolObsticalesArray[index]) return false;
            return true;
        }


        private void RandomlyPlaceKnightPieces(int numberOfPeicesToPlace)
        {
            var knightPlacementTryMax = 100;
            var piecesLeftToPlace = numberOfPeicesToPlace;
            // Debug.Log($"Coords of StartPoint {startPoint}");
            // Debug.Log($"Coords of endPoint {endPoint}");
            while (piecesLeftToPlace > 0 && knightPlacementTryMax > 0)
            {
                var randomIndex = Random.Range(0, BoolObsticalesArray.Length);
                if (BoolObsticalesArray[randomIndex] == false)
                {
                    // Debug.Log($"$Attempting to place object {numberOfPeicesToPlace-PeicesLeftToPlace} at index {randomIndex} ");
                    var coords = mapGrid.CalculateCoordinatesFromIndex(randomIndex);
                    // Debug.Log($"Checking Coords: {coords}");
                    if (coords == _startPoint || coords == _endPoint) continue;
                    // Check if a knight piece already exists at this index
                    if (_knightPiecesList.Any(kp => kp.Position == coords)) continue;
                    BoolObsticalesArray[randomIndex] = true;
                    _knightPiecesList.Add(new KnightPiece(coords));
                    piecesLeftToPlace--;
                }

                knightPlacementTryMax--;
            }
        }

        private void _FillObsticalArrayFromKnightLocations()
        {
            foreach (var knight in _knightPiecesList)
            foreach (var relativeIndex in KnightPiece.PossibleMoves)
            {
                var possiblePosition = knight.Position + relativeIndex;
                // Debug.Log($"Possible Position for obstical {possiblePosition}");
                if (mapGrid.IsPositionValid(possiblePosition))
                    if (CheckIfPositionCanBeObstical(possiblePosition))
                        BoolObsticalesArray[
                                mapGrid.CalculateIndexFromCoordinates(possiblePosition.x, possiblePosition.z)] =
                            true;
            }
        }

        public MapData GetMapData()
        {
            return new MapData
            {
                ObsticalesArray = BoolObsticalesArray,
                KnightPeicesList = _knightPiecesList,
                StartPoint = _startPoint,
                EndPoint = _endPoint,
                Path = _pathList
            };
        }
    }
}