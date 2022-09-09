using System;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void Load(string sceneName, Action<Scene> loaded = null)
    {
        LoadScene(sceneName, (scene) =>
        {
            loaded?.Invoke(scene);
        });
    }

    public static void Load<T>(string sceneName, T data, Action<Scene> loaded = null)
    {
        LoadScene(sceneName, (scene) =>
        {
            CallLoadHandlers(scene, data);
            loaded?.Invoke(scene);
        });
    }

    private static void LoadScene(string sceneName, Action<Scene> completed)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (scene.isLoaded)
        {
            completed(scene);
        }
        else
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += (op) =>
            {
                completed(SceneManager.GetSceneByName(sceneName));
            };
        }
    }

    private static void CallLoadHandlers<T>(Scene scene, T data)
    {
        ISceneLoadHandler<T>[] components = SceneObjectFinder.FindAll<ISceneLoadHandler<T>>(scene);

        foreach (ISceneLoadHandler<T> component in components)
        {
            component.OnSceneLoaded(data);
        }
    }
}
