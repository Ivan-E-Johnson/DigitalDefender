using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Color = UnityEngine.Color;

namespace Maps
{
    public class MapVisualizer : MonoBehaviour
    {
        private Transform _parent;
        public Color startColor = Color.green;
        public Color endColor = Color.red;

        [FormerlySerializedAs("obsticalColor")]
        public Color obstacleColor = Color.black;

        public Color knightColor = Color.yellow;

        public GameObject roadStraightPrefab;
        public GameObject roadCornerPrefab;
        public GameObject tileEmptyPrefab;
        public GameObject tileStartPrefab;
        public GameObject tileEndPrefab;

        public GameObject[] tileEnvironmentPrefabs;

        private Dictionary<Vector3Int, GameObject> _dictionaryOfObsticals = new();

        private void Awake()
        {
            _parent = transform;
        }
        
        public void ClearMap()
        {
            foreach (var obstical in _dictionaryOfObsticals.Values) Destroy(obstical);
            _dictionaryOfObsticals.Clear();
        }
        
        // This unified method will handle both prefab and primitive visualization based on a boolean flag.
        public void VisualizeMap(MapGrid mapGrid, bool visualizeUsingPrefabs)
        {
            // Set up the environment for the visualization process.


            // Determine the method of visualization and iterate through the map grid.
            for (var col = 0; col < mapGrid.Width; col++)
            {
                for (var row = 0; row < mapGrid.Length; row++)
                {
                    var position = new Vector3Int(col, 0, row);
                    var cell = mapGrid.GetCell(col, row);
                    VisualizeCell(cell, position, visualizeUsingPrefabs);
                }
            }
        }

        // Set the cell types based on obstacles and path.
        public void InitializeMapCells(MapGrid mapGrid, MapData mapData)
        {
            // Name might be slightly misleading as the start end end points are initialized int eh MapHelper class
            
            for (var j = 1; j < mapData.Path.Count-1; j++) // Do not overwrite the start and end points
            {
                var currentPosition = mapData.Path[j];
                bool isCorner = j > 0 && j < mapData.Path.Count - 1 &&
                                CheckIfPathCorner(mapData.Path[j - 1], currentPosition, mapData.Path[j + 1]);
                if (isCorner)
                {
                    mapGrid.SetCell(currentPosition.x,currentPosition.z, MapCellObjectType.Waypoint);
                    
                    mapGrid.waypoints.Add(new Vector3Int(currentPosition.x, 0, currentPosition.z));
                    
                }
                else
                {
                    mapGrid.SetCell(currentPosition.x,currentPosition.z, MapCellObjectType.Road);
                }
            }

            // Set cells for obstacles
            for (var i = 0; i < mapData.ObsticalesArray.Length; i++)
            {
                if (mapData.ObsticalesArray[i])
                {
                    var coordinates = mapGrid.CalculateCoordinatesFromIndex(i);
                    if (!_dictionaryOfObsticals.ContainsKey(coordinates))
                        mapGrid.SetCell(coordinates.x, coordinates.z, MapCellObjectType.Obstacle);
                }
            }
        }

        private bool CheckIfPathCorner(Vector3Int prev, Vector3Int current, Vector3Int next)
        {
            // Calculate direction vectors
            Vector3Int vector1 = new Vector3Int { x = current.x - prev.x, z = current.z - prev.z };
            Vector3Int vector2 = new Vector3Int { x = next.x - current.x, z = next.z - current.z };

            // Check if the direction changes
            return (vector1.x * vector2.z != vector1.z * vector2.x);
        }

        // Visualizes each cell based on its type and whether to use prefabs or primitives.

        private void VisualizeCell(MapCell cell, Vector3Int position, bool usePrefabs)
        {
            var identityQuaternion = Quaternion.identity;
            if (usePrefabs)
            {
                GameObject prefab = DeterminePrefab(cell);
                CreatePrefabIndicator(position, prefab, identityQuaternion);
            }
            else
            {
                Color color = DeterminePrimitiveColor(cell);
                if (color != Color.white)
                {
                    PrimitiveType shape = DeterminePrimitiveShape(cell);
                    CreateIndicator(position, color, shape);
                }

            }
        }


        private GameObject DeterminePrefab(MapCell cell)
        {
            switch (cell.ObjectType)
            {
                case MapCellObjectType.Start:
                    return tileStartPrefab;
                case MapCellObjectType.End:
                    return tileEndPrefab;
                case MapCellObjectType.Obstacle:
                    return tileEnvironmentPrefabs[Random.Range(0, tileEnvironmentPrefabs.Length)];
                case MapCellObjectType.Road:
                    return roadStraightPrefab;
                case MapCellObjectType.Waypoint:
                    return roadCornerPrefab; // TODO Determine orientation of prefab
                case MapCellObjectType.Empty:
                default:
                    return tileEmptyPrefab;
            }
        }


        private Color DeterminePrimitiveColor(MapCell cell)
        {
            switch (cell.ObjectType)
            {
                case MapCellObjectType.Start:
                    return startColor;
                case MapCellObjectType.End:
                    return endColor;
                case MapCellObjectType.Obstacle:
                    return obstacleColor;
                case MapCellObjectType.Waypoint:
                    return Color.cyan;  // Using knightColor for waypoints as an example
                default:
                    return Color.white;  // Default color for empty or undefined cell types
            }
        }

        private PrimitiveType DeterminePrimitiveShape(MapCell cell)
        {
            switch (cell.ObjectType)
            {
                case MapCellObjectType.Start:
                case MapCellObjectType.End:
                    return PrimitiveType.Cube;  // Using cubes for significant start/end points
                case MapCellObjectType.Obstacle:
                    return PrimitiveType.Sphere;  // Spheres for obstacles for a more pronounced effect
                case MapCellObjectType.Road:
                    return PrimitiveType.Cylinder;  // Cylinders might visually represent roads
                case MapCellObjectType.Waypoint:
                    return PrimitiveType.Capsule;  // Capsules for waypoints as a stylistic choice
                case MapCellObjectType.Empty:
                default:
                    return PrimitiveType.Cube;  // Default shape
            }
        }

        private void CreatePrefabIndicator(Vector3 position, GameObject prefab, Quaternion rotation = new())
        {
            var placementPosition = position + new Vector3(0.5f, 0.5f, 0.5f);
            var element = Instantiate(prefab, placementPosition, rotation);
            element.transform.parent = _parent;
            _dictionaryOfObsticals.Add(Vector3Int.RoundToInt(position), element);
        }

        private void CreateIndicator(Vector3Int position, Color color, PrimitiveType primitiveType)
        {
            var element = GameObject.CreatePrimitive(primitiveType);
            _dictionaryOfObsticals.Add(position, element);

            element.transform.position = position + new Vector3(0.5f, 0f, 0.5f);
            element.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            element.transform.parent = _parent;
            var componentRenderer = element.GetComponent<Renderer>();
            componentRenderer.material.SetColor("_Color", color);
        }


    }
}