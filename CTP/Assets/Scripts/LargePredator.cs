using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargePredator : MonoBehaviour
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

    public int hunger = 0;
    int maxHunger = 100;
    public float speed;
    public float turnSpeed;
    float turnTime;
    float distanceToAvoid;
    public bool hungry = false;

    GameObject targetFish;
    bool fishFound = false;

    //check collider
    Collider[] objectsHit;

    private Vector3 targetPoint;
    private Quaternion targetRotation;

    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.name == "Predator" && hungry)
        {
            Destroy(collision.gameObject);
            hunger = 0;
        }
    }

    //reset all values
    private void resetValues()
    {
        objectToAvoid = null;
        objectToFollow = null;
    }

    //object to avoid
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

    //generate random position for patrol
    private void GenerateRandomPos()
    {
        //set radius
        radius = manager.SpawnRadius;
        radiusx = Random.Range(-radius, radius);
        float radiusy = Random.Range(-radius + 15, radius);
        float radiusz = Random.Range(-radius, radius);
        position = new Vector3(radiusx, radiusy, radiusz);
    }

    void Start()
    {
        //set radius
        radius = manager.SpawnRadius;
        this.gameObject.name = "LargePredator";
        GenerateRandomPos();
        speed = Random.Range(speed, speed * 2);
        turnTime = Random.Range(turnSpeed, turnSpeed * 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hungry)
        {
            hunger++;
        }


        //ensure predator is within bounds
        if (gameObject.transform.position.x > radius || gameObject.transform.position.x < -radius ||
            gameObject.transform.position.y > radius || gameObject.transform.position.y < -radius ||
            gameObject.transform.position.z > radius || gameObject.transform.position.z < -radius)
        {
            objectToFollow = null;
            objectToAvoid = null;
        }


        if (hunger > maxHunger)
        {
            hungry = true;
        }

        else
        {
            hungry = false;
        }

        if (targetFish == null)
        {
            fishFound = false;
        }

        //go forward
        transform.position += this.gameObject.transform.forward * Time.deltaTime * speed;

        //check what is around
        objectsHit = Physics.OverlapSphere(transform.position, 4f);
        foreach (Collider hit in objectsHit)
        {
            //avoid obstactle
            if (hit.name == "Obstacle" && hit != this.gameObject)
            {
                resetValues();
                Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.blue);

                distanceToAvoid = 3f;
                objectToAvoid = hit.gameObject.transform;
                direction = (objectToAvoid.position - transform.position - objectToAvoid.position);
                GenerateRandomPos();
                break;
            }

            //if fish not found
            else if (!fishFound && hungry)
            {
                if (hit.name == "Predator" && hit != this.gameObject && hungry)
                {
                    targetFish = hit.gameObject;
                    fishFound = true;
                    break;
                }
            }
        }


        //if fish found
        if (fishFound && objectToAvoid != null)
        {
            objectToFollow = null;
            objectToFollow = targetFish.transform;

            Debug.DrawLine(transform.position, targetFish.gameObject.transform.position, Color.green);

            if (this.gameObject.transform.position.y > -radius + 15)
            {
                //rotate to look at the player
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetFish.transform.position - transform.position), 3 * Time.deltaTime);
                speed = 9;
            }

            else
            {
                targetFish = null;
                objectToFollow = null;
            }
        }

        else if (objectToAvoid != null)
        {
            avoidObject();
        }

        //patrol
        else if (objectToFollow == null)
        {
            if (Vector3.Distance(position, transform.position) > 4)
            {
                //draw line towards object
                Debug.DrawLine(transform.position, position, Color.green);

                //set object to look at
                direction = (position - transform.position).normalized;
                lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
            }

            else
            {
                GenerateRandomPos();
            }
        }
    }
}
