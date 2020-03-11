using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
    Rigidbody rigidBody;
    public float counter = 0;
    float maxCounter;
    public float speed = 25;
    GameObject leadFish;
    Fish fishScript;

    public bool fishFound = false;
    public bool leaderFish = false;
    public bool predatorNear = false;
    bool nearLeaderFish = false;


    public Vector3 destination;
    Vector3 predatorDirection;
    Rigidbody predatorRig;

    public int currentFlockNum;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(1, 3);
        rigidBody = this.GetComponent<Rigidbody>();
        maxCounter = Random.Range(5, 6);
        counter = maxCounter;
        this.gameObject.name = "Predator";

        var cubeRenderer = this.gameObject.GetComponent<Renderer>();
        cubeRenderer.material.SetColor("_Color", Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        if (counter > maxCounter)
        {
            destination = new Vector3(this.gameObject.transform.position.x + Random.Range(-35, 35),
            this.gameObject.transform.position.y + Random.Range(-15, 15),
            this.gameObject.transform.position.z + Random.Range(-15, 15));

            counter = 0;
            maxCounter = Random.Range(-10.0f, 10.0f);
        }


        this.gameObject.transform.position = Vector3.Lerp(transform.position, destination, 1 * Time.deltaTime);
    }
}
