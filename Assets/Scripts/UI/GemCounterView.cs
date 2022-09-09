using UnityEngine;
using UnityEngine.UI;

public class GemCounterView : MonoBehaviour
{
    [SerializeField] private Text _text;

    private void Awake()
    {
        _text.text = "0";
    }

    public void OnGemCountChanged(int value)
    {
        _text.text = value.ToString();
    }
}
