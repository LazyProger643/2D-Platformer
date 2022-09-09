using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class SpriteEffect : MonoBehaviour {
    private void Awake()
    {
        Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}
