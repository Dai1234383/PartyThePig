using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class HomePlayerCtrl : MonoBehaviour
{
    [SerializeField] private int playerIndex; // 0 = 左, 1 = 右（シーンにあらかじめ設定）
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private InputActionAsset _action;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions = _action;
        _playerInput.SwitchCurrentActionMap("UI");
    }

    private void Start()
    {
        // 色をプレイヤー設定から適用
        if (PlayerManager.Instance != null && playerIndex >= 0 && playerIndex < PlayerManager.Instance.players.Length)
        {
            var playerPrefab = PlayerManager.Instance.players[playerIndex].playerSprite;
            if (playerPrefab != null)
            {
                // すでにオブジェクトがある場合は消しておく
                if (transform.childCount > 0)
                {
                    foreach (Transform child in transform)
                    {
                        Destroy(child.gameObject);
                    }
                }

                // Prefabをこのオブジェクトの子として生成
                GameObject playerObj = Instantiate(playerPrefab, transform);

                // 色を設定（SpriteRendererがある場合）
                var renderer = playerObj.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.color = PlayerManager.Instance.players[playerIndex].playerColor;
                }
            }
            else
            {
                Debug.LogWarning($"プレイヤーPrefabが設定されていません: {playerIndex}");
            }
        }
        else
        {
            Debug.LogWarning($"PlayerManager が見つからないか、playerIndex が無効です: {playerIndex}");
        }

        // 初回の接続デバイスを保存（未保存時のみ）
        var currentDevice = _playerInput.devices.Count > 0 ? _playerInput.devices[0] : null;
        if (currentDevice != null)
        {
            PlayerManager.Instance.AssignDevice(playerIndex, currentDevice);
        }

        // 保存済みのデバイスを再ペアリング（シーン再読み込み時など）
        var savedDevice = PlayerManager.Instance.GetDevice(playerIndex);
        if (savedDevice != null)
        {
            _playerInput.user.UnpairDevices(); // デバイスだけ解除（ユーザーは残す）
            InputUser.PerformPairingWithDevice(savedDevice, _playerInput.user); // 再ペアリング
        }
    }

}
