using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuKeybinds : MonoBehaviour
{
    public NewGame ng;
    public NewGame ng1;
    public NewGame ng2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ng.NextScene();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ng1.NextScene();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ng2.NextScene();
        }
    }
}
