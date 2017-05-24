using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

[InitializeOnLoad]
public class Warning
{
    static Warning()
    {
        EditorApplication.update += RunOnce;
    }

    static void RunOnce()
    {
//        Debug.Log(EditorSceneManager.GetSceneManagerSetup()[0].path);
//        if (EditorSceneManager.GetSceneManagerSetup()[0].path != "Assets/WoodlandBubble/Scenes/game.unity")
//            EditorSceneManager.OpenScene("Assets/WoodlandBubble/Scenes/game.unity");
        if (!Directory.Exists("Assets/Plugins"))
        {   
            EditorUtility.DisplayDialog("Warning!", "Please move Plugins folder to Assets folder and reimport it", "Ok");
        }
        EditorApplication.update -= RunOnce;
    }
}