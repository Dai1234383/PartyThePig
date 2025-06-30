using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Transform playerTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerTarget = playerObj.transform;
    }

    void FixedUpdate()
    {
        Transform target = GetCurrentTarget();
        if (target == null) return;

        Vector2 direction = ((Vector2)target.position - rb.position).normalized;
        Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    //  プレイヤー or 一番近いエサをターゲットに選ぶ
    Transform GetCurrentTarget()
    {
        GameObject[] baits = GameObject.FindGameObjectsWithTag("Itemmeet");

        if (baits.Length == 0) return playerTarget;

        Transform closestBait = null;
        float minDistance = float.MaxValue;

        foreach (GameObject bait in baits)
        {
            float distance = Vector2.Distance(transform.position, bait.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBait = bait.transform;
            }
        }

        return closestBait != null ? closestBait : playerTarget;
    }
}
