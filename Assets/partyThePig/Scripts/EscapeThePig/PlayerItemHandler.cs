using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemHandler : MonoBehaviour
{
    private GameObject currentItem;

    public void SetItem(GameObject item)
    {
        currentItem = item;
        currentItem.SetActive(false); // プレイヤーが持っている状態
    }

    void Update()
    {
        if (currentItem != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            UseItem();
        }
    }

    void UseItem()
    {
        currentItem.transform.position = transform.position;
        currentItem.SetActive(true); // 表示して発動
        currentItem = null;
    }
}