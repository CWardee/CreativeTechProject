using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    Rigidbody rigidBody;
    public float counter = 0;
    float maxCounter;
    public float speed = 25;
    GameObject leadFish;
    Fish fishScript;

    public bool fishFound = false;
    public bool leaderFish = false;
    bool nearLeaderFish = false;


    public Vector3 destination;

    public int currentFlockNum;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Fish" && !fishFound && leaderFish)
        {
            fishScript = other.GetComponent<Fish>();

            if(fishScript.currentFlockNum > currentFlockNum)
            {
                leaderFish = false;
                fishScript.currentFlockNum++;
                fishFound = true;
                leadFish = other.gameObject;
                var cubeRenderer = this.gameObject.GetComponent<Renderer>();
                cubeRenderer.material.SetColor("_Color", Color.white);
            }
        }

        if (other.name == "Fish" && !fishFound && !leaderFish)
        {
            fishScript = other.GetComponent<Fish>();
            if (fishScript.leaderFish == true)
            {
                fishScript.currentFlockNum++;
                fishFound = true;
                leadFish = other.gameObject;
                fishScript.currentFlockNum++;
            }
        }

        if (fishFound && other == fishScript)
        {
            nearLeaderFish = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (fishFound && other == fishScript)
        {
            nearLeaderFish = false;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(1, 3);
        rigidBody = this.GetComponent<Rigidbody>();
        maxCounter = Random.Range(5, 6);
        counter = maxCounter;
        this.gameObject.name = "Fish";

        int RandomLeaderFish = Random.Range(0, 5);

        var cubeRenderer = this.gameObject.GetComponent<Renderer>();
        cubeRenderer.material.SetColor("_Color", Color.white);

        if (RandomLeaderFish > 3)
        {
            leaderFish = true;
            cubeRenderer = this.gameObject.GetComponent<Renderer>();
            cubeRenderer.material.SetColor("_Color", Color.red);
        }

    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;


        if(counter > maxCounter)
        {
            if (!fishFound)
            {
                destination = new Vector3(this.gameObject.transform.position.x + Random.Range(-15, 15),
                    this.gameObject.transform.position.y + Random.Range(-15, 15),
                    this.gameObject.transform.position.z + Random.Range(-15, 15));
            }

            else if (fishFound)
            {
                destination = new Vector3(fishScript.gameObject.transform.position.x + Random.Range(-speed, speed),
                                fishScript.gameObject.transform.position.y + Random.Range(-speed, speed),
                                fishScript.gameObject.transform.position.z + Random.Range(-speed, speed));
            }


            counter = 0;
            maxCounter = Random.Range(-10.0f, 10.0f);
        }



        if(nearLeaderFish)
        {
            speed = 0;
        }

        else
        {
            speed = Random.Range(1.0f, 3.0f);
        }

        this.gameObject.transform.position = Vector3.Lerp(transform.position, destination, 1 * Time.deltaTime);
    }
}
