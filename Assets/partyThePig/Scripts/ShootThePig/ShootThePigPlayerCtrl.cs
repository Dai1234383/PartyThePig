using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootThePigPlayerCtrl : MonoBehaviour
{
    [Header("操作設定")]
    [SerializeField] private float _moveSpeed = 5f;



    [SerializeField] private int playerIndex; // 手動でインスペクターから設定
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private PlayerInput input;


    private Rigidbody2D _rb;
    private Vector2 _moveInput;



    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        playerIndex = input.playerIndex;  // 自動取得に変更
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (PlayerManager.Instance != null && playerIndex >= 0 && playerIndex < PlayerManager.Instance.players.Length)
        {
            _spriteRenderer.color = PlayerManager.Instance.players[playerIndex].playerColor;
        }
        else
        {
            Debug.LogWarning($"PlayerManager が見つからないか、playerIndex が無効です: {playerIndex}");
        }
    }


    private void FixedUpdate()
    {
        if (ShootThePigGameStateManager.Instance.GameState == ShootThePigGameStateManager.GameStateName.GAME)
        {
            Vector2 velocity = _rb.velocity;
            velocity.x = _moveInput.x * _moveSpeed;
            velocity.y = _moveInput.y * _moveSpeed;
            _rb.velocity = velocity;
        }
    }

    /// <summary>
    /// 移動入力（Invoke Unity Events で呼ばれる）
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (ShootThePigGameStateManager.Instance.GameState != ShootThePigGameStateManager.GameStateName.GAME) return;

        _moveInput = context.ReadValue<Vector2>();
    }


    /// <summary>
    /// 射撃入力
    /// </summary>
    /// <param name="context"></param>
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (ShootThePigGameStateManager.Instance.GameState != ShootThePigGameStateManager.GameStateName.GAME) return;
        if (context.performed)
        {
            Shoot();
        }
    }

    /// <summary>
    /// 重なっている Target を攻撃
    /// </summary>
    private void Shoot()
    {
        // プレイヤーのコライダーを取得
        Collider2D myCollider = GetComponent<Collider2D>();
        if (myCollider == null) return;

        // 重なっているコライダーのリストを格納
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true; // Triggerも対象にしたい場合
        filter.SetLayerMask(Physics2D.DefaultRaycastLayers); // 必要なら制限可
        filter.useLayerMask = true;

        List<Collider2D> results = new List<Collider2D>();
        myCollider.OverlapCollider(filter, results);

        // 重なっている Target を調べて Hit()
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
