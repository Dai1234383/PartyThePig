using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStartPositionSetter : MonoBehaviour
{
    [SerializeField] private Vector2 leftStartPos = new Vector2(-6, 0);
    [SerializeField] private Vector2 rightStartPos = new Vector2(6, 0);

    private void Awake()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.SetStartPositions(leftStartPos, rightStartPos);
        }
    }
}
