using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

namespace DigitalDefender
{
    public class CandidateMap
    {
        public MapGrid mapGrid;
        private int numberOfPeices = 0;
        private bool[] obsticalesArray = null;
        private Vector3 startPoint, endPoint;
        private List<KnightPeice> KnightPeicesList;
        
        public MapGrid MapGrid { get => mapGrid; }
        public int NumberOfPeices { get => numberOfPeices; }
        public bool[] ObsticalesArray { get => obsticalesArray; }
        
        
        public CandidateMap(MapGrid mapGrid, int numberOfPieces)
        {
            this.mapGrid = mapGrid;
            this.numberOfPeices = numberOfPieces;
            this.KnightPeicesList = new List<KnightPeice>();
        }
        public void CreateMap(Vector3 startPoint, Vector3 endPoint, bool autoRepair = false)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.obsticalesArray = new bool[mapGrid.Width * mapGrid.Length];
            RandomlyPlaceKnightPieces(numberOfPeices);
            _FillObsticalArrayFromKnightLocations();
        }
        
        private bool CheckIfPositionCanBeObstical(Vector3 position)
        {
            if (position == startPoint || position == endPoint)
            {
                return false;
            }
            int index = mapGrid.CalculateIndexFromCoordinates(position.x, position.z);
            if (obsticalesArray[index])
            {
                return false;
            }
            return true;
        }
        
        
        private void RandomlyPlaceKnightPieces(int numberOfPeicesToPlace)
        {
            var KnightPlacementTryMax = 100;
            var PeicesLeftToPlace = numberOfPeicesToPlace;
            Debug.Log($"Coords of StartPoint {startPoint}");
            Debug.Log($"Coords of endPoint {endPoint}");
            while(PeicesLeftToPlace > 0 && KnightPlacementTryMax > 0)
            {
                var randomIndex = Random.Range(0, obsticalesArray.Length);
                if (obsticalesArray[randomIndex] == false)
                {   
                    Debug.Log($"$Attempting to place object {numberOfPeicesToPlace-PeicesLeftToPlace} at index {randomIndex} ");
                    var coords = mapGrid.CalculateCoordinatesFromIndex(randomIndex);
                    Debug.Log($"Checking Coords: {coords}");
                    if(coords == startPoint || coords == endPoint)
                        
                    {
                        continue;
                    }
                    // Check if a knight piece already exists at this index
                    if (KnightPeicesList.Any(kp => kp.Position == coords))
                    {
                        continue;
                    }
                    obsticalesArray[randomIndex] = true;
                    KnightPeicesList.Add(new KnightPeice(coords));
                    PeicesLeftToPlace--;
                    
                }
                KnightPlacementTryMax--;
            }
        }

        private void _FillObsticalArrayFromKnightLocations()
        {
            foreach (var knight in KnightPeicesList)
            {
                foreach (var relativeIndex in KnightPeice.PossibleMoves)
                {
                    Vector3 possiblePosition = knight.Position + relativeIndex;
                    Debug.Log($"Possible Position for obstical {possiblePosition}");
                    if (mapGrid.IsPositionValid(position:possiblePosition))
                    {
                        if (CheckIfPositionCanBeObstical(possiblePosition))
                        {
                            obsticalesArray[mapGrid.CalculateIndexFromCoordinates(possiblePosition.x, possiblePosition.z)] =
                                true;
                        }
                    }
                   
                }
            }
        }
        
        public MapData GetMapData()
        {
            return new MapData
            {
                ObsticalesArray = this.obsticalesArray,
                KnightPeicesList = KnightPeicesList,
                StartPoint = startPoint,
                EndPoint = endPoint
            };
        }
    }
}