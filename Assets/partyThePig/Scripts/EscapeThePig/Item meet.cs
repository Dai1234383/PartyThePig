using UnityEngine;

public class Itemmeet : MonoBehaviour
{
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // ˆê’èŠÔŒã‚ÉÁ–Å
    }
}