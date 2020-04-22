using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleMovemet : MonoBehaviour
{
    // State
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;

    ModuleMovemet fishScript;

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
    public float matingUrge = 0;
    public float maxMatingUrge = 50;

    bool avoidingObject = false; 

    public bool female;
    public bool mating = false;

    bool foodFound = false;
    public bool hide = false;
    public float age = 0;
    //check collider
    Collider[] objectsHit;

    public GameObject foodToEat;

    public float chance;

    private void OnTriggerEnter(Collider other)
    {

        ////if predator found
        //if (other.name == "Predator" || other.name == "Obstacle")
        //{
        //    Debug.DrawLine(transform.position, other.gameObject.transform.position, Color.cyan);

        //    distanceToAvoid = 5f;
        //    speed = 9;
        //    objectToAvoid = other.gameObject.transform;
        //    direction = (objectToAvoid.position - transform.position + objectToAvoid.position);
        //    //rotate to look at the player

        //    //if predator found
        //    if (other.name == "Fish")
        //    {
        //        Debug.DrawLine(transform.position, other.gameObject.transform.position, Color.cyan);

        //        distanceToAvoid = 5f;
        //        speed = 9;
        //        objectToAvoid = other.gameObject.transform;
        //        //direction = (objectToAvoid.position - transform.position + objectToAvoid.position);
        //        //rotate to look at the player
        //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(other.transform.position + transform.position), 5 * Time.deltaTime);
        //        //hide = true;
        //    }
        //}

        // if algea found
        if (other.name == "Algae" && other != this.gameObject && hungry)
        {
            foodToEat = other.gameObject;
        }

        //else
        //{
        //    foodToEat = null;
        //}

        //if (other.name == "Algae" && hungry)
        //{
        //    hunger = 0;
        //}

        if (other.name == "Fish" && !hungry)
        {
            numOfFish++;
            fishScript = other.GetComponent<ModuleMovemet>();

            if(fishScript.mating && mating && this.gameObject.transform.position.y < -radius + 25 && female == false)
            {
                if(female == false && fishScript.female == true)
                {
                    mating = false;
                    hide = false;
                    matingUrge = 0;

                    Instantiate(this.gameObject, new Vector3(this.gameObject.transform.position.x + 1,
                                                            this.gameObject.transform.position.y,
                                                            this.gameObject.transform.position.z), Quaternion.Euler(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
                }


                else if (female == true && fishScript.female == false)
                {
                    mating = false;
                    hide = false;
                    matingUrge = 0;

                    Instantiate(this.gameObject, new Vector3(this.gameObject.transform.position.x + 1,
                                                            this.gameObject.transform.position.y,
                                                            this.gameObject.transform.position.z), Quaternion.Euler(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
                }

            }



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
        direction = (objectToAvoid.position - transform.position + objectToAvoid.position);
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
            float radiusx = Random.Range(-radius, radius);
            float radiusy = Random.Range(-radius, radius - 25);
            float radiusz = Random.Range(-radius, radius);
            position = new Vector3(radiusx, radiusy, radiusz);
        }

        else if (hungry || hide)
        {
            //set radius
            radius = manager.SpawnRadius;
            float radiusx = Random.Range(-radius, radius);
            float radiusy = Random.Range(-radius + 1, -radius);
            float radiusz = Random.Range(-radius, radius);
            position = new Vector3(radiusx, radiusy, radiusz);
        }
    }


    void Start()
    {
        hide = false;
        resetValues();
        GenerateRandomPos();
        this.gameObject.name = "Fish";
        speed = Random.Range(speed, speed * 2);
        turnTime = Random.Range(turnSpeed, turnSpeed * 2);
        hunger = Random.Range(-35, 35);
        chance = Random.Range(-50, 50);
        age = Random.Range(-50, 0);

        if (chance < 0)
        {
            female = false;
        }

        else
        {
            female = true;
        }
    }



    void Update()
    {
        GenerateRandomPos();
        //set age
        age+= Time.deltaTime;

        if (hide == true && this.gameObject.transform.position.y < -radius + 10)
        {
            hide = false;
        }
        //get latest radius
        radius = manager.SpawnRadius;

        //go forward
        transform.position += this.gameObject.transform.forward * Time.deltaTime * speed;

        //set hunger
        hunger += Time.deltaTime;

        if (hunger > maxHunger)
        {
            hungry = true;
            resetValues();
        }

        else
        {
            hungry = false;
        }

        //set mating urge
        matingUrge += Time.deltaTime;

        if (matingUrge > maxMatingUrge && age < 80)
        {
            mating = true;
            hide = true;
        }

        else
        {
            mating = false;
        }

        //ensure predator is within bounds
        if (position.x > radius || position.x < -radius ||
            position.y > radius || position.y < -radius ||
            position.z > radius || position.z < -radius)
        {
            GenerateRandomPos();
            resetValues();
            objectToFollow = null;
            objectToAvoid = null;
        }

        objectsHit = Physics.OverlapSphere(transform.position, 6f);
        foreach (Collider hit in objectsHit)
        {
            //if predator found
            if (hit.name == "Predator" && hit != this.gameObject)
            {
                GenerateRandomPos();
                resetValues();
                Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.red);

                distanceToAvoid = 10f;
                speed = 7;
                objectToAvoid = hit.gameObject.transform;
                direction = (objectToAvoid.position - transform.position - objectToAvoid.position);
                //hide = true;

            }


            //if predator found
            else if (hit.name == "Algea" && hit != this.gameObject && hungry)
            {
                foodFound = true;
                foodToEat = hit.gameObject;

            }
        }
        //objectsHit = Physics.OverlapSphere(transform.position, 0.2f);
        //foreach (Collider hit in objectsHit)
        //{
        //    if (hit.name == "Fish" || hit.name == "LargePredator" && hit != this.gameObject)
        //    {
        //        Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.blue);

        //        distanceToAvoid = 0.5f;
        //        objectToAvoid = hit.gameObject.transform;
        //        direction = (objectToAvoid.position - transform.position + objectToAvoid.position);
        //        GenerateRandomPos();
        //    }

        //}

        objectsHit = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider hit in objectsHit)
        {
            //if (hit.name == "Predator" && hit != this.gameObject)
            //{
            //    hide = true;
            //}

            //avoid obstactle
            if (hit.name == "Obstacle"|| hit.name == "LargePredator" || hit.name == "Worm")
            {
                Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.blue);

                distanceToAvoid = 5f;
                objectToAvoid = hit.gameObject.transform;
                direction = (objectToAvoid.position - transform.position - objectToAvoid.position);
                GenerateRandomPos();
                break;
            }

            else
            {
                avoidingObject = false;
            }


            //if fish is not hungry
            if (!hungry)
            {
                //if fish found
                if (hit.name == "Fish" && hit != this.gameObject)
                {
                    if(numOfFish < 8 && !hungry)
                    {
                        Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.green);
                        objectToFollow = hit.gameObject.transform;
                        direction = (objectToFollow.position - transform.position + objectToFollow.position).normalized;
                    }
                }
            }

            //if fish is hungry
            else if (hungry)
            {
                // if algea found
                if (hit.name == "Algea" && !foodFound)
                {
                    foodToEat = hit.gameObject;
                    resetValues();
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(hit.gameObject.transform.position - transform.position), 3 * Time.deltaTime);
                    Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.magenta);
                    moveTowardsFood(hit.transform);
                    foodFound = true;
                    objectToFollow = hit.gameObject.transform;
                    break;
                }

                else if (foodToEat == null)
                {
                    foodFound = false;
                }
            }
        }

        if(foodToEat != null && hungry)
        {
            moveTowardsFood(foodToEat.transform);
        }

        //if object found avoid object
        else if (objectToAvoid != null)
        {
            avoidObject();
        }

        //if object found move towards object
        else if (objectToFollow != null && !hungry)
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
