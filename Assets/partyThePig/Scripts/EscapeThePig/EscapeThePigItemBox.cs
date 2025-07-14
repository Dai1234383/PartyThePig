using UnityEngine;

[System.Serializable]
public class EscapeThePigItemData
{
    public GameObject prefab;
    [Range(0f, 100f)]
    public float spawnRate; // 出現率（合計100になる必要なし）
}

public class EscapeThePigItemBox : MonoBehaviour
{
    [SerializeField] private EscapeThePigItemData[] itemTable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var itemSystem = other.GetComponent<EscapeThePigPlayerItemSystem>();
        if (itemSystem == null || itemSystem.HasItem()) return;

        GameObject selectedItem = GetRandomItem();
        if (selectedItem != null)
        {
            itemSystem.ReceiveItem(selectedItem);
            Destroy(gameObject);
        }
    }

    private GameObject GetRandomItem()
    {
        float total = 0f;
        foreach (var item in itemTable)
        {
            total += item.spawnRate;
        }

        float rand = Random.Range(0f, total);
        float current = 0f;
        foreach (var item in itemTable)
        {
            current += item.spawnRate;
            if (rand <= current)
            {
                return item.prefab;
            }
        }
        return null;
    }
}
