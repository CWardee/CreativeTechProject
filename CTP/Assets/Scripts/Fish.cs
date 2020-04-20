using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public GameManager gameManager;
    Rigidbody rigidBody;
    public float counter = 0;
    float maxCounter;
    public float speed = 25;
    GameObject leadFish;
    Fish fishScript;

    public int health;
    int maxHealth;

    public bool fishFound = false;
    public bool leaderFish = false;
    public bool predatorNear = false;
    bool nearLeaderFish = false;

   

    public Vector3 destination;
    Vector3 predatorDirection;
    Rigidbody predatorRig;

    public int currentFlockNum;


    public int hunger;
    public int hungerThreshhold;
    public bool hungry = false;
    bool nearFoodSource = false;
    public bool currentlyEating = false;


    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Predator")
        {

            //Rigidbody predRig = other.gameObject.GetComponent<Rigidbody>();
            predatorDirection = other.transform.position - transform.position;

            predatorDirection = other.transform.position;

            predatorDirection = transform.position - predatorDirection;

            //transform.LookAt(2 * transform.position - other.attachedRigidbody.position);

            predatorRig = other.gameObject.GetComponent<Rigidbody>();

            transform.LookAt(2 * transform.position - predatorRig.position);
            fishFound = false;
            leaderFish = false;
            predatorNear = true;
        }

        else if (other.name == "FoodSource" && hungry)
        {
            destination = other.transform.position;



            nearFoodSource = true;
            fishFound = false;
            leaderFish = false;
        }

        else if (other.name == "Fish" && !fishFound && leaderFish)
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

        else if (other.name == "Fish" && !fishFound && !leaderFish)
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

        else if (fishFound && other == fishScript)
        {
            nearLeaderFish = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Predator")
        {
            predatorNear = false;
            destination = this.gameObject.transform.position;
        }

        if (other.name == "FoodSource")
        {
            nearFoodSource = false;
        }

        if (fishFound && other == fishScript && !predatorNear)
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
        maxHealth = Random.Range(75, 125);
        hungerThreshhold = Random.Range(300, 400);

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
        if(fishScript == null)
        {
            fishFound = false;
        }

        if(health < 0)
        {
            if(!leaderFish)
            {
                gameManager.NumberOfFish--;
                Destroy(gameObject);
            }
        }

        if(!currentlyEating)
        {
            hunger++;
        }

        if (!predatorNear)
        {
            counter += Time.deltaTime;
        }

        if (predatorNear)
        {
            transform.position += this.gameObject.transform.forward * Time.deltaTime * 5;
        }

        else if (hunger > hungerThreshhold)
        {
            hungry = true;
            GameObject foodSourceToFind = GameObject.Find("FoodSource");
            destination = foodSourceToFind.transform.position;


            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, destination, step);

            if (Vector3.Distance(transform.position, destination) < 0.001f)
            {
                // Swap the position of the cylinder.
                destination *= -1.0f;
            }

            health--;
        }


        else if (currentlyEating)
        {
            hunger--;
            if(health < 100)
            {
                health++;
            }

            if (hunger < 0)
            {
                currentlyEating = false;
                hungry = false;
            }
        }

        else if (nearFoodSource)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, destination, step);

            if (Vector3.Distance(transform.position, destination) < 0.001f)
            {
                // Swap the position of the cylinder.
                destination *= -1.0f;
            }
        }


        else if(counter > maxCounter && !predatorNear)
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

        if(!predatorNear)
        {
            this.gameObject.transform.position = Vector3.Lerp(transform.position, destination, 1 * Time.deltaTime);
        }
    }
}
