using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int TimeScale = 1;
    public GameObject FishToSpawn;
    public GameObject PredatorsToSpawn;
    public int NumberOfFish;
    public int NumberOfPredators;
    public int SpawnRadius;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < NumberOfFish; i++)
        {
            Instantiate(FishToSpawn, new Vector3(Random.Range(-SpawnRadius, SpawnRadius), Random.Range(-SpawnRadius, SpawnRadius), Random.Range(-SpawnRadius, SpawnRadius)), Quaternion.identity);
        }

        for (int i = 0; i < NumberOfPredators; i++)
        {
            Instantiate(PredatorsToSpawn, new Vector3(Random.Range(-SpawnRadius, SpawnRadius), Random.Range(-SpawnRadius, SpawnRadius), Random.Range(-SpawnRadius, SpawnRadius)), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = TimeScale;
    }
}
