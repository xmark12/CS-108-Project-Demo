using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    //Camera Movement Fields

    [SerializeField]
    GameObject player;

    [SerializeField]
    Vector2 posOffset;

    [SerializeField]
    float timeOffset; //make it 0.05

    private Vector3 velocity;


    //Camera Limit Fields


    //[SerializeField]
    public float leftLimit; //-33 (for intended area), -15.2 (for actual area)
    //[SerializeField]  
    public float rightLimit;  //239 or 175, 221.2, 
    //[SerializeField]
    public float bottomLimit;  //-5, 4
    //[SerializeField]
    public float topLimit; //35, 18


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Camera Movement
        Vector3 startPos = transform.position;
        Vector3 endPos = player.transform.position;
        endPos.x += posOffset.x;
        endPos.y += posOffset.y;
        endPos.z = -10;
        transform.position = Vector3.SmoothDamp(startPos, endPos, ref velocity, timeOffset);

        //Camera Borders

        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
            Mathf.Clamp(transform.position.y, bottomLimit, topLimit),
            transform.position.z
        );

    }

    void OnDrawGizmos()
    {
        //draw a box for visual reference

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(leftLimit, topLimit), new Vector2(rightLimit, topLimit));
        Gizmos.DrawLine(new Vector2(rightLimit, topLimit), new Vector2(rightLimit, bottomLimit));
        Gizmos.DrawLine(new Vector2(rightLimit, bottomLimit), new Vector2(leftLimit, bottomLimit));
        Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(leftLimit, topLimit));

    }
}
