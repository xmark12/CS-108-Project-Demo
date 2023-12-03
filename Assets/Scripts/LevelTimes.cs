using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTimes : MonoBehaviour
{
    public TMP_Text levelOneRecord;
    private float highestTimeOne = 70;

    public TMP_Text levelTwoRecord;
    private float highestTimeTwo = 90;
    public Button levelTwoButton;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetFloat("HighestTimeLvl1", 70) >= 70)
        {
            levelOneRecord.text = "Record To Beat: 01 : 10";
        }
        else if (PlayerPrefs.GetFloat("HighestTimeLvl1", 70) < 70)
        {
            highestTimeOne = PlayerPrefs.GetFloat("HighestTimeLvl1", 70);
            float highMinutes = Mathf.FloorToInt(highestTimeOne / 60);
            float highSeconds = Mathf.FloorToInt(highestTimeOne % 60);
            levelOneRecord.text = "Your Record: " + string.Format("{0:00} : {1:00}", highMinutes, highSeconds);
        }

        if (PlayerPrefs.GetFloat("HighestTimeLvl2", 90) >= 90)
        {
            levelTwoRecord.text = "Record To Beat: 01 : 30";
        }
        else if (PlayerPrefs.GetFloat("HighestTimeLvl2", 90) < 90)
        {
            highestTimeTwo = PlayerPrefs.GetFloat("HighestTimeLvl2", 90);
            float highMinutes = Mathf.FloorToInt(highestTimeTwo / 60);
            float highSeconds = Mathf.FloorToInt(highestTimeTwo % 60);
            levelTwoRecord.text = "Your Record: " + string.Format("{0:00} : {1:00}", highMinutes, highSeconds);
        }

        if (PlayerPrefs.GetInt("Level1Finished", 0) != 0)
        {
            levelTwoButton.interactable = true;
        }
        else
        {
            levelTwoButton.interactable = false;
        }
    }
}
