using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.IO;
using System;

public class ObjectPoolMaker : EditorWindow
{
    Vector2 pos = Vector2.zero;
    List<GameObject> objects = new List<GameObject>();
    List<int> amounts = new List<int>();

    [MenuItem("Editor/ObjectPoolMaker")]
    static void ShowWindow()
    {
        GetWindow(typeof(ObjectPoolMaker)).Show();
    }

    private void OnEnable()
    {
        SetObject();
    }

    private void OnDisable()
    {
        objects.Clear();
        amounts.Clear();
    }

    private void OnGUI()
    {
        pos = EditorGUILayout.BeginScrollView(pos);
        for(int i = 0; i < objects.Count; i++)
        {
            string name = string.Empty;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(i.ToString(),GUILayout.MaxWidth(30));
            name = EditorGUILayout.TextField(objects[i].name);
            amounts[i] = EditorGUILayout.IntField(amounts[i]);

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("생성"))
        {
            CreateData();
        }

        if (GUILayout.Button("새로고침"))
        {
            ResetWindow();
        }
        EditorGUILayout.EndScrollView();

    }

    private void SetObject()
    {
        Addressables.LoadAssetsAsync<GameObject>("Prefab", (g) =>
        {
            objects.Add(g);
            amounts.Add(0);
        });
    }

    private void CreateData()
    {
        ObjectPoolData pooldata = new ObjectPoolData();

        for(int i = 0; i < objects.Count; i++)
        {
            PoolData data = new PoolData(objects[i].name, amounts[i]);
            pooldata.data.Add(data);
        }

        string json = JsonUtility.ToJson(pooldata, true);

        File.WriteAllText(Application.dataPath + "/AddressableDatas/ObjectPoolData.json", json);
    }

    private void ResetWindow()
    {
        objects.Clear();
        amounts.Clear();

        SetObject();
    }
}
