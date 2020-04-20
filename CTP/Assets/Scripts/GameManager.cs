using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int TimeScale = 1;
    public GameObject FishToSpawn;
    public GameObject PredatorsToSpawn;
    public GameObject FoodToSpawn;
    public int NumberOfFish;
    public int NumberOfPredators;
    public int NumberOfFood;
    public int SpawnRadius;

    public Image BarChart_NumOfFish;
    public Image BarChart_NumOfPredators;
    public Image BarChart_NumOfFood;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < NumberOfFish; i++)
        {
            Instantiate(FishToSpawn, new Vector3(Random.Range(-SpawnRadius, SpawnRadius), 
                                                    Random.Range(-SpawnRadius, SpawnRadius), 
                                                    Random.Range(-SpawnRadius, SpawnRadius)), Quaternion.identity);
        }

        for (int i = 0; i < NumberOfPredators; i++)
        {
            Instantiate(PredatorsToSpawn, new Vector3(Random.Range(-SpawnRadius, SpawnRadius), 
                                                        Random.Range(-SpawnRadius, SpawnRadius), 
                                                        Random.Range(-SpawnRadius, SpawnRadius)), Quaternion.identity);
        }

        for (int i = 0; i < NumberOfFood; i++)
        {
            Instantiate(FoodToSpawn, new Vector3(Random.Range(-SpawnRadius, SpawnRadius), 
                                                    Random.Range(-SpawnRadius, SpawnRadius), 
                                                    Random.Range(-SpawnRadius, SpawnRadius)), Quaternion.identity);
        }



    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = TimeScale;

        BarChart_NumOfFish.rectTransform.sizeDelta = new Vector2(0, NumberOfFish * 10);
        BarChart_NumOfPredators.rectTransform.sizeDelta = new Vector2(0, NumberOfPredators * 10);
        BarChart_NumOfFood.rectTransform.sizeDelta = new Vector2(0, NumberOfFood * 10);
    }
}
