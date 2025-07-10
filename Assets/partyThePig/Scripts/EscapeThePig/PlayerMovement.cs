using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private int playerIndex; // 手動でインスペクターから設定
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private SpriteRenderer _spriteRenderer;    //画像

    private Vector2 moveInput;
    private Rigidbody2D rb;

    private PlayerInput input;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        playerIndex = input.playerIndex;  // 自動取得に変更

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private bool canMove = false;
    void Start()
    {
        if (PlayerManager.Instance != null && playerIndex >= 0 && playerIndex < PlayerManager.Instance.players.Length)
        {
            _spriteRenderer.color = PlayerManager.Instance.players[playerIndex].playerColor;
        }
        else
        {
            Debug.LogWarning($"PlayerManager が見つからないか、playerIndex が無効です: {playerIndex}");
        }
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(StartDelay());
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(3f);
        canMove = false;
    }

    void FixedUpdate()
    {
        if (canMove) return;
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);

        Vector2 newPosition = rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime;

        // 画面のワールド座標を取得（カメラの範囲を計算）
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));

        // 現在のスプライトのサイズを考慮（任意）
        float halfWidth = _spriteRenderer.bounds.size.x / 2f;
        float halfHeight = _spriteRenderer.bounds.size.y / 2f;

        // newPosition を画面内に制限
        newPosition.x = Mathf.Clamp(newPosition.x, bottomLeft.x + halfWidth, topRight.x - halfWidth);
        newPosition.y = Mathf.Clamp(newPosition.y, bottomLeft.y + halfHeight, topRight.y - halfHeight);

        // 移動
        rb.MovePosition(newPosition);
    }
}
