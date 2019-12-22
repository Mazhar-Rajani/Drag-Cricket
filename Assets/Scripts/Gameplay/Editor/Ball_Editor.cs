using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Ball))]
public class Ball_Editor : Editor
{
    private Ball ball;

    private void OnEnable()
    {
        ball = (Ball)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Throw Ball"))
        {
            ball.ThrowBall();
        }

        if (GUILayout.Button("Reset Throw And Ball"))
        {
            ball.ResetThrow();
            ball.ResetBall();
        }
    }
}