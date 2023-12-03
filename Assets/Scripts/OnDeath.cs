using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeath : MonoBehaviour
{
    public Vector2 position;
    public bool died = false;

    SpriteRenderer sr;
    Rigidbody2D rb;
    //FieldOfView fov;

    float colorValue = 1f;
    //GameObject cc;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        //fov = GameObject.FindGameObjectWithTag("FoV").GetComponent<FieldOfView>();
        //cc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameObject>();
    }

    void Start()
    {
        position = transform.position;
    }

    void Update()
    {
        //Debug.Log(died);
        if (died)
        {
            Die();
        }
    }

    public void UpdateCheckpoint(Vector2 pos)
    {
        position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            died = true;
            //Die();
        }
    }

    public void Die()
    {
        //cc.anim.Play("DeathScreen");
        StartCoroutine(Respawn(0.5f));
    }

    IEnumerator Respawn(float duration)
    {
        for (int i = 0; i < duration; i++)
        {
            sr.color = new Color(1f, 0f, 0f, colorValue);
            colorValue -= 0.1f;
        }
        rb.simulated = false;
        //fov.playerDetected = false;
        //fov.myrenderer.material.color = new Color(1f, 1f, 1f, 0.2f);
        yield return new WaitForSeconds(duration);
        transform.position = position;
        died = false;
        sr.color = new Color(1f, 1f, 1f, 1f);
        rb.simulated = true;
        colorValue = 1f;
    }
}
