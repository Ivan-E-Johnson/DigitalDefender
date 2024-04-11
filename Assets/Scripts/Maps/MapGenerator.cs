using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalDefender
{
    public class MapGenerator : MonoBehaviour
    {
        // Start is called before the first frame update
        public GridVisualizer gridVisualizer;
        public MapVisualizer mapVisualizer;
        public bool randomPlaceStartAndEnd;
        public Direction startEdgeDirection;
        public Direction endEdgeDirection;
        [Range(1,10)]
        public int numberOfPieces = 5;

        [Range(5,20)] // This will create a slider in the editor that will allow you to set the width of the grid
        public int width, length = 11;

        private Vector3 startPositions, endPositions;
        private  MapGrid mapGrid;
        private void Start()
        {
            if (gridVisualizer == null)
            {
                gridVisualizer = new GridVisualizer();
            }
            MapGrid mapGrid = new MapGrid(width, length);
            gridVisualizer.VisulaizeGrid(width, length);
            MapHelper.RandomlyChooseAndSetStartAndEnd(mapGrid, ref startPositions, ref endPositions, randomPlaceStartAndEnd,
                startEdgeDirection, endEdgeDirection);
            
            Debug.Log("*******************");
            Debug.Log("Start Position: " + startPositions);
            Debug.Log("End Position: " + endPositions);
            Debug.Log("*******************");
            CandidateMap candidateMap = new CandidateMap(mapGrid, numberOfPieces);
            candidateMap.CreateMap(startPositions, endPositions);
            mapVisualizer.VisualizeMap(mapGrid, candidateMap.GetMapData(), false);
            
    
            //mapGrid.CheckCoordinates();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}