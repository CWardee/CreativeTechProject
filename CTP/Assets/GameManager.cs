using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int TimeScale = 1;
    public GameObject FishToSpawn;
    public int NumberOfFish;
    public int SpawnRadius;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < NumberOfFish; i++)
        {
            Instantiate(FishToSpawn, new Vector3(Random.Range(-SpawnRadius, SpawnRadius), Random.Range(-SpawnRadius, SpawnRadius), Random.Range(-SpawnRadius, SpawnRadius)), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = TimeScale;
    }
}
