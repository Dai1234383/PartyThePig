using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBush : MonoBehaviour
{
    public float InvisibilityDuration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            PlayerStatus status = other.GetComponent<PlayerStatus>();
            if (status != null)
            {
                status.ActivateInvisibility(InvisibilityDuration);
            }

            Destroy(gameObject);

        }
    }

}
