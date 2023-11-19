using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject target;
    public float speed;
    Rigidbody2D bulletRB;

    private float timer = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = target.transform.position - transform.position;
        bulletRB.velocity = new Vector2(moveDir.x, moveDir.y).normalized * speed;

        float rot = Mathf.Atan2(-moveDir.y, -moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot); //add +90 to rot if bullet sprite is vertical
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        if (col.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
