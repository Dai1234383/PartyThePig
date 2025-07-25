using UnityEngine;

public class JumpThePigCollision : MonoBehaviour
{
    [Header("地面判定")]
    [SerializeField] private int _groundLayerNum = 6;

    private bool _isGrounded;

    public bool IsGrounded =>_isGrounded;
    /// <summary>
    /// 接地判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _groundLayerNum)
        {
            _isGrounded = true;
        }
    }

    /// <summary>
    /// 接地判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _groundLayerNum)
        {
            _isGrounded = false;
        }
    }

}
