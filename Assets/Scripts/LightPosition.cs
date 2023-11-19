using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPosition : MonoBehaviour
{
    [SerializeField] private FieldOfView fieldOfView;
    public float angle = -90f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fieldOfView.SetPosition(transform.position);
        fieldOfView.SetAngle(angle);
    }
}
