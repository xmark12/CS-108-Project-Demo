using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 0;
    public float highestTime = 0;

    public bool timeIsRunning = true;

    public TMP_Text timeText;
    public TMP_Text highestTimeText;
    public TMP_Text highestTimeText2;

    // Start is called before the first frame update
    void Start()
    {
        timeIsRunning = true;
        highestTimeText.text = "Record: 00 : 00";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeIsRunning)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining += Time.deltaTime;
                DisplayTime(timeRemaining);
            }
        }
    }

    void DisplayTime (float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);

        //highestTimeText.text = "Record: " + string.Format("{0:00} : {1:00}", highestTime, highestTime);
    }
}
