using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Pathfinding
{
    [CustomEditor(typeof(NodeGrid))]
    public class NodeGridEditor : Editor
    {
        private SerializedProperty gridProp;

        private void OnEnable()
        {
            gridProp = serializedObject.FindProperty("_grid");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            NodeGrid nodeGrid = (NodeGrid)target;

            if (GUILayout.Button("Create Grid"))
            {
                nodeGrid.EmptyGrid();
                nodeGrid.CreateGrid();
            }
            if (GUILayout.Button("Empty Grid"))
            {
                nodeGrid.EmptyGrid();
            }
            if (GUILayout.Button("Create Nodes and Get Neightbourds"))
            {
                nodeGrid.EmptyGrid();
                nodeGrid.CreateGrid();
                nodeGrid.SearchNeightbourds();
            }
            if (GUILayout.Button("Get Neightbourds"))
            {
                nodeGrid.SearchNeightbourds();
            }

        }
    }
}
