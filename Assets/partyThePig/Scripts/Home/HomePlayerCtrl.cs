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
        var data = PlayerManager.Instance.GetPlayerData(playerIndex);
        if (data != null)
        {
            _spriteRenderer.sprite = data.playerSprite;
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
