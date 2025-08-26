using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class MoneyThePigPlayerCtrl : MonoBehaviour
{
    [Header("操作設定")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 10f;

    [Header("地面判定")]
    [SerializeField] private int _groundLayerNum = 6;

    [Header("ジャンプ効果音")]
    [SerializeField] private AudioClip jumpSE;
    [SerializeField] private AudioSource seAudioSource;

    [SerializeField] private int playerIndex; // 手動でインスペクターから設定
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private InputActionAsset _action;

    private PlayerInput _playerInput;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private bool _isGrounded = false;

    private AnimalAnimation _animalAnim;
    private bool _isAnime = false;

    public int PlayerIndex => playerIndex;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        playerIndex = _playerInput.playerIndex;  // 自動取得
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // スタート位置を設定
        transform.position = PlayerManager.Instance.GetStartPosition(playerIndex);

        if (PlayerManager.Instance != null && playerIndex >= 0 && playerIndex < PlayerManager.Instance.players.Length)
        {
            var playerPrefab = PlayerManager.Instance.players[playerIndex].playerSprite;
            if (playerPrefab != null)
            {
                // 子オブジェクトをクリア
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }

                // プレイヤーの見た目を生成
                GameObject playerObj = Instantiate(playerPrefab, transform);

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

        // 初回の接続デバイスを保存
        var currentDevice = _playerInput.devices.Count > 0 ? _playerInput.devices[0] : null;
        if (currentDevice != null)
        {
            PlayerManager.Instance.AssignDevice(playerIndex, currentDevice);
        }

        // 保存済みのデバイスを再ペアリング
        var savedDevice = PlayerManager.Instance.GetDevice(playerIndex);
        if (savedDevice != null)
        {
            _playerInput.user.UnpairDevices();
            InputUser.PerformPairingWithDevice(savedDevice, _playerInput.user);
        }
    }

    private void FixedUpdate()
    {
        if (MoneyThePigGameStateManager.Instance.GameState == MoneyThePigGameStateManager.GameStateName.OVER)
        {
            _playerInput.actions = _action;
            _playerInput.SwitchCurrentActionMap("UI");
        }

        if (MoneyThePigGameStateManager.Instance.GameState == MoneyThePigGameStateManager.GameStateName.GAME)
        {
            Vector2 velocity = _rb.velocity;
            velocity.x = _moveInput.x * _moveSpeed;
            _rb.velocity = velocity;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (MoneyThePigGameStateManager.Instance.GameState != MoneyThePigGameStateManager.GameStateName.GAME) return;

        _moveInput = context.ReadValue<Vector2>();

        if (_animalAnim != null && !_isAnime)
        {
            if (_moveInput != Vector2.zero)
            {
                _animalAnim.Walk();
            }
            else if (context.canceled)
            {
                _animalAnim.Idle();
            }
        }
    }

    public async void OnJump(InputAction.CallbackContext context)
    {
        if (MoneyThePigGameStateManager.Instance.GameState != MoneyThePigGameStateManager.GameStateName.GAME) return;

        if (context.performed && _isGrounded)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);

            // 🔊 ジャンプ効果音
            if (seAudioSource != null && jumpSE != null)
            {
                seAudioSource.PlayOneShot(jumpSE);
            }

            if (_animalAnim != null)
            {
                _animalAnim.Jump();
                _isAnime = true;
                await UniTask.Delay(1000);
                _animalAnim.Idle();
                _isAnime = false;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _groundLayerNum)
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _groundLayerNum)
        {
            _isGrounded = false;
        }
    }
}
