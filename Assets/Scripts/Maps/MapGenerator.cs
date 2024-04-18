using System.Collections;
using System.Collections.Generic;
using Maps;
using UnityEngine;

namespace DigitalDefender
{
    public class MapGenerator : MonoBehaviour
    {
        // Start is called before the first frame update
        public GridVisualizer gridVisualizer;
        public MapVisualizer mapVisualizer;
        public bool randomPlaceStartAndEnd;
        public EdgeDirection startEdgeDirection;
        public EdgeDirection endEdgeDirection;
        public bool visualizeUsingPrefabs = false; // Change this in the future
        public bool autoRepair = true;

        [Range(1, 10)] public int numberOfPieces = 5;

        [Range(5, 20)] // This will create a slider in the editor that will allow you to set the width of the grid
        public int width, length = 11;

        private Vector3Int startPositions, endPositions;
        private MapGrid mapGrid;

        private void Start()
        {
            gridVisualizer.VisualizeGrid(width, length);
            GenerateNewMap();
        }

        public void GenerateNewMap()
        {
            mapGrid = new MapGrid(width, length);
            mapVisualizer.ClearMap();
            // This will initialize the start and end positions in the mapGrid
            // 
            MapHelper.RandomlyChooseAndSetStartAndEnd(mapGrid, ref startPositions, ref endPositions,
                randomPlaceStartAndEnd,
                startEdgeDirection, endEdgeDirection);

            if (startPositions == null || endPositions == null)
            {
                Debug.Log("Start or End positions are null");
                throw new System.Exception("Start or End positions are null");
            }

            Debug.Log("*******************");
            Debug.Log("Start Position: " + startPositions);
            Debug.Log("End Position: " + endPositions);
            Debug.Log("*******************");
            var candidateMap = new CandidateMap(mapGrid, numberOfPieces);
            candidateMap.CreateMap(startPositions, endPositions, autoRepair);
            mapVisualizer.VisualizeMap(mapGrid, candidateMap.GetMapData(), visualizeUsingPrefabs);
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}