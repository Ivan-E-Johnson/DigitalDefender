using System.Collections.Generic;
using DigitalDefender;
using UnityEngine;
using UnityEngine.Serialization;

namespace Maps
{
    public class MapVisualizer : MonoBehaviour
    {
        private Transform _parent;
        public Color startColor = Color.green;
        public Color endColor = Color.red;
        [FormerlySerializedAs("obsticalColor")] public Color obstacleColor = Color.black;
        public Color knightColor = Color.yellow;

        private Dictionary<Vector3Int, GameObject> _dictionaryOfObsticals = new Dictionary<Vector3Int, GameObject>();
        private void Awake()
        {
            _parent = this.transform;
        }
        
        public void VisualizeMap(MapGrid mapGrid, MapData mapData, bool visualizeUsingPrefabs)
        {
            if (visualizeUsingPrefabs)
            {
              
            }
            else
            {
                VisualizeUsingPrimitives(mapGrid, mapData);
            }

        }
        
        private void VisualizeUsingPrimitives(MapGrid mapGrid, MapData mapData)
        {
            PlaceStartAndEndPoints(mapData);
            for (int i =0; i < mapData.ObsticalesArray.Length; i++)
            {
                if (mapData.ObsticalesArray[i])
                {
                    var coordinates = mapGrid.CalculateCoordinatesFromIndex(i);
                    if (coordinates != mapData.StartPoint && coordinates != mapData.EndPoint && !_dictionaryOfObsticals.ContainsKey(coordinates))
                    {
                        if(PlaceKnightPeice(mapData, coordinates))
                        {
                            CreateIndicator(new Vector3Int(coordinates.x, 0, coordinates.z), knightColor,
                                PrimitiveType.Cylinder);
                        }
                        else
                        {
                            CreateIndicator(new Vector3Int(coordinates.x,0, coordinates.z), obstacleColor,
                                PrimitiveType.Sphere);
                        }
                        
                    }
                    
                }
            }
        }
        
        private bool PlaceKnightPeice(MapData mapData, Vector3Int coordinates)
        {
            foreach (var knightPeice in mapData.KnightPeicesList)
            {
                if (knightPeice.Position == coordinates)
                {
                    return true;
                }
            }

            return false;
        }
        
        private void PlaceStartAndEndPoints(MapData mapData)
        {
            CreateIndicator(mapData.StartPoint, startColor, PrimitiveType.Cube);
            CreateIndicator(mapData.EndPoint, endColor, PrimitiveType.Cube);
        }
        
        
        public void CreateIndicator(Vector3Int position, Color color, PrimitiveType primitiveType)
        {
            var element = GameObject.CreatePrimitive(primitiveType);
            this._dictionaryOfObsticals.Add(position, element);

            element.transform.position = position + new Vector3(0.5f, 0.5f, 0.5f);
            element.transform.parent = _parent;
            var componentRenderer = element.GetComponent<Renderer>();
            componentRenderer.material.SetColor("_Color", color);
            
        }

        public void ClearMap()
        {
            foreach (var obstical in _dictionaryOfObsticals.Values)
            {
                Destroy(obstical);
            }
            _dictionaryOfObsticals.Clear();
        }
    }
}