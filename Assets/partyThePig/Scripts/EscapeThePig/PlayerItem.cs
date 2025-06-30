using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public GameObject[] usableItems; // プレイヤーが使えるアイテムPrefab（例：バナナ、スピードアップ）
    private GameObject currentItem;

    void Update()
    {
        if (currentItem != null && Input.GetKeyDown(KeyCode.Space)) // アイテム使用
        {
            UseItem();
        }
    }

    public void ReceiveRandomItem()
    {
        if (usableItems.Length == 0) return;

        int index = Random.Range(0, usableItems.Length);
        currentItem = usableItems[index];
        Debug.Log("アイテム獲得：" + currentItem.name);
    }

    void UseItem()
    {
        Instantiate(currentItem, transform.position, Quaternion.identity);
        Debug.Log("アイテム使用：" + currentItem.name);
        currentItem = null;
    }
}