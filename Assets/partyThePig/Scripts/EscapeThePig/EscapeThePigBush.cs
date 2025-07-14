using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 接触しているプレイヤーを一定時間敵から見えなくするBushアイテム
/// </summary>
public class EscapeThePigBush : MonoBehaviour
{
    [Header("Bushが存在する時間（秒）")]
    [SerializeField] private float activeDuration = 5f;

    private readonly List<EscapeThePigInvisibility> _inBushPlayers = new();

    private void Start()
    {
        // 一定時間で消滅
        Invoke(nameof(DestroySelf), activeDuration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var invis = other.GetComponent<EscapeThePigInvisibility>();
        if (invis != null && !_inBushPlayers.Contains(invis))
        {
            invis.SetInvisible(true);
            _inBushPlayers.Add(invis);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var invis = other.GetComponent<EscapeThePigInvisibility>();
        if (invis != null && _inBushPlayers.Contains(invis))
        {
            invis.SetInvisible(false);
            _inBushPlayers.Remove(invis);
        }
    }

    private void DestroySelf()
    {
        // Bushが消えるとき、全員をvisibleに戻す
        foreach (var invis in _inBushPlayers)
        {
            if (invis != null)
                invis.SetInvisible(false);
        }

        _inBushPlayers.Clear();
        Destroy(gameObject);
    }
}
