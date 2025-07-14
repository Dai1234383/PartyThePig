using UnityEngine;
using UnityEngine.Events;

public class EscapeThePigPlayerItemSystem : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;

    private GameObject _heldItemPrefab;

    // アイテム変更時に通知するイベント
    public UnityEvent<GameObject> OnItemChanged = new();

    public void ReceiveItem(GameObject item)
    {
        _heldItemPrefab = item;
        OnItemChanged?.Invoke(_heldItemPrefab);
    }

    public void UseItem()
    {
        if (_heldItemPrefab != null)
        {
            GameObject item = Instantiate(_heldItemPrefab, _spawnPoint.position, Quaternion.identity);

            // Trapの場合、設置者をセット
            var trap = item.GetComponent<EscapeThePigTrap>();
            if (trap != null)
                trap.SetOwner(gameObject);

            _heldItemPrefab = null;
            OnItemChanged?.Invoke(null); // 所持アイテム消失を通知
        }
    }

    public bool HasItem() => _heldItemPrefab != null;

    public GameObject GetHeldItem() => _heldItemPrefab;
}
