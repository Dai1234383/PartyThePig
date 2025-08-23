using Cysharp.Threading.Tasks;
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

    private AnimalAnimation _animalAnim;
    private bool _isAnime = false;

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
        _animalAnim = GetComponentInChildren<AnimalAnimation>();

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

        if (_animalAnim != null && !_isAnime)
        {
            if (_moveInput != Vector2.zero)
            {
                _animalAnim.Walk(); // 入力あり → Walk
            }
            else if (context.canceled)
            {
                _animalAnim.Idle(); // 入力終了 → Idle
            }
        }
    }

    /// <summary>
    /// インタラクト入力（アイテム使用）
    /// </summary>
    public async void OnInteract(InputAction.CallbackContext context)
    {
        if (EscapeThePigGameStateManager.Instance.GameState != EscapeThePigGameStateManager.GameStateName.GAME) return;
        if (!context.performed) return;

        EscapeThePigPlayerItemSystem itemSystem = GetComponent<EscapeThePigPlayerItemSystem>();
        if (itemSystem != null)
        {
            itemSystem.UseItem();
        }

        if (_animalAnim != null)
        {
            _animalAnim.Eat(); // Eatアニメーションも再生
            _isAnime = true;
            await UniTask.Delay(500);
            _animalAnim.Idle();
            _isAnime = false;
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
