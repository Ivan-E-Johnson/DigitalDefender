using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Maps;
using UnityEngine;

namespace Maps
{
    public class MapGenerator : MonoBehaviour
    {
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
        [CanBeNull] private MapCenterPoint _startPositions;
        [CanBeNull] private MapCenterPoint _endPositions;

        private void Start()
        {
            GenerateNewMap();
        }

        public void GenerateNewMap()
        {
            
            mapGrid = new MapGrid(width, length);
            _startPositions = new MapCenterPoint();
            _endPositions = new MapCenterPoint();
            
            gridVisualizer.VisualizeGrid(width, length);
            mapVisualizer.ClearMap();        // TODO FIX THIS SOMETHING DOESN't GET CLEARED WHEN YOU GENERATE A NEW MAP
            // Cells for start and end are initialized in here 
            MapHelper.RandomlyChooseAndSetStartAndEnd(mapGrid, ref _startPositions, ref _endPositions,
                randomPlaceStartAndEnd,
                startEdgeDirection, endEdgeDirection);
            
            
            if (_startPositions == null || _endPositions == null)
            {
                // Debug.Log("Start or End positions are null");
                throw new System.Exception("Start or End positions are null");
            }

            Debug.Log("*******************");
            Debug.Log("Start Position: " + _startPositions);
            Debug.Log("End Position: " + _endPositions);
            Debug.Log("*******************");
            var candidateMap = new CandidateMap(mapGrid, numberOfPieces);
            candidateMap.CreateMap(_startPositions, _endPositions, autoRepair);
            mapVisualizer.InitializeMapCells(mapGrid,candidateMap.GetMapData()); // Rest of the 
            mapVisualizer.VisualizeMap(mapGrid, visualizeUsingPrefabs); 
            
        }
        
    }
}