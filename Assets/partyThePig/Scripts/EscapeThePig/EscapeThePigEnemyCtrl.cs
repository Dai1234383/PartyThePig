using UnityEngine;

public class EscapeThePigEnemyCtrl : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("ターゲットのタグ設定")]
    [SerializeField] private string playerTag = "Player";

    private Transform[] _players;
    private Rigidbody2D _rb;

    private bool _isStopped = false; // Trapなどで停止中かどうか

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // プレイヤーを取得
        GameObject[] playerObjs = GameObject.FindGameObjectsWithTag(playerTag);
        _players = new Transform[playerObjs.Length];
        for (int i = 0; i < playerObjs.Length; i++)
        {
            _players[i] = playerObjs[i].transform;
        }
    }

    private void Update()
    {
        if (EscapeThePigGameStateManager.Instance.GameState != EscapeThePigGameStateManager.GameStateName.GAME || _isStopped)
        {
            _rb.velocity = Vector2.zero;
            return;
        }

        Transform target = GetPriorityTarget();
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            _rb.velocity = direction * moveSpeed;
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    /// <summary>
    /// 優先的に追いかける対象（Bait → 最も近いプレイヤー）
    /// </summary>
    private Transform GetPriorityTarget()
    {
        // 優先：Bait
        GameObject bait = GameObject.FindWithTag("Bait");
        if (bait != null)
        {
            return bait.transform;
        }

        // プレイヤーの中から、不可視でない最も近い対象を探す
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (Transform player in _players)
        {
            if (player == null) continue;

            var invis = player.GetComponent<EscapeThePigInvisibility>();
            if (invis != null && invis.IsInvisible) continue;

            float dist = Vector2.Distance(transform.position, player.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = player;
            }
        }

        return closest;
    }

    /// <summary>
    /// Trapなどで一時停止させる
    /// </summary>
    public void Stop(float duration)
    {
        if (gameObject.activeInHierarchy) // 念のためチェック
        {
            StartCoroutine(StopRoutine(duration));
        }
    }

    private System.Collections.IEnumerator StopRoutine(float duration)
    {
        _isStopped = true;
        _rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(duration);
        _isStopped = false;
    }
}
