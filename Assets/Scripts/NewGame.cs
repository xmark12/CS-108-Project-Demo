using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    public string scene;

    public void NextScene()
    {
        SceneManager.LoadScene(scene);

        Time.timeScale = 1;
    }
}
