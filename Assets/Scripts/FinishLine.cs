using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishLine : MonoBehaviour
{
    public GameObject player;

    public TMP_Text winText;

    public bool stageFinished = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            winText.gameObject.SetActive(true);
            stageFinished = true;
            Time.timeScale = 0;
        }
    }
}
