using UnityEditor;
using UnityEngine;

public class PrefabReplacer : EditorWindow
{
    GameObject oldPrefab;
    GameObject newPrefab;

    [MenuItem("Tools/Prefab Replacer")]
    static void Open()
    {
        GetWindow<PrefabReplacer>("Prefab Replacer");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Replace Prefab In Scene", EditorStyles.boldLabel);

        oldPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Old Prefab",
            oldPrefab,
            typeof(GameObject),
            false
        );

        newPrefab = (GameObject)EditorGUILayout.ObjectField(
            "New Prefab",
            newPrefab,
            typeof(GameObject),
            false
        );

        GUI.enabled = oldPrefab && newPrefab;

        if (GUILayout.Button("Replace All Instances"))
        {
            ReplacePrefabs();
        }

        GUI.enabled = true;
    }

    void ReplacePrefabs()
    {
        foreach (GameObject obj in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            GameObject source = PrefabUtility.GetCorrespondingObjectFromSource(obj);

            if (source == oldPrefab)
            {
                Transform t = obj.transform;

                GameObject replacement = (GameObject)PrefabUtility.InstantiatePrefab(newPrefab, t.parent);

                replacement.transform.SetPositionAndRotation(t.position, t.rotation);
                replacement.transform.localScale = t.localScale;

                Object.DestroyImmediate(obj);
            }
        }
    }
}