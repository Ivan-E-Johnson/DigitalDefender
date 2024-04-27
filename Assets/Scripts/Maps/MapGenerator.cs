using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Maps;
using UnityEngine;

namespace Maps
{
    public class MapGenerator : MonoBehaviour
    {
        [CanBeNull]
        public MapCenterPoint StartPosition => _startPosition;

        [CanBeNull]
        public MapCenterPoint EndPosition => _endPosition;
        public MapGrid MapGrid => mapGrid;
        private static int instatiationCount = 0;
        public static MapGenerator Instance { get; private set; }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        [CanBeNull] public GridVisualizer gridVisualizer;
        [CanBeNull] public MapVisualizer mapVisualizer;
        public bool randomPlaceStartAndEnd;
        public EdgeDirection startEdgeDirection;
        public EdgeDirection endEdgeDirection;
        public bool visualizeUsingPrefabs = false; // Change this in the future
        public bool autoRepair = true;

        [Range(1, 10)] public int numberOfPieces = 5;

        [Range(5, 20)] // This will create a slider in the editor that will allow you to set the width of the grid
        public int width, length = 11;

        
        private MapGrid mapGrid;
        [CanBeNull] private MapCenterPoint _startPosition;
        [CanBeNull] private MapCenterPoint _endPosition;
        
        public void GenerateNewMap()
        {
            instatiationCount++;
            
            mapGrid = new MapGrid(width, length);
            _startPosition = new MapCenterPoint();
            _endPosition = new MapCenterPoint();

            gridVisualizer.RemoveGrid();
            gridVisualizer.VisualizeGrid(width, length);
            mapVisualizer.ClearMap();        // TODO FIX THIS SOMETHING DOESN't GET CLEARED WHEN YOU GENERATE A NEW MAP
            // Cells for start and end are initialized in here 
            MapHelper.RandomlyChooseAndSetStartAndEnd(mapGrid, ref _startPosition, ref _endPosition,
                randomPlaceStartAndEnd,
                startEdgeDirection, endEdgeDirection);
            
            
            if (_startPosition == null || _endPosition == null)
            {
                // Debug.Log("Start or End positions are null");
                throw new System.Exception("Start or End positions are null");
            }

            Debug.Log("*******************");
            Debug.Log("Start Position: " + _startPosition);
            Debug.Log("End Position: " + _endPosition);
            Debug.Log("*******************");
            var candidateMap = new CandidateMap(mapGrid, numberOfPieces);
            candidateMap.CreateMap(_startPosition, _endPosition, autoRepair);
            mapVisualizer.InitializeMapCells(mapGrid,candidateMap.GetMapData()); // Rest of the 
            mapVisualizer.VisualizeMap(mapGrid, visualizeUsingPrefabs); 
            
        }

        public Vector3[] GetNodePositions()
        {
            var nodePositions = new Vector3[mapGrid.waypoints.Count + 2]; // Add 2 for start and end
            nodePositions[0] = _startPosition.Position;
            for (var i = 0; i < mapGrid.waypoints.Count; i++)
            {
                nodePositions[i + 1] = mapGrid.waypoints[i];
            }
            nodePositions[nodePositions.Length - 1] = _endPosition.Position;
            return nodePositions;
        }
        
        
    }
}