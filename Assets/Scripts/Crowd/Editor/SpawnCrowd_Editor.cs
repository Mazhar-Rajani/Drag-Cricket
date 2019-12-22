using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnCrowd))]
public class SpawnCrowd_Editor : Editor
{
    private SpawnCrowd spawnCrowd;

    private void OnEnable()
    {
        spawnCrowd = (SpawnCrowd)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Spawn Crowd"))
        {
            spawnCrowd.Spawn();
        }
        if (GUILayout.Button("Reset Crowd"))
        {
            spawnCrowd.ResetCrowd();
        }
    }
}