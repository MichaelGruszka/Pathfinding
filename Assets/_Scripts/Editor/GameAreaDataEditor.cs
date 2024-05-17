using GameArea.Data;
using UnityEditor;
using UnityEngine;
namespace Editor
{
    [CustomEditor(typeof(GameAreaData))]
    public class GameAreaDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var gameAreaData = (GameAreaData)target;

            // Disable all GUI elements
            GUI.enabled = false;

            // Display all serialized properties using a default inspector
            DrawPropertiesExcluding(serializedObject, new string[] { });

            // Enable GUI elements back
            GUI.enabled = true;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
