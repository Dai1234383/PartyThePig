using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class TankThePigPlayerCtrl : MonoBehaviour
{
    [Header("プレイヤー設定")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotateSpeed = 200f;
    [SerializeField] private int _maxLife = 3;
    [SerializeField] private int playerIndex;

    [Header("オブジェクト参照")]
    [SerializeField] private SpriteRenderer[] _spriteRenderers;
    [SerializeField] private InputActionAsset _action;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Sprite _clearSprite;

    [Header("弾・サウンド")]
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private int _intervalTime = 2000;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootClip;

    private Rigidbody2D _rb;
    private PlayerInput _playerInput;
    private Vector2 _moveInput;
    private float _rotateInput;

    private bool _isRotate = false;
    private bool _isMove = false;
    private bool _isDamageInterval = false;
    private int _currentLife;
    private int _bulletInterval;

    private AnimalAnimation _animalAnim;
    private bool _isAnime = false;

    private Sprite[] _originalSprites;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        playerIndex = _playerInput.playerIndex;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // スタート位置をプレイヤーインデックスで設定
        transform.position = PlayerManager.Instance.GetStartPosition(playerIndex);

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

        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true)
            .Where(sr => sr.gameObject != this.gameObject)
            .ToArray();
        // 各 SpriteRenderer が持っている元スプライトを保存
        _originalSprites = _spriteRenderers.Select(sr => sr.sprite).ToArray();

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

        _currentLife = _maxLife;
    }

    private void FixedUpdate()
    {
        if (TankThePigGameStateManager.Instance.GameState == TankThePigGameStateManager.GameStateName.OVER)
        {
            _playerInput.actions = _action;
            _playerInput.SwitchCurrentActionMap("UI");
        }

        if (TankThePigGameStateManager.Instance.GameState != TankThePigGameStateManager.GameStateName.GAME) return;

        Vector2 moveDir = -transform.right * _moveInput.y;
        _rb.velocity = moveDir * _moveSpeed;

        if (Mathf.Abs(_rotateInput) > 0.1f)
        {
            _rb.MoveRotation(_rb.rotation + -_rotateInput * _rotateSpeed * Time.fixedDeltaTime);
        }

        _bulletInterval--;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (TankThePigGameStateManager.Instance.GameState != TankThePigGameStateManager.GameStateName.GAME) return;

        _rb.constraints = RigidbodyConstraints2D.None;
        if (!_isRotate)
        {
            _isMove = true;
            _moveInput = context.ReadValue<Vector2>();
        }

        if (context.canceled)
        {
            _isMove = false;
            _rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        if (_animalAnim != null && !_isAnime)
        {
            if (_moveInput != Vector2.zero) _animalAnim.Walk();
            else if (context.canceled) _animalAnim.Idle();
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (TankThePigGameStateManager.Instance.GameState != TankThePigGameStateManager.GameStateName.GAME) return;

        _rb.constraints = RigidbodyConstraints2D.None;

        if (!_isMove)
        {
            _isRotate = true;
            _rotateInput = context.ReadValue<float>();
        }

        if (_animalAnim != null && !_isAnime)
        {
            if (_moveInput != Vector2.zero) _animalAnim.Walk();
            else if (context.canceled) _animalAnim.Idle();
        }

        if (context.canceled)
        {
            _isRotate = false;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public async void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started && TankThePigGameStateManager.Instance.GameState == TankThePigGameStateManager.GameStateName.GAME)
        {
            if (_bulletInterval <= 0)
            {
                GameObject bullet = Instantiate(_bulletPrefab, transform.position + -transform.right * 0.6f, transform.rotation);
                _bulletInterval = _intervalTime;

                TankThePigBalletCtrl bulletCtrl = bullet.GetComponent<TankThePigBalletCtrl>();
                if (bulletCtrl != null) bulletCtrl.shooter = this.gameObject;

                SpriteRenderer bulletRenderer = bullet.GetComponent<SpriteRenderer>();
                if (bulletRenderer != null && PlayerManager.Instance != null)
                    bulletRenderer.color = PlayerManager.Instance.players[playerIndex].playerColor;

                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                if (bulletRb != null)
                    bulletRb.velocity = -transform.right * bulletSpeed;

                // ✅ 効果音を再生
                if (_audioSource != null && _shootClip != null)
                    _audioSource.PlayOneShot(_shootClip);

                if (_animalAnim != null)
                {
                    _animalAnim.Jump();
                    _isAnime = true;
                    await UniTask.Delay(300);
                    _animalAnim.Idle();
                    _isAnime = false;
                }
            }
        }
    }

    public void ReceiveBulletHit()
    {
        if (!_isDamageInterval) HitBullet();
    }

    private async void HitBullet()
    {
        _isDamageInterval = true;
        _currentLife--;
        TankThePigUIManager.Instance.UpdateLifeUI(playerIndex, _currentLife);

        if (_currentLife == 0)
        {
            string winner = playerIndex == 0 ? "Player2" : "Player1";
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            TankThePigGameStateManager.Instance.GameOver(winner);
        }

        for (int i = 0; i < 3; i++)
        {
            foreach (var sr in _spriteRenderers)
                if (sr != null) sr.sprite = _clearSprite;
            await UniTask.Delay(300);

            for (int j = 0; j < _spriteRenderers.Length; j++)
                if (_spriteRenderers[j] != null)
                    _spriteRenderers[j].sprite = _originalSprites[j];
            await UniTask.Delay(300);
        }

        _isDamageInterval = false;
    }
}
