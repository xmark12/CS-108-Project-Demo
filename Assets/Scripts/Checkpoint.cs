using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    OnDeath gameController;
    Collider2D coll;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("Player").GetComponent<OnDeath>();
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameController.UpdateCheckpoint(transform.position);
            coll.enabled = false;
        }
    }
}
