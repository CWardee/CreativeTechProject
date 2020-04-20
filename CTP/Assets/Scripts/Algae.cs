using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algae : MonoBehaviour
{
    Flock fishScript;
    FlockManager gameManager;
    void OnCollisionEnter(Collision collision)
    {
        float spawnRadius = gameManager.SpawnRadius;
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.name == "Fish")
        {
            fishScript = collision.gameObject.GetComponent<Flock>();
            fishScript.hunger = 0;
            fishScript.hungry = false;

            this.gameObject.transform.position = new Vector3(Random.Range(-spawnRadius, spawnRadius),
                                        -spawnRadius,
                                        Random.Range(-spawnRadius, spawnRadius));
        }

        if (collision.gameObject.name == "Obstactle")
        {
            this.gameObject.transform.position = new Vector3(Random.Range(-spawnRadius, spawnRadius),
                                        Random.Range(-spawnRadius / 1.5f, -spawnRadius / 1.5f),
                                        Random.Range(-spawnRadius, spawnRadius));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.name = "Algae";
        gameManager = GameObject.Find("GameManager").GetComponent<FlockManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
