using UnityEngine;

public class EscapeThePigItemBoxSpawner : MonoBehaviour
{
    [Header("出現させるアイテムボックスのプレハブ")]
    [SerializeField] private GameObject itemBoxPrefab;

    [Header("出現間隔（秒）")]
    [SerializeField] private float spawnInterval = 10f;

    [Header("出現範囲（ワールド座標）")]
    [SerializeField] private Vector2 spawnAreaMin; // 左下
    [SerializeField] private Vector2 spawnAreaMax; // 右上

    [Header("同時に存在できる最大数")]
    [SerializeField] private int maxItemBoxes = 3;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= spawnInterval)
        {
            TrySpawnItemBox();
            _timer = 0f;
        }
    }

    private void TrySpawnItemBox()
    {
        if (EscapeThePigGameStateManager.Instance.GameState != EscapeThePigGameStateManager.GameStateName.GAME) return;

        // すでに存在しているアイテムボックスを数える
        int currentCount = GameObject.FindGameObjectsWithTag("ItemBox").Length;
        if (currentCount >= maxItemBoxes) return;

        Vector2 spawnPos = GetRandomPositionInArea();
        Instantiate(itemBoxPrefab, spawnPos, Quaternion.identity);
    }

    private Vector2 GetRandomPositionInArea()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector2(x, y);
    }

    private void OnDrawGizmosSelected()
    {
        // 出現範囲の可視化
        Gizmos.color = Color.green;
        Vector3 center = (spawnAreaMin + spawnAreaMax) / 2;
        Vector3 size = spawnAreaMax - spawnAreaMin;
        Gizmos.DrawWireCube(center, size);
    }
}
