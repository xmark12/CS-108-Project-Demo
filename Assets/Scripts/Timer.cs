using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    private float highestTime;
    private float timeRemaining = 0;
    private bool finishedBool = false;

    public TMP_Text timeText;
    public TMP_Text highestTimeText;

    public string timeLevel = "HighestTimeLvl1";
    public float levelRecordToBeat = 70f;

    FinishLine fl;

    // Start is called before the first frame update
    void Start()
    {
        fl = GameObject.FindGameObjectWithTag("WinState").GetComponent<FinishLine>();

        //PlayerPrefs.SetFloat(timeLevel, levelRecordToBeat); //reset highest time for debug purposes

        if (PlayerPrefs.GetFloat(timeLevel, levelRecordToBeat) >= levelRecordToBeat)
        {
            highestTime = levelRecordToBeat;
            highestTimeText.text = "Record To Beat: 01 : 30";
        }
        else if (PlayerPrefs.GetFloat(timeLevel, levelRecordToBeat) < levelRecordToBeat)
        {
            highestTime = PlayerPrefs.GetFloat(timeLevel, levelRecordToBeat);
            float highMinutes = Mathf.FloorToInt(highestTime / 60);
            float highSeconds = Mathf.FloorToInt(highestTime % 60);
            highestTimeText.text = "Your Record: " + string.Format("{0:00} : {1:00}", highMinutes, highSeconds);
        }
        Debug.Log("Highest Time: " + highestTime);
    }

    void Update()
    {
        // if stage is finished and time remaining is less than current highest time and 24 hours (86400 seconds)
        if (fl.stageFinished && (timeRemaining < highestTime))
        {
            Debug.Log("Accessed");
            highestTime = timeRemaining;
            PlayerPrefs.SetFloat(timeLevel, timeRemaining);
            float highMinutes = Mathf.FloorToInt(highestTime / 60);
            float highSeconds = Mathf.FloorToInt(highestTime % 60);
            highestTimeText.text = "Your Record: " + string.Format("{0:00} : {1:00}", highMinutes, highSeconds);
        }
        if (fl.stageFinished)
        {
            if (timeLevel == "HighestTimeLvl1")
            {
                finishedBool = true;
                PlayerPrefs.SetInt("Level1Finished", (finishedBool ? 1 : 0));
            }
        }
        else
        {
            finishedBool = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeRemaining += Time.deltaTime;
        DisplayTime(timeRemaining);
    }

    void DisplayTime (float timeToDisplay)
    {
        //timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
