using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using LazyProger.ScenesConstantNames;

public class Gameplay : MonoBehaviour, ISceneLoadHandler<LoadingScreen>
{
    [SerializeField] private int _levelCount;
    [SerializeField] private Player _player;

    [SerializeField] private UnityEvent<int> _levelStarted;
    [SerializeField] private UnityEvent _levelFinished;

    private const int FirstLevelNumber = 1;

    private int _currentLevel;
    private int _gemsLeftToCollect;

    private LoadingScreen _loadingScreen;

    private void Start()
    {
        _player.Deactivate();

        LoadLevel(FirstLevelNumber);
    }

    public void OnGemCollected()
    {
        _gemsLeftToCollect--;

        if (_gemsLeftToCollect <= 0)
        {
            if (_currentLevel == _levelCount)
            {
                ReturnToMainMenu();
            }
            else
            {
                LoadNextLevel();
            }
        }
    }

    public void OnExitButtonClick()
    {
        ReturnToMainMenu();
    }

    public void OnPlayerDied()
    {
        ReturnToMainMenu();
    }

    public void OnSceneLoaded(LoadingScreen loadingScreen)
    {
        _loadingScreen = loadingScreen;
    }

    private void LoadLevel(int level)
    {
        _currentLevel = level;

        LevelLoader.Load(level, OnLevelLoaded);
    }

    private void LoadNextLevel()
    {
        FinishLevel(() =>
        {
            LoadLevel(_currentLevel + 1);
        });
    }

    private void ReturnToMainMenu()
    {
        FinishLevel(() =>
        {
            SceneManager.UnloadSceneAsync(Scenes.Gameplay);
            SceneLoader.Load(Scenes.MainMenu, _loadingScreen);
        });
    }

    private void OnLevelLoaded(LevelData levelData)
    {
        _gemsLeftToCollect = levelData.GemCount;
        _player.transform.position = levelData.PlayerStartPosition;

        StartLevel(levelData.GemCount);
    }

    private void StartLevel(int gemCount)
    {
        _loadingScreen.Hide(() => _player.Activate());
        _levelStarted.Invoke(gemCount);
    }

    private void FinishLevel(Action finished)
    {
        _player.Deactivate();
        _loadingScreen.Show(() =>
        {
            LevelLoader.Unload(_currentLevel);

            finished.Invoke();
        });
        _levelFinished.Invoke();
    }
}
