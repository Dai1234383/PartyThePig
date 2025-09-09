using UnityEngine;

public class EscapeThePigBait : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;

    private void Start()
    {
        Invoke(nameof(DestroySelf), _lifeTime);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
