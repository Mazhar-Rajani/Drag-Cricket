using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnCrowd_Linear))]
public class SpawnCrowdLinear_Editor : Editor
{
    private SpawnCrowd_Linear spawnCrowd_Linear;

    private void OnEnable()
    {
        spawnCrowd_Linear = (SpawnCrowd_Linear)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Spawn Crowd"))
        {
            spawnCrowd_Linear.Spawn();
        }
        if (GUILayout.Button("Reset Crowd"))
        {
            spawnCrowd_Linear.ResetCrowd();
        }
    }
}