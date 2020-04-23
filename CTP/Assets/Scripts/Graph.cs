using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;

    List<int> valueList = new List<int>();

    private RectTransform graphContainer;

    public float time;
    public float timeToPlaceNewPoint;

    private bool showGraph = false;

    public string nameToTrack;

    public void ToggleGraph()
    {
        if (showGraph == false)
        {
            showGraph = true;
        }

        else
        {
            showGraph = false;
        }
    }

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();

        ShowGraph(valueList);
    }

    //private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    //{
    //    GameObject gameObject = new GameObject("dotConnection", typeof(Image));
    //    gameObject.transform.SetParent(graphContainer, false);
    //    gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    //    RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
    //    Vector2 dir = (dotPositionB - dotPositionA).normalized;
    //    float distance = Vector2.Distance(dotPositionA, dotPositionB);
    //    rectTransform.anchorMin = new Vector2(0, 0);
    //    rectTransform.anchorMax = new Vector2(0, 0);
    //    rectTransform.sizeDelta = new Vector2(distance, 3f);
    //    rectTransform.anchoredPosition = dotPositionA + dir * distance * 0.5f;
    //    rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    //}

    private void CreateCircle(Vector2 anchoredPosition)
    {

        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }

    private void ShowGraph(List<int> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;
        float yMaximum = 100f;
        float xMaximum = 10f;
        float xSize = 800f;

        for (int i = 0; i < valueList.Count; i++)
        {
            foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (gameObj.name == "circle")
                {
                    Destroy(gameObj);
                }
            }
        }

        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = i * xSize / valueList.Count - 1;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            CreateCircle(new Vector2(xPosition, yPosition));
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        if(time > timeToPlaceNewPoint)
        {
            int currentNumOfFish = 0;

            foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (gameObj.name == nameToTrack)
                {
                    currentNumOfFish++;
                }
            }
            valueList.Add(currentNumOfFish);
            ShowGraph(valueList);
            time = 0;
        }
    }
}
