using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEnable : MonoBehaviour
{
    private bool showGraph = false;
    public GameObject graph;

    public void ToggleGraph()
    {
        if (showGraph == false)
        {
            showGraph = true;
        }

        else if (showGraph)
        {
            showGraph = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(showGraph)
        {
            graph.SetActive(true);
        }

        else if (!showGraph)
        {
            graph.SetActive(false);
        }
    }
}
