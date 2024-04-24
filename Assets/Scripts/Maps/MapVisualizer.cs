using System.Collections.Generic;
using System.Drawing;
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

        public void VisualizeMap(MapGrid mapGrid, MapData mapData, bool visualizeUsingPrefabs)
        {
            if (visualizeUsingPrefabs)
                VisualizeUsingPrefabs(mapGrid, mapData);
            else
                VisualizeUsingPrimitives(mapGrid, mapData);
        }

        private void VisualizeUsingPrefabs(MapGrid mapGrid, MapData mapData)
        {
            for (var i = 0; i < mapData.Path.Count; i++)
            {
                var vec3IntPathPosition = mapData.Path[i];
                if (vec3IntPathPosition != mapData.StartPoint.Position && vec3IntPathPosition != mapData.EndPoint.Position)
                    mapGrid.SetCell(vec3IntPathPosition.x, vec3IntPathPosition.z, MapCellObjectType.Road);
            }

            for (var col = 0; col < mapGrid.Width; col++)
            {
                for (var row = 0; row < mapGrid.Length; row++)
                {
                    var cell = mapGrid.GetCell(col, row);
                    var position = new Vector3(col, 0, row);
                    var index = mapGrid.CalculateIndexFromCoordinates(col, row);
                    if (mapData.ObsticalesArray[index] && cell.IsTaken) cell.ObjectType = MapCellObjectType.Obstacle;
                    

                    var identityQuaternion = Quaternion.identity;
                    switch (cell.ObjectType)
                    {
                        case MapCellObjectType.Empty:
                            CreatePrefabIndicator(position, tileEmptyPrefab, identityQuaternion);
                            break;
                        case MapCellObjectType.Waypoint:
                            // We may want to have visuals of checkpoints later but maybe not
                            CreatePrefabIndicator(position, tileEmptyPrefab, identityQuaternion);
                            break;
                        case MapCellObjectType.Start:
                            CreatePrefabIndicator(position, tileStartPrefab, identityQuaternion);
                            break;
                        case MapCellObjectType.End:
                            CreatePrefabIndicator(position, tileEndPrefab, identityQuaternion);
                            break;
                        case MapCellObjectType.Obstacle:
                            CreatePrefabIndicator(position,
                                tileEnvironmentPrefabs[Random.Range(0, tileEnvironmentPrefabs.Length)],
                                identityQuaternion);
                            break;
                        case MapCellObjectType.Road:
                            CreatePrefabIndicator(position, roadStraightPrefab,
                                identityQuaternion); // TODO Dynamicly change the road prefab
                            break;
                    }
                }
            }
        }
        
                private void VisualizeUsingPrimitives(MapGrid mapGrid, MapData mapData)
        {
            _PlaceStartAndEndPointsPrimatives(mapData);
            for (var i = 0; i < mapData.ObsticalesArray.Length; i++)
            {
                if (mapData.ObsticalesArray[i])
                {
                    var coordinates = mapGrid.CalculateCoordinatesFromIndex(i);
                    if (coordinates != mapData.StartPoint.Position && coordinates != mapData.EndPoint.Position &&
                        !_dictionaryOfObsticals.ContainsKey(coordinates))
                    {
                        if (PlaceKnightPeice(mapData, coordinates))
                            CreateIndicator(new Vector3Int(coordinates.x, 0, coordinates.z), knightColor,
                                PrimitiveType.Cylinder);
                        else
                            CreateIndicator(new Vector3Int(coordinates.x, 0, coordinates.z), obstacleColor,
                                PrimitiveType.Sphere);
                    }
                }
            }
            HighlightPathWithCorners(mapData);
        }

        private void HighlightPathWithCorners(MapData mapData)
        {
            if (mapData.Path.Count < 3) return; // Need at least three points to define a turn
            
            CreateIndicator(mapData.Path[0], Color.blue, PrimitiveType.Cube);
            
            // Iterate through path points to detect corners
            for (var j = 1; j < mapData.Path.Count - 1; j++)
            {
                var previousPosition = mapData.Path[j - 1];
                var currentPosition = mapData.Path[j];
                var nextPosition = mapData.Path[j + 1];
                
                if (CheckIfPathCorner(previousPosition, currentPosition, nextPosition)) 
                    CreateIndicator(currentPosition, Color.cyan, PrimitiveType.Cube);
                    
                else
                    CreateIndicator(currentPosition, Color.blue, PrimitiveType.Cube);
                    
            }
        }


        private void CreatePrefabIndicator(Vector3 position, GameObject prefab, Quaternion rotation = new())
        {
            var placementPosition = position + new Vector3(0.5f, 0.5f, 0.5f);
            var element = Instantiate(prefab, placementPosition, rotation);
            element.transform.parent = _parent;
            _dictionaryOfObsticals.Add(Vector3Int.RoundToInt(position), element);
        }



// Checks if there is a corner between three consecutive points
        public bool CheckIfPathCorner(Vector3Int prev, Vector3Int current, Vector3Int next)
        {
            // Calculate direction vectors
            Vector3Int vector1 = new Vector3Int { x = current.x - prev.x, z = current.z - prev.z };
            Vector3Int vector2 = new Vector3Int { x = next.x - current.x, z = next.z - current.z };

            // Check if the direction changes
            return (vector1.x * vector2.z != vector1.z * vector2.x);
        }
        private bool PlaceKnightPeice(MapData mapData, Vector3Int coordinates)
        {
            foreach (var knightPeice in mapData.KnightPeicesList)
                if (knightPeice.Position == coordinates)
                    return true;

            return false;
        }

        private void _PlaceStartAndEndPointsPrimatives(MapData mapData)
        {
            CreateIndicator(mapData.StartPoint.Position, startColor, PrimitiveType.Cube);
            CreateIndicator(mapData.EndPoint.Position, endColor, PrimitiveType.Cube);
        }


        public void CreateIndicator(Vector3Int position, Color color, PrimitiveType primitiveType)
        {
            var element = GameObject.CreatePrimitive(primitiveType);
            _dictionaryOfObsticals.Add(position, element);

            element.transform.position = position + new Vector3(0.5f, 0.5f, 0.5f);
            element.transform.parent = _parent;
            var componentRenderer = element.GetComponent<Renderer>();
            componentRenderer.material.SetColor("_Color", color);
        }

        public void ClearMap()
        {
            foreach (var obstical in _dictionaryOfObsticals.Values) Destroy(obstical);
            _dictionaryOfObsticals.Clear();
            
            
        }
    }
}