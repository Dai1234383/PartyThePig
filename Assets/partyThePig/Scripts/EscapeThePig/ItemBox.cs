using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public GameObject[] itemPrefabs; // 出現させたいアイテムのプレハブリスト

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GiveRandomItem(other.gameObject);
            Destroy(gameObject); // ボックスを消す（1回限り）
        }
    }

    void GiveRandomItem(GameObject player)
    {
        // 安全チェック（nullや空配列なら何もしない）
        if (itemPrefabs == null || itemPrefabs.Length == 0)
        {
            Debug.LogWarning("ItemBox: itemPrefabsが設定されていません。");
            return;
        }

        // ランダムに1つ選んでフィールド上に出す
        int index = Random.Range(0, itemPrefabs.Length);
        GameObject item = Instantiate(itemPrefabs[index], transform.position, Quaternion.identity);

        // プレイヤーに装備させたい場合は、以下のように拡張可能：
        // var handler = player.GetComponent<PlayerItemHandler>();
        // if (handler != null)
        //     handler.SetItem(item);
    }
}