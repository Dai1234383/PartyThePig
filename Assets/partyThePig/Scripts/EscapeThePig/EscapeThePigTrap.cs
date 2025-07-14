using UnityEngine;
using System.Collections;

public class EscapeThePigTrap : MonoBehaviour
{
    [SerializeField] private float disableDuration = 2f; // í‚é~éûä‘
    [SerializeField] private float lifeTime = 10f;

    private GameObject _owner;

    public void SetOwner(GameObject owner)
    {
        _owner = owner;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == _owner) return;

        if (other.CompareTag("Player"))
        {
            var rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
                StartCoroutine(FreezeTarget(rb));
        }
        else if (other.CompareTag("Enemy"))
        {
            var enemyCtrl = other.GetComponent<EscapeThePigEnemyCtrl>();
            if (enemyCtrl != null)
            {
                enemyCtrl.Stop(disableDuration); // Å© Ç±ÇÍÇ≈í‚é~Ç≥ÇπÇÈ
            }
        }
    }

    private IEnumerator FreezeTarget(Rigidbody2D rb)
    {
        Vector2 originalVelocity = rb.velocity;

        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(disableDuration);

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // âÒì]ÇæÇØå≈íËÅiå≥Ç…ñﬂÇ∑Åj
        rb.velocity = originalVelocity;
    }
}
