using UnityEngine;

[System.Serializable]
public class EscapeThePigItemData
{
    public GameObject prefab;
    [Range(0f, 100f)]
    public float spawnRate;
}

public class EscapeThePigItemBox : MonoBehaviour
{
    [SerializeField] private EscapeThePigItemData[] itemTable;

    [Header("効果音")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _getItemClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var itemSystem = other.GetComponent<EscapeThePigPlayerItemSystem>();
        if (itemSystem == null || itemSystem.HasItem()) return;

        GameObject selectedItem = GetRandomItem();
        if (selectedItem != null)
        {
            // 🎵 効果音を鳴らす
            if (_audioSource != null && _getItemClip != null)
            {
                _audioSource.PlayOneShot(_getItemClip);
            }

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

