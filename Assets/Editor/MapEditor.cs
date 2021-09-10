using System.Collections;
using UnityEditor;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Editors
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MapGenerator map = target as MapGenerator;
            if (DrawDefaultInspector())
            {
                map.GenerateMap();
            }
            if(GUILayout.Button("Generate Map"))
            {
                map.GenerateMap();
            }
        }
    }
}