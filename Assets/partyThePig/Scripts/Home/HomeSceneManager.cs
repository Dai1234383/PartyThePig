using UnityEngine;

public class HomeSceneManager : MonoBehaviour
{
    public GameObject playerDisplayPrefab;
    public Transform[] spawnPoints;

    void Start()
    {
        for (int i = 0; i < PlayerManager.Instance.players.Length; i++)
        {
            var obj = Instantiate(playerDisplayPrefab, spawnPoints[i].position, Quaternion.identity);
            var display = obj.GetComponent<PlayerDisplay>();
            display.Setup(PlayerManager.Instance.players[i]);
        }
    }
}
