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
    private float maxHunger;
    private float matingUrge = 0;
    private float maxMatingUrge = 50;

    public bool female = false;
    public bool mating = false;

    //check collider
    Collider[] objectsHit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Fish")
        {
            numOfFish++;
        }

        if(mating)
        {
            Instantiate(this.gameObject, new Vector3(this.gameObject.transform.position.x, 
                                                    this.gameObject.transform.position.y, 
                                                    this.gameObject.transform.position.z), Quaternion.Euler(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f)));

            mating = false;
            matingUrge = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Fish")
        {
            numOfFish--;
        }
    }
    private void GenerateRandomPos()
    {
        if(!hungry)
        {
            //set radius
            radius = manager.SpawnRadius;
            radiusx = Random.Range(-radius, radius);
            float radiusy = Random.Range(-radius, radius / 2);
            float radiusz = Random.Range(-radius, radius);
            position = new Vector3(radiusx, radiusy, radiusz);
        }

        else if (hungry)
        {
            //set radius
            radius = manager.SpawnRadius;
            radiusx = Random.Range(-radius, radius);
            float radiusy = Random.Range(-radius, radius / 15);
            float radiusz = Random.Range(-radius, radius);
            position = new Vector3(radiusx, radiusy, radiusz);
        }
    }


    void Start()
    {
        GenerateRandomPos();
        this.gameObject.name = "Fish";
        speed = Random.Range(speed, speed * 2);
        turnTime = Random.Range(turnSpeed, turnSpeed * 2);
        maxHunger = 50;
        hunger = Random.Range(-5, 5);
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


        if (gameObject.transform.position.x > radius || gameObject.transform.position.x < -radius ||
            gameObject.transform.position.y > radius || gameObject.transform.position.y < -radius ||
            gameObject.transform.position.z > radius || gameObject.transform.position.z < -radius)
        {
            objectToFollow = null;
            objectToAvoid = null;
        }



        objectsHit = Physics.OverlapSphere(transform.position, 1f);
        if (!hungry)
        {
           // matingUrge++;
  
            foreach (Collider hit in objectsHit)
            {
                    Vector3 targetPoint;
                    Quaternion targetRotation;
                    if (hit.name == "Obstacle" && hit != this.gameObject)
                    {
                    distanceToAvoid = 3f;
                    objectToFollow = null;
                    objectToAvoid = null;
                    Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.green);
                    objectToAvoid = hit.gameObject.transform;
                    direction = (objectToAvoid.position - transform.position - objectToAvoid.position).normalized;
                    

                    Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.green);
                        targetPoint = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z) - transform.position;
                        targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
                        break;
                    }

                    else if (hit.name == "Fish" && hit != this.gameObject)
                    {
                        if (matingUrge > maxMatingUrge
                            && female == false && hit.gameObject.GetComponent<Flock>().female == true ||
                            matingUrge > maxMatingUrge
                            && female == true && hit.gameObject.GetComponent<Flock>().female == false)
                        {
                            objectToFollow = hit.gameObject.transform;
                            direction = (objectToAvoid.position + transform.position + objectToAvoid.position).normalized;
                        }

                        else
                        {
                            //Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.green);
                            objectToFollow = hit.gameObject.transform;
                            direction = (objectToFollow.position - transform.position + objectToFollow.position).normalized;
                        }
                    }

                else if (hit.name == "Predator" && hit != this.gameObject)
                {
                    distanceToAvoid = 15f;
                    speed = 9;
                    objectToFollow = null;
                    objectToAvoid = null;
                    // Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.green);
                    objectToAvoid = hit.gameObject.transform;
                    direction = (objectToAvoid.position - transform.position - objectToAvoid.position).normalized;
                    break;  
                }


                else if (hit.name == "Algae" && hit != this.gameObject && hungry)
                {
                    distanceToAvoid = 1f;
                    objectToFollow = null;
                    objectToAvoid = null;
                    Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.green);

                    //set object to look at
                    // Rotate the camera every frame so it keeps looking at the target
                    objectToFollow = hit.gameObject.transform;
                    direction = (objectToFollow.position - transform.position + objectToFollow.position).normalized;
                    //transform.LookAt(hit.transform);
                }
            }
        }

        //check collider
        //objectsHit = Physics.OverlapSphere(transform.position, 2f);
        //foreach (Collider hit in objectsHit)
        //{
        //    if (hit.name == "Obstacle" && hit != this.gameObject)
        //    {
        //        distanceToAvoid = 5f;
        //        objectToFollow = null;
        //        objectToAvoid = null;
        //        //Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.green);
        //        objectToAvoid = hit.gameObject.transform;
        //        direction = (objectToAvoid.position - transform.position - objectToAvoid.position).normalized;
        //    }

        //    else if (hit.name == "Predator" && hit != this.gameObject)
        //    {
        //        distanceToAvoid = 15f;
        //        speed = 9;
        //        objectToFollow = null;
        //        objectToAvoid = null;
        //        // Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.green);
        //        objectToAvoid = hit.gameObject.transform;
        //        direction = (objectToAvoid.position - transform.position - objectToAvoid.position).normalized;
        //        break;
        //    }


        //    else if (hit.name == "Algae" && hit != this.gameObject && hungry && hit.gameObject != null)
        //    {
        //        distanceToAvoid = 1f;
        //        objectToFollow = null;
        //        objectToAvoid = null;
        //        Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.green);

        //        //set object to look at
        //        // Rotate the camera every frame so it keeps looking at the target
        //        transform.LookAt(hit.transform);
        //    }
        //}

        //if object found
        if (objectToAvoid != null)
        {
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
        }

        //if object found
        else if (objectToFollow != null)
        {
            {
                //set object to look at
                direction = (objectToFollow.position - transform.position + objectToFollow.position).normalized;
                lookRotation = Quaternion.LookRotation(-direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * turnTime);

                lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, objectToFollow.rotation, Time.deltaTime * turnTime / 8);
            }
        }

        //patrol
        else
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





    }
}
 