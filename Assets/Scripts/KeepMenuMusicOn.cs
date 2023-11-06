using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepMenuMusicOn : MonoBehaviour
{
    public bool musicIsOn = false;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (!musicIsOn)
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayMusic();
            musicIsOn = true;
        }
    }
}
