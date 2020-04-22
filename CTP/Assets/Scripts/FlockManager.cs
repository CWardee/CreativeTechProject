using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public int TimeScale = 1;

    public GameObject WormToSpawn;
    public int NumberOfWorm;

    public GameObject PreadatorToSpawn;
    public int NumberOfPredators;

    public GameObject FishToSpawn;
    public int NumberOfFish;

    public GameObject AlgaeToSpawn;
    public int NumberOfAlgae;

    public int SpawnRadius;
    public int MinSpawnRadius;

    public GameObject Waypoint;
    int counter;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < NumberOfFish; i++)
        {
            Instantiate(FishToSpawn, new Vector3(Random.Range(-SpawnRadius / 2, SpawnRadius / 2),
                                                    Random.Range(-SpawnRadius / 5, SpawnRadius / 25),
                                                    Random.Range(-SpawnRadius / 2, SpawnRadius / 2)), Quaternion.Euler(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
        }



        for (int i = 0; i < NumberOfAlgae; i++)
        {
            Instantiate(AlgaeToSpawn, new Vector3(Random.Range(-SpawnRadius, SpawnRadius),
                                                    -SpawnRadius,
                                                    Random.Range(-SpawnRadius, SpawnRadius)), Quaternion.Euler(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
        }



        for (int i = 0; i < NumberOfPredators; i++)
        {
            Instantiate(PreadatorToSpawn, new Vector3(Random.Range(-SpawnRadius / 2, SpawnRadius / 2),
                                                    Random.Range(-SpawnRadius / 5, SpawnRadius / 5),
                                                    Random.Range(-SpawnRadius / 2, SpawnRadius / 2)), Quaternion.Euler(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
        }


        for (int i = 0; i < NumberOfWorm; i++)
        {
            Instantiate(WormToSpawn, new Vector3(Random.Range(-SpawnRadius, SpawnRadius),
                                                    -SpawnRadius + 0.2f, Random.Range(-SpawnRadius, SpawnRadius)), 
                                                    Quaternion.Euler(90, 0, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = TimeScale;
    }
}
