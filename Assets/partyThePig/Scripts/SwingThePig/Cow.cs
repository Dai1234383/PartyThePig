using UnityEngine;

public class Cow : MonoBehaviour
{
    private LaneManager laneManager;

    public void SetLaneManager(LaneManager manager)
    {
        laneManager = manager;
    }

    private void OnMouseDown()
    {
        if (laneManager.GetBottomCow() == gameObject)
        {
            laneManager.RemoveCow(gameObject);
        }
        else
        {
            Debug.Log("ˆê”Ô‰º‚Ì‹‚¾‚¯Á‚¹‚é‚æ");
        }
    }
}
