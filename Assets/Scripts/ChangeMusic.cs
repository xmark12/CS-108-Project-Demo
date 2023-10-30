using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    public FinishLine fl;

    public AudioSource stageMusic;

    public bool alreadyPlayedStageMusic = false;

    // Start is called before the first frame update
    void Start()
    {
        stageMusic = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fl.stageFinished && alreadyPlayedStageMusic)
        {
            stageMusic.Pause();
            AudioListener.pause = true;
            alreadyPlayedStageMusic = false;
        }
        else if (!fl.stageFinished && !alreadyPlayedStageMusic)
        {
            AudioListener.pause = false;
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().StopMusic();
            stageMusic.Play();
            alreadyPlayedStageMusic = true;
        }
    }
}
