using UnityEngine;
using System.Collections;
using UnityEditor;
//using ChartboostSDK;

[CustomEditor(typeof(ChartboostIntegration))]
public class ChartboostSettingsButton : Editor
{

    ChartboostIntegration t;
    int ListSize;

    void OnEnable()
    {

        t = (ChartboostIntegration)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        if (GUILayout.Button("Settings"))
        {
            //Selection.activeObject = CBSettings.Instance;

        }

    }
}
