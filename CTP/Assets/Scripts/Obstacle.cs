using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.name == "Fish")
        {
            
            collision.transform.forward = -collision.transform.forward;
           // collision.transform.rotation = Quaternion.LookRotation(transform.position - collision.gameObject.transform.position);
            //collision.transform.rotation = Quaternion.Slerp(transform.rotation, collision.gameObject.transform.rotation, Time.deltaTime * 15555);

        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
