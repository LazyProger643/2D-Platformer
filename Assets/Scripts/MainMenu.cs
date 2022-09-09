using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using LazyProger.ScenesConstantNames;

public class MainMenu : MonoBehaviour, ISceneLoadHandler<LoadingScreen>
{
    private LoadingScreen _loadingScreen;

    private void Start()
    {
        _loadingScreen.Hide();
    }

    public void OnStartButtonClick()
    {
        LeaveMenu(StartGameplay);
    }

    public void OnExitButtonClick()
    {
        LeaveMenu(ExitGame);
    }

    public void OnSceneLoaded(LoadingScreen loadingScreen)
    {
        _loadingScreen = loadingScreen;
    }

    private void LeaveMenu(Action action)
    {
        _loadingScreen.Show(() =>
        {
            action?.Invoke();
        });
    }

    private void StartGameplay()
    {
        SceneManager.UnloadSceneAsync(Scenes.MainMenu);
        SceneLoader.Load(Scenes.Gameplay, _loadingScreen);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
