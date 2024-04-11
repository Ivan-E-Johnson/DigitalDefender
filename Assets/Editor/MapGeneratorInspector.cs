using System;
using UnityEditor;
using UnityEngine;

namespace DigitalDefender
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorInspector : Editor
    {
        private MapGenerator map;

        private void OnEnable()
        {
            map = (MapGenerator)target; // ? 
            
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Application.isPlaying)
            {
                if (GUILayout.Button("Generate New Map"))
                {
                    map.GenerateNewMap();
                }
            }
        }
    }
}