using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSource : MonoBehaviour
{
    Fish fishScript;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Fish")
        {
            fishScript = other.GetComponent<Fish>();
            fishScript.currentlyEating = true;


        }
    }

    private void OnTriggerExit(Collider other)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.name = "FoodSource";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
