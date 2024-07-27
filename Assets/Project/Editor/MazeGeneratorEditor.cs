#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MazeGenerator script = (MazeGenerator)target;
        if (GUILayout.Button("Generate Maze"))
        {
            script.GenerateMaze();
        }
    }
}
#endif