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
                if (vec3IntPathPosition != mapData.StartPoint && vec3IntPathPosition != mapData.EndPoint)
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

        private void CreatePrefabIndicator(Vector3 position, GameObject prefab, Quaternion rotation = new())
        {
            var placementPosition = position + new Vector3(0.5f, 0.5f, 0.5f);
            var element = Instantiate(prefab, placementPosition, rotation);
            element.transform.parent = _parent;
            _dictionaryOfObsticals.Add(Vector3Int.RoundToInt(position), element);
        }


        private void VisualizeUsingPrimitives(MapGrid mapGrid, MapData mapData)
        {
            _PlaceStartAndEndPointsPrimatives(mapData);
            for (var i = 0; i < mapData.ObsticalesArray.Length; i++)
            {
                if (mapData.ObsticalesArray[i])
                {
                    var coordinates = mapGrid.CalculateCoordinatesFromIndex(i);
                    if (coordinates != mapData.StartPoint && coordinates != mapData.EndPoint &&
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

            for (var j = 0; j < mapData.Path.Count; j++)
            {
                var pathPosition = mapData.Path[j];
                if (pathPosition != mapData.StartPoint && pathPosition != mapData.EndPoint)
                    CreateIndicator(pathPosition, Color.blue, PrimitiveType.Cube);
            }
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
            CreateIndicator(mapData.StartPoint, startColor, PrimitiveType.Cube);
            CreateIndicator(mapData.EndPoint, endColor, PrimitiveType.Cube);
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