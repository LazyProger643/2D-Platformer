using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private float _showAndHideDuration = 1f;

    private const float MinAlphaValue = 0;
    private const float MaxAlphaValue = 1;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void ShowImmediately()
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, MaxAlphaValue);
    }

    public void Hide(Action completed = null)
    {
        _image.DOFade(MinAlphaValue, _showAndHideDuration).OnComplete(() =>
        {
            completed?.Invoke();
        });
    }

    public void Show(Action completed = null)
    {
        _image.DOFade(MaxAlphaValue, _showAndHideDuration).OnComplete(() =>
        {
            completed?.Invoke();
        });
    }
}
