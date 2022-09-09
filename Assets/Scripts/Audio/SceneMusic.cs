using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SceneMusic : MonoBehaviour
{
    [SerializeField] private float _musicFadingDuration = 1;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
    }

    private void Start()
    {
        Play();
    }

    public void Play()
    {
        _audioSource.Play();
        _audioSource.DOFade(1, _musicFadingDuration);
    }

    public void Stop()
    {
        _audioSource.DOFade(0, _musicFadingDuration).OnComplete(() =>
        {
            _audioSource.Stop();
        });
    }
}
