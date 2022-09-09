using UnityEngine;
using DG.Tweening;
using LazyProger.ScenesConstantNames;

public class Startup : MonoBehaviour
{
    [SerializeField] private StartupMode _startupMode = StartupMode.MainMenu;
    [SerializeField] private LoadingScreen _loadingScreen;

    private void Start()
    {
        DOTween.Init();

        _loadingScreen.ShowImmediately();

        if (Application.isEditor && _startupMode == StartupMode.Gameplay)
        {
            SceneLoader.Load(Scenes.Gameplay, _loadingScreen);
        }
        else
        {
            SceneLoader.Load(Scenes.MainMenu, _loadingScreen);
        }
    }
}
