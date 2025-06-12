using UnityEngine;
using TMPro;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private TextMeshProUGUI nameText;

    public void Setup(PlayerData data)
    {
        body.color = data.playerColor;
        nameText.text = data.playerName;
    }
}
