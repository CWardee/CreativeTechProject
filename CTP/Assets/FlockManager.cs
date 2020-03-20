using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public int TimeScale = 1;
    public GameObject FishToSpawn;

    public int NumberOfFish;
    public float SpawnRadius;

    public GameObject Waypoint;
    int counter;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < NumberOfFish; i++)
        {
            Instantiate(FishToSpawn, new Vector3(Random.Range(-SpawnRadius, SpawnRadius),
                                                    Random.Range(-SpawnRadius, SpawnRadius),
                                                    Random.Range(-SpawnRadius, SpawnRadius)), Quaternion.Euler(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = TimeScale;
    }
}
