using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class ShootThePigPlayerCtrl : MonoBehaviour
{
    [Header("操作設定")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("オーディオ設定")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootClip;

    [SerializeField] private int playerIndex;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private InputActionAsset _action;

    private PlayerInput _playerInput;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        playerIndex = _playerInput.playerIndex;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        transform.position = PlayerManager.Instance.GetStartPosition(playerIndex);

        if (PlayerManager.Instance != null && playerIndex >= 0 && playerIndex < PlayerManager.Instance.players.Length)
        {
            var playerPrefab = PlayerManager.Instance.players[playerIndex].playerSprite2;
            if (playerPrefab != null)
            {
                GameObject playerObj = Instantiate(playerPrefab, transform);
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

        var currentDevice = _playerInput.devices.Count > 0 ? _playerInput.devices[0] : null;
        if (currentDevice != null)
        {
            PlayerManager.Instance.AssignDevice(playerIndex, currentDevice);
        }

        var savedDevice = PlayerManager.Instance.GetDevice(playerIndex);
        if (savedDevice != null)
        {
            _playerInput.user.UnpairDevices();
            InputUser.PerformPairingWithDevice(savedDevice, _playerInput.user);
        }
    }

    private void FixedUpdate()
    {
        if (ShootThePigGameStateManager.Instance.GameState == ShootThePigGameStateManager.GameStateName.OVER)
        {
            _playerInput.actions = _action;
            _playerInput.SwitchCurrentActionMap("UI");
        }

        if (ShootThePigGameStateManager.Instance.GameState == ShootThePigGameStateManager.GameStateName.GAME)
        {
            Vector2 velocity = _rb.velocity;
            velocity.x = _moveInput.x * _moveSpeed;
            velocity.y = _moveInput.y * _moveSpeed;
            _rb.velocity = velocity;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (ShootThePigGameStateManager.Instance.GameState != ShootThePigGameStateManager.GameStateName.GAME) return;
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (ShootThePigGameStateManager.Instance.GameState != ShootThePigGameStateManager.GameStateName.GAME) return;

        if (context.performed)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // 🔊 効果音再生
        if (_audioSource != null && _shootClip != null)
        {
            _audioSource.PlayOneShot(_shootClip);
        }

        Collider2D myCollider = GetComponent<Collider2D>();
        if (myCollider == null) return;

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.SetLayerMask(Physics2D.DefaultRaycastLayers);
        filter.useLayerMask = true;

        List<Collider2D> results = new List<Collider2D>();
        myCollider.OverlapCollider(filter, results);

        foreach (var hit in results)
        {
            if (hit.CompareTag("Target"))
            {
                ShootThePigTargetCtrl targetCtrl = hit.GetComponent<ShootThePigTargetCtrl>();
                if (targetCtrl != null)
                {
                    targetCtrl.Hit(playerIndex);
                }
            }
        }
    }
}
