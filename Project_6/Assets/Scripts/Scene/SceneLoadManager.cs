using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    //public bool isDontDestroy = false;

    //public string NowSceneName = "";

    //public override void Awake()
    //{
    //    base.Awake();

    //    NowSceneName = SceneManager.GetActiveScene().name;
    //}

    //public async void ChangeScene(string SceneName, Action callback = null, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    //{
    //    var op = SceneManager.LoadSceneAsync(SceneName, loadSceneMode);

    //    while (!op.isDone)
    //    {
    //        await Task.Yield();
    //    }

    //    if (loadSceneMode == LoadSceneMode.Single)
    //        NowSceneName = SceneName;

    //    callback?.Invoke();
    //}

    //public async void UnLoadScene(string SceneName, Action callback = null)
    //{
    //    var op = SceneManager.UnloadSceneAsync(SceneName);

    //    while (!op.isDone)
    //    {
    //        await Task.Yield();
    //    }

    //    callback?.Invoke();
    //}
}
