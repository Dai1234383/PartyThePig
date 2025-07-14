using UnityEngine;

public class EscapeThePigBait : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        Invoke(nameof(DestroySelf), lifeTime);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
