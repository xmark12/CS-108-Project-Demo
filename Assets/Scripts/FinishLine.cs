using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishLine : MonoBehaviour
{
    public GameObject player;
    public PlayerController playControl;
    public TMP_Text pauseText;
    public TMP_Text winText;
    public Button button;

    public bool stageFinished = false;
    public bool stagePaused = false;
    

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (stagePaused)
        {
            PauseScene();
        }
        if (!stagePaused && !stageFinished)
        {
            ResumeScene();
        }
        else if (!stagePaused && stageFinished)
        {
            
        }
    }

    // This function triggers when clicking the pause button or pressing P.
    public void PauseScene()
    {
        pauseText.gameObject.SetActive(true);
        button.gameObject.SetActive(false);
        stagePaused = true;
        Time.timeScale = 0;
    }

    // This function triggers when clicking continue game or pressing C.
    public void ResumeScene()
    {
        pauseText.gameObject.SetActive(false);
        button.gameObject.SetActive(true);
        stagePaused = false;
        Time.timeScale = 1;
    }

    // This function triggers when clearing the game.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            winText.gameObject.SetActive(true);
            button.gameObject.SetActive(false);
            stageFinished = true;
            Time.timeScale = 0;
        }
    }
}
