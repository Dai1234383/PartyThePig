using UnityEngine;

public class Itemmeet : MonoBehaviour
{
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // 一定時間後に消滅
    }
}