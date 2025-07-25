using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class EscapeThePigPlayerCtrl : MonoBehaviour
{
    [Header("操作設定")]
    [SerializeField] private float _moveSpeed = 5f;

    [SerializeField] private int playerIndex; // 0 = 左, 1 = 右（シーンにあらかじめ設定）
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private InputActionAsset _action;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        transform.position = PlayerManager.Instance.GetStartPosition(playerIndex);

        // 色設定
        if (PlayerManager.Instance != null)
        {
            _spriteRenderer.sprite = PlayerManager.Instance.players[playerIndex].playerSprite;
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

    private void FixedUpdate()
    {
        if (EscapeThePigGameStateManager.Instance.GameState == EscapeThePigGameStateManager.GameStateName.OVER)
        {
            _playerInput.actions = _action;
            _playerInput.SwitchCurrentActionMap("UI");
        }

        if (EscapeThePigGameStateManager.Instance.GameState == EscapeThePigGameStateManager.GameStateName.GAME)
        {
            _rb.velocity = _moveInput * _moveSpeed;
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    /// <summary>
    /// 移動入力（Invoke Unity Events で呼ばれる）
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (EscapeThePigGameStateManager.Instance.GameState != EscapeThePigGameStateManager.GameStateName.GAME) return;

        _moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// インタラクト入力（アイテム使用）
    /// </summary>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (EscapeThePigGameStateManager.Instance.GameState != EscapeThePigGameStateManager.GameStateName.GAME) return;
        if (!context.performed) return;

        EscapeThePigPlayerItemSystem itemSystem = GetComponent<EscapeThePigPlayerItemSystem>();
        if (itemSystem != null)
        {
            itemSystem.UseItem();
        }
    }

    /// <summary>
    /// 敵に当たったらゲームオーバー
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            string winnerName = (playerIndex == 0) ? "Player2" : "Player1";
            EscapeThePigGameStateManager.Instance.GameOver(winnerName);
        }
    }

}
