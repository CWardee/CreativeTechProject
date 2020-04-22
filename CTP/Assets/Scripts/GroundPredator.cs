using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPredator : MonoBehaviour
{
    //check collider
    Collider[] objectsHit;

    private GameObject target;
    private Vector3 targetPoint;
    private Quaternion targetRotation;

    private void OnTriggerEnter(Collider other)
    {

        //if predator found
        if (other.name == "Fish")
        {
            Destroy(other.gameObject);

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.name = "Worm";
    }

    // Update is called once per frame
    void Update()
    {
        objectsHit = Physics.OverlapSphere(transform.position, 3f);

        foreach (Collider hit in objectsHit)
        {
            if (hit.name == "Fish" && hit != this.gameObject)
            {
                Debug.DrawLine(transform.position, hit.gameObject.transform.position, Color.green);
                targetPoint = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z) - transform.position;
                targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
            }
        }
    }
}
