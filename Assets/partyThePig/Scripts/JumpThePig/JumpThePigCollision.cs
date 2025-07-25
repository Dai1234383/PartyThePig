using UnityEngine;

public class JumpThePigCollision : MonoBehaviour
{
    [Header("ínñ îªíË")]
    [SerializeField] private int _groundLayerNum = 6;

    private bool _isGrounded;

    public bool IsGrounded =>_isGrounded;
    /// <summary>
    /// ê⁄ínîªíË
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
    /// ê⁄ínîªíË
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
