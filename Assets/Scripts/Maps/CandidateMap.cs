using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using AStar;


namespace DigitalDefender
{
    public class CandidateMap
    {
        public MapGrid mapGrid;
        private int numberOfPeices = 0;
        private bool[] obsticalesArray = null;
        private Vector3Int startPoint, endPoint;
        private List<KnightPeice> KnightPeicesList;
        private List<Vector3Int> _pathList;
        
        
        public MapGrid MapGrid { get => mapGrid; }
        public int NumberOfPeices { get => numberOfPeices; }
        public bool[] ObsticalesArray { get => this.obsticalesArray; }
        
        
        public CandidateMap(MapGrid mapGrid, int numberOfPieces)
        {
            this.mapGrid = mapGrid;
            this.numberOfPeices = numberOfPieces;
            this.KnightPeicesList = new List<KnightPeice>();
        }
        public void CreateMap(Vector3Int startPoint, Vector3Int endPoint, bool autoRepair = false)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.obsticalesArray = new bool[mapGrid.Width * mapGrid.Length];
            RandomlyPlaceKnightPieces(numberOfPeices);
            _FillObsticalArrayFromKnightLocations();
            _FindPath();
            if (autoRepair && _pathList.Count == 0)
            {
                    Debug.Log("No Path Found, Attempting to repair");
                    //CreateMap(startPoint, endPoint, autoRepair);
            }
            foreach( var path in _pathList)
            {
                Debug.Log($"Path: {path}");
                _CreateIndicator(new Vector3Int(path.x, 0, path.z), Color.magenta, 
                    PrimitiveType.Cylinder);
            }
            
            
        }

        private void _FindPath()
        {
            
            if (startPoint == null || endPoint == null || ObsticalesArray == null || mapGrid == null)
            {
                throw new ArgumentNullException("One or more arguments are null");
            }
            
            this._pathList = AStar.AStar.GetPath( startPoint, endPoint,ObsticalesArray, MapGrid);
            
            Debug.Log($"Path Length: {_pathList.Count}");
            
            foreach (var positionVector3Int in _pathList)
            {
                Debug.Log($"Postion :{positionVector3Int}");
                
            }
        }

        private static void _CreateIndicator(Vector3Int position, Color color, PrimitiveType primitiveType)
        {
            var element = GameObject.CreatePrimitive(primitiveType);
            element.transform.position = position + new Vector3(0.5f, 0.5f, 0.5f);
            element.GetComponent<Renderer>().material.color = color;
        }

        private bool CheckIfPositionCanBeObstical(Vector3Int position)
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
            // Debug.Log($"Coords of StartPoint {startPoint}");
            // Debug.Log($"Coords of endPoint {endPoint}");
            while(PeicesLeftToPlace > 0 && KnightPlacementTryMax > 0)
            {
                var randomIndex = Random.Range(0, ObsticalesArray.Length);
                if (obsticalesArray[randomIndex] == false)
                {   
                    // Debug.Log($"$Attempting to place object {numberOfPeicesToPlace-PeicesLeftToPlace} at index {randomIndex} ");
                    var coords = mapGrid.CalculateCoordinatesFromIndex(randomIndex);
                    // Debug.Log($"Checking Coords: {coords}");
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
                    Vector3Int possiblePosition = knight.Position + relativeIndex;
                    // Debug.Log($"Possible Position for obstical {possiblePosition}");
                    if (mapGrid.IsPositionValid(position:possiblePosition))
                    {
                        if (CheckIfPositionCanBeObstical(possiblePosition))
                        {
                            this.obsticalesArray[mapGrid.CalculateIndexFromCoordinates(possiblePosition.x, possiblePosition.z)] =
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