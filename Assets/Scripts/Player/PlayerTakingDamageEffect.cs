using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerTakingDamageEffect : MonoBehaviour
{
    [SerializeField, Range(0.1f, 5)] private float _blinkDuration = 1;
    [SerializeField, Min(1)] private int _blinkCount = 3;

    private SpriteRenderer _sprite;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void Blink()
    {
        int halfBlinkCount = _blinkCount * 2;
        float halfBlinkDuration = _blinkDuration / halfBlinkCount;

        _sprite.DOFade(0, halfBlinkDuration).SetLoops(halfBlinkCount, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
