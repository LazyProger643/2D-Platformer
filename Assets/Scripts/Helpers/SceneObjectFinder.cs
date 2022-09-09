using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneObjectFinder
{
    public static T FindFirst<T>(Scene scene)
    {
        T result = default;

        Find<T>(scene, (component) =>
        {
            result = component;
            return true;
        });

        return result;
    }

    public static T[] FindAll<T>(Scene scene)
    {
        List<T> components = new List<T>();

        Find<T>(scene, (component) =>
        {
            components.Add(component);
            return false;
        });

        return components.ToArray();
    }

    private static void Find<T>(Scene scene, Func<T, bool> foundComponentHandler)
    {
        foreach (GameObject gameObject in scene.GetRootGameObjects())
        {
            foreach (T component in gameObject.GetComponentsInChildren<T>())
            {
                if (foundComponentHandler(component))
                {
                    return;
                }
            }
        }
    }
}
