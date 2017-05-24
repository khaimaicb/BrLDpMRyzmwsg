using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

//[CustomEditor(typeof(AdsController))]
public class AdEventsList : Editor
{
//    AdsController t;
    SerializedObject GetTarget;
    SerializedProperty ThisList;
    int ListSize;
    void OnEnable()
    {

//        t = (AdsController)target;
//        GetTarget = new SerializedObject(t);
        ThisList = GetTarget.FindProperty("adsEvents");
    }

    public override void OnInspectorGUI()
    {
        GetTarget.Update();
        ListSize = ThisList.arraySize;
        ListSize = EditorGUILayout.IntField("List Size", ListSize);

        if (ListSize != ThisList.arraySize)
        {
            while (ListSize > ThisList.arraySize)
            {
                ThisList.InsertArrayElementAtIndex(ThisList.arraySize);
            }
            while (ListSize < ThisList.arraySize)
            {
                ThisList.DeleteArrayElementAtIndex(ThisList.arraySize - 1);
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();


        for (int i = 0; i < ThisList.arraySize; i++)
        {
            SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
            SerializedProperty MygameEvent = MyListRef.FindPropertyRelative("gameEvent");
            SerializedProperty MyadType = MyListRef.FindPropertyRelative("adType");
            EditorGUILayout.BeginHorizontal();
            MygameEvent.enumValueIndex = (int)(GameState)EditorGUILayout.EnumPopup((GameState)Enum.GetValues(typeof(GameState)).GetValue(MygameEvent.enumValueIndex));
//            MyadType.enumValueIndex = (int)(AdType)EditorGUILayout.EnumPopup((AdType)Enum.GetValues(typeof(AdType)).GetValue(MyadType.enumValueIndex));
            //EditorGUILayout.PropertyField(MygameEvent);
            //EditorGUILayout.PropertyField(MyadType);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            ThisList.InsertArrayElementAtIndex(ThisList.arraySize);

        }
        if (GUILayout.Button("Delete"))
        {
            if (ThisList.arraySize > 0)
                ThisList.DeleteArrayElementAtIndex(ThisList.arraySize - 1);

        }
        EditorGUILayout.EndHorizontal();

        GetTarget.ApplyModifiedProperties();
    }
}
