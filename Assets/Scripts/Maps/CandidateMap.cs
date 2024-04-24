using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using AStar;
using Maps;


namespace Maps
{
    public class CandidateMap
    {
        public MapGrid mapGrid;
        private int _numberOfPeices = 0;
        private bool[] _obsticalesArray = null;
        private MapCenterPoint _startPoint, _endPoint;

        public MapCenterPoint StartPoint
        {
            get => _startPoint;
            set => _startPoint = value;
        }

        public MapCenterPoint EndPoint
        {
            get => _endPoint;
            set => _endPoint = value;
        }

        private List<KnightPiece> _knightPiecesList;
        private List<Vector3Int> _pathList;
        
        

        public MapGrid MapGrid => mapGrid;
        public int NumberOfPeices => _numberOfPeices;
        public bool[] BoolObsticalesArray => _obsticalesArray;


        public CandidateMap(MapGrid mapGrid, int numberOfPieces)
        {
            this.mapGrid = mapGrid;
            _numberOfPeices = numberOfPieces;
            _knightPiecesList = new List<KnightPiece>();
            _pathList = new List<Vector3Int>();
            _obsticalesArray = new bool[mapGrid.Width * mapGrid.Length];
        }

        public void CreateMap(MapCenterPoint startPoint, MapCenterPoint endPoint, bool autoRepair)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            RandomlyPlaceKnightPieces(NumberOfPeices);
            _FillObsticalArrayFromKnightLocations();
            _FindPath();
            if (autoRepair && _pathList.Count == 0)
            {
                Debug.Log("No Path Found, Attempting to repair");
                //_RepairMap();
            }
        }
        
        

        private void _RepairMap()
        {
            // This method attempts to remove the minimum number of obstacles to ensure a path exists from start to end.

            var number_of_obsticals_to_remove = 2;
            var number_of_current_obsticals = _obsticalesArray.Count(x => x);
            var list_of_indexes_with_obsticals = _obsticalesArray
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
                    _obsticalesArray[index_to_remove] = false; // Remove the obstacle at the randomly selected index
                    random_obstical_indexes.Add(index_to_remove);
                }

                // Recalculate the path and see if it is valid
                _pathList = AStar.AStar.GetPath(StartPoint.Position, EndPoint.Position, _obsticalesArray, mapGrid);
                if (_pathList != null && _pathList.Count > 0)
                {
                    Debug.Log($"Path found with {number_of_current_obsticals} obstacles still in place.");
                    break; // A valid path is found, exit the loop
                }

                // If no path is found, reset the obstacles removed and try again if conditions allow
                foreach (var index in random_obstical_indexes) _obsticalesArray[index] = true;
                number_of_current_obsticals = _obsticalesArray.Count(x => x);
            }

            // If a valid path is still not found, additional logic may be needed to handle the scenario.
            if (_pathList == null || _pathList.Count == 0)
                Console.WriteLine("Unable to clear a path with minimal obstacle removal.");
            // Consider other strategies or notify the user/system
        }


        private void _FindPath()
        {
            if (StartPoint == null || EndPoint == null || BoolObsticalesArray == null || mapGrid == null)
                throw new ArgumentNullException("One or more arguments are null");

            _pathList = AStar.AStar.GetPath(StartPoint.Position, EndPoint.Position, BoolObsticalesArray, MapGrid);
        }

        private bool CheckIfPositionCanBeObstical(Vector3Int position)
        {
            if (position == StartPoint.Position || position == EndPoint.Position) return false;
            var index = mapGrid.CalculateIndexFromCoordinates(position.x, position.z);
            if (_obsticalesArray[index]) return false;
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
                if (_obsticalesArray[randomIndex] == false)
                {
                    // Debug.Log($"$Attempting to place object {numberOfPeicesToPlace-PeicesLeftToPlace} at index {randomIndex} ");
                    var coords = mapGrid.CalculateCoordinatesFromIndex(randomIndex);
                    // Debug.Log($"Checking Coords: {coords}");
                    if (coords == StartPoint.Position || coords == EndPoint.Position) continue;
                    // Check if a knight piece already exists at this index
                    if (_knightPiecesList.Any(kp => kp.Position == coords)) continue;
                    _obsticalesArray[randomIndex] = true;
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
                if (mapGrid.IsCellValid(possiblePosition))
                    if (CheckIfPositionCanBeObstical(possiblePosition))
                        _obsticalesArray
                                [mapGrid.CalculateIndexFromCoordinates(possiblePosition.x, possiblePosition.z)] =
                            true;
            }
        }

        public MapData GetMapData()
        {
            return new MapData
            {
                ObsticalesArray = _obsticalesArray,
                KnightPeicesList = _knightPiecesList,
                StartPoint = _startPoint,
                EndPoint = _endPoint,
                Path = _pathList
            };
        }
    }
}