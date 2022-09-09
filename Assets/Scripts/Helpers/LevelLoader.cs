using System;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    public static void Load(int level, Action<LevelData> loaded)
    {
        SceneLoader.Load(GetLevelSceneName(level), (scene) =>
        {
            loaded.Invoke(GetLevelDataHolder(scene));
        });
    }

    public static void Unload(int level)
    {
        SceneManager.UnloadSceneAsync(GetLevelSceneName(level));
    }

    private static string GetLevelSceneName(int level)
    {
        return $"Level{level}";
    }

    private static LevelData GetLevelDataHolder(Scene scene)
    {
        LevelData levelData = SceneObjectFinder.FindFirst<LevelData>(scene);

        if (levelData == null)
        {
            throw new NullReferenceException($"LevelLoader: The LevelData component was not found on the scene {scene.name}.");
        }

        return levelData;
    }
}
