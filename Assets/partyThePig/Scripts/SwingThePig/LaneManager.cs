using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public GameObject[] cowPrefabs;  // 普通牛(0),マイナス牛(1),金牛(2)
    public Transform spawnPoint;
    public int maxCowCount = 6;

    private List<GameObject> cows = new List<GameObject>();
    private float cowHeight = 1.1f;

    void Start()
    {
        for (int i = 0; i < maxCowCount; i++)
        {
            SpawnCow();
        }
    }

    void SpawnCow()
    {
        if (cows.Count >= maxCowCount) return;

        int rand = Random.Range(0, 100);
        int index = rand < 10 ? 1 : (rand < 20 ? 2 : 0);

        Vector3 pos = spawnPoint.position + Vector3.up * cows.Count * cowHeight;
        GameObject cow = Instantiate(cowPrefabs[index], pos, Quaternion.identity, transform);
        cow.GetComponent<Cow>().SetLaneManager(this);

        cows.Add(cow);
    }

    public void RemoveCow(GameObject cow)
    {
        int idx = cows.IndexOf(cow);
        if (idx < 0) return;

        cows.RemoveAt(idx);
        Destroy(cow);

        for (int i = 0; i < cows.Count; i++)
        {
            cows[i].transform.position = spawnPoint.position + Vector3.up * i * cowHeight;
        }

        SpawnCow();
    }

    public GameObject GetBottomCow()
    {
        return cows.Count > 0 ? cows[0] : null;
    }
}
