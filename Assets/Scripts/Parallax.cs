using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

	private float length, startPos;
	public GameObject cam;
	public SpriteRenderer sRenderer;
	public float parallexEffect;

	void Start()
	{
		startPos = transform.position.x;
		//length = GetComponent<SpriteRenderer>().bounds.size.x;
		length = sRenderer.bounds.size.x;
	}

	void Update()
	{
		float temp = (cam.transform.position.x * (1 - parallexEffect));
		float dist = (cam.transform.position.x * parallexEffect);

		transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

		Debug.Log("Temp: " + temp);
		Debug.Log("StartPos: " + startPos);
		Debug.Log("Length: " + length);

		if (temp > startPos + length) 
			startPos += length;
		else if (temp < startPos - length) 
			startPos -= length;
	}

}
