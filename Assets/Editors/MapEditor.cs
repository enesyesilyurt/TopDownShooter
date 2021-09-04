using System.Collections;
using UnityEngine;
using UnityEditor;
using Assets.Scripts;

namespace Assets.Editors
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            MapGenerator map = target as MapGenerator;
            map.GenerateMap();
        }
    }
}