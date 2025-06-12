using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThePigDeadLineCtrl : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private LayerMask _groundLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag==_playerTag)
        {
            JumpThePigGameStateManager.Instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (((1 << other.gameObject.layer) & _groundLayer) != 0)
        {
            Debug.Log("aaa");
            Destroy(other.gameObject);
        }
    }
}
