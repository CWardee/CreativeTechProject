using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleMovemet : MonoBehaviour
{
    // State
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;


    public Transform objectToFollow;
    public Transform objectToAvoid;
    private Quaternion lookRotation;
    private Vector3 direction;
    public FlockManager manager;
    float radius;
    public Transform target;
    public float radiusx;
    public List<Transform> allObjects;

    public float speed;
    public float turnSpeed;
    float turnTime;
    float distanceToAvoid;
    public bool hungry = false;

    int numOfFish = 0;
    int maxNumOfFish = 12;
    public float hunger = 0;
    public float maxHunger;
    private float matingUrge = 0;
    private float maxMatingUrge = 50;

    public bool female = false;
    public bool mating = false;

    bool foodFound = false;
    bool hide = false;

    //check collider
    Collider[] objectsHit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Algae" && hungry)
        {
            hunger = 0;
        }

        if (other.name == "Fish")
        {
            numOfFish++;
        }

        if (mating)
        {
            Instantiate(this.gameObject, new Vector3(this.gameObject.transform.position.x,
                                                    this.gameObject.transform.position.y,
                                                    this.gameObject.transform.position.z), Quaternion.Euler(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));

            mating = false;
            matingUrge = 0;
        }
    }

    //check if fish exited
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Fish")
        {
            numOfFish--;
        }
    }

    //reset all values
    private void resetValues()
    {
        objectToAvoid = null;
        objectToFollow = null;
    }

    //move towrads object
    private void moveTowards()
    {
        //set object to look at
        direction = (objectToFollow.position - transform.position + objectToFollow.position).normalized;
        lookRotation = Quaternion.LookRotation(-direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * turnTime);

        lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, objectToFollow.rotation, Time.deltaTime * turnTime / 8);
    }

    //avoid object
    private void avoidObject()
    {
        //Debug.DrawLine(transform.position, objectToAvoid.gameObject.transform.position, Color.blue);
        direction = (objectToAvoid.position - transform.position + objectToAvoid.position).normalized;
        lookRotation = Quaternion.LookRotation(-direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f);

        if (Vector3.Distance(transform.position, objectToAvoid.position) > distanceToAvoid)
        {
            speed = 3;
            objectToAvoid = null;
        }
    }

    private void patrolArea()
    {
        if (Vector3.Distance(position, transform.position) > 4)
        {
            //draw line towards object
            //Debug.DrawLine(transform.position, position, Color.red);

            //set object to look at
            direction = (position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
        }

        else
        {
            GenerateRandomPos();
        }

        objectToFollow = null;
        objectToAvoid = null;
    }

    private void moveTowardsFood(Transform foodLocation)
    {
        //rotate to look at the player
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(foodLocation.position - transform.position), 3 * Time.deltaTime);
    }

    private void GenerateRandomPos()
    {
        if (!hungry || !hide)
        {
            //set radius
            radius = manager.SpawnRadius;
            radiusx = Random.Range(-radius, radius);
            float radiusy = Random.Range(-radius, radius - 25);
            float radiusz = Random.Range(-radius, radius);
            position = new Vector3(radiusx, radiusy, radiusz);
        }

        else if (hungry || hide)
        {
            //set radius
            radius = manager.SpawnRadius;
            radiusx = Random.Range(-radius, radius);
            float radiusy = Random.Range(-radius, -radius + 2);
            float radiusz = Random.Range(-radius, radius);
            position = new Vector3(radiusx, radiusy, radiusz);
        }
    }


    void Start()
    {
        resetValues();
        GenerateRandomPos();
        this.gameObject.name = "Fish";
        speed = Random.Range(speed, speed * 2);
        turnTime = Random.Range(turnSpeed, turnSpeed * 2);
        hunger = Random.Range(-35, 35);
    }


    void Update()
    {
        //get latest radius
        radius = manager.SpawnRadius;
        //go forward
        transform.position += this.gameObject.transform.forward * Time.deltaTime * speed;
        //set hunger
        hunger += Time.deltaTime;

        if (hunger > maxHunger)
        {
            hungry = true;
        }

        else
        {
            hungry = false;
        }

        //ensure fish is within bounds
        if (gameObject.transform.position.x > radius || gameObject.transform.position.x < -radius ||
            gameObject.transform.position.y > radius || gameObject.transform.position.y < -radius ||
            gameObject.transform.position.z > radius || gameObject.transform.position.z < -radius)
        {
            objectToFollow = null;
            objectToAvoid = null;
        }

        objectsHit = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider hit in objectsHit)
        {
            //if predator found
            if (hit.name == "Predator" && hit != this.gameObject)
            {
                resetValues();
                Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.red);

                distanceToAvoid = 35f;
                speed = 12;
                objectToAvoid = hit.gameObject.transform;
                direction = (objectToAvoid.position - transform.position - objectToAvoid.position);
                GenerateRandomPos();
                hide = true;
            }
        }

        objectsHit = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider hit in objectsHit)
        {
            //avoid obstactle
            if (hit.name == "Obstacle" && hit != this.gameObject || hit.name == "LargePredator" && hit != this.gameObject)
            {
                resetValues();
                Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.green);

                distanceToAvoid = 3f;
                objectToAvoid = hit.gameObject.transform;
                direction = (objectToAvoid.position - transform.position - objectToAvoid.position);
                GenerateRandomPos();
            }


            //if fish is not hungry
            if (!hungry)
            {
                //if fish found
                if (hit.name == "Fish" && hit != this.gameObject)
                {
                    resetValues();
                    Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.green);


                    objectToFollow = hit.gameObject.transform;
                    direction = (objectToFollow.position - transform.position + objectToFollow.position).normalized;
                }
            }

            //if fish is hungry
            else if (hungry)
            {
                // if algea found
                if (hit.name == "Algae" && hit != this.gameObject && hungry)
                {
                    resetValues();
                    distanceToAvoid = 1f;
                    Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.red);

                    //objectToFollow = hit.gameObject.transform;
                    //direction = (objectToFollow.position - transform.position + objectToFollow.position).normalized;
                    foodFound = true;
                    moveTowardsFood(hit.transform);
                }

                else
                {
                    foodFound = false;
                }
            }
        }

        //if object found avoid object
        if (objectToAvoid != null)
        {
            avoidObject();
        }

        //if object found move towards object
        else if (objectToFollow != null)
        {
            moveTowards();
        }

        //patrol
        else
        {
            patrolArea();
        }
    }
}
