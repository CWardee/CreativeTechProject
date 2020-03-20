using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // State
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;
    Vector3 velocity;

    public Transform objectToFollow;
    private Quaternion lookRotation;


    private Vector3 direction;

    public FlockManager manager;
    float radius;

    public Transform target;

    int numOfFishCollided = 0;

    Quaternion averageFishHeading;


    public float radiusx;

    public List<Transform> allObjects;

    public float speed;
    public float turnSpeed;
    float turnTime;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.name == "Fish")
    //    {
    //        allObjects.Add(other.gameObject.transform);
    //        target = other.transform;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.name == "Fish")
    //    {
    //        allObjects.Remove(other.gameObject.transform);
    //        numOfFishCollided--;
    //        objectToFollow = null;
    //    }
    //}

    private void GenerateRandomPos()
    {
        //set radius
        radius = manager.SpawnRadius;
         radiusx = Random.Range(-radius, radius);
        float radiusy = Random.Range(-radius, radius);
        float radiusz = Random.Range(-radius, radius);
        position = new Vector3(radiusx, radiusy, radiusz);
    }


    void Start()
    {
        GenerateRandomPos();
        this.gameObject.name = "Fish";
        speed = Random.Range(speed, speed * 2);
        turnTime = Random.Range(turnSpeed, turnSpeed * 2);

    }


    void Update()
    {
        if (gameObject.transform.position.x > radius || gameObject.transform.position.x < -radius ||
            gameObject.transform.position.y > radius || gameObject.transform.position.y < -radius ||
            gameObject.transform.position.z > radius || gameObject.transform.position.z < -radius)
        {
            objectToFollow = null;
        }

            //check collider
            Collider[] objectsHit = Physics.OverlapSphere(transform.position, 1f);
            foreach (Collider hit in objectsHit)
            {
                if (hit.name == "Fish" && hit != this.gameObject)
                {
                    ///Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.green);
                    objectToFollow = hit.gameObject.transform;
                    direction = (objectToFollow.position - transform.position + objectToFollow.position).normalized;
                }

                if (hit.name == "Obstacle" && hit != this.gameObject)
                {
                    //Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.green);
                    transform.rotation = Quaternion.LookRotation(transform.position - hit.gameObject.transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, hit.gameObject.transform.rotation, Time.deltaTime * turnTime / 8);

            }
        }







        //go forward
        transform.position += this.gameObject.transform.forward * Time.deltaTime * speed;

        

        //Debug.DrawLine(transform.position, allObjects.position, Color.green);




        //if object found
        if (objectToFollow != null)
        {

            {
                //set object to look at
                direction = (objectToFollow.position - transform.position + objectToFollow.position).normalized;
                lookRotation = Quaternion.LookRotation(-direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * turnTime);

                lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, objectToFollow.rotation, Time.deltaTime * turnTime / 8);


                // transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 25);
            }

        }

        //patrol
        else
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

            objectToFollow = null;
        }



    }
}
