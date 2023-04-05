using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DebugHelper))]
public class DebugHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DebugHelper myScript = (DebugHelper)target;
        if (GUILayout.Button("Give relic"))
        {
            myScript.GiveRelic();
        }

        if (GUILayout.Button("Give gold"))
        {
            myScript.GiveGold();
        }

        if (GUILayout.Button("Update brightness"))
        {
            myScript.ChangeBrightness();
        }
    }
}